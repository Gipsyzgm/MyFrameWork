/*
 *  项目名字：MyFrameWork
 *  创建时间：2020.8.8
 *  描述信息：热更新流程说明。
 *  使用说明：
 *  1：使用ILruntime热更方案 
 *  2: 文件夹Application.streamingAssetsPath路径下没有资源文件的话首次安装也需要对比更新资源
 *  3：判断本地资源版本小于远程版本资源只能更新更高的版本，如果判断本地资源版本不等于远程版本资源的话则可以通过早期Ab包文件还原至更低的版本
 *  4：简述流程，方便问题查找。
 *  5：进入热更页面——判断网络状态——对比版本信息（判断热更或者整包更新）——热更
 *  6：热更——对比MD5文件判断需要下载的资源数量——下载资源并替换Application.persistentDataPath的对应文件-结束
 */
using AssetBundles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class VersionCheckMgr : MonoSingleton<VersionCheckMgr>
{
    /// <summary>
    /// 资源信息
    /// </summary>
    public Dictionary<string, ABResFile> dicABRes = new Dictionary<string, ABResFile>();
    /// <summary>
    /// 资源版本号
    /// </summary>
    public int ResVersion = 0;

    /// <summary>远程服务器版本信息仅用来判断热更和整包更新，只有整包更新时才需要变更</summary>
    private VersionInfo remoteVersion;

    /// <summary>远程服务器AB资源版本信息</summary>
    private ABResInfo remoteABRes;

    /// <summary>StreamingAssets AB资源版本信息</summary>
    private ABResInfo streamingABRes;

    /// <summary>PersistentDataPath AB资源版本信息</summary>
    private ABResInfo persistentABRes;

    /// <summary>需要下载的队列（https://UnityWebRequest.runoob.com/csharp/csharp-queue.html）</summary>
    private Queue<ABResFile> needUpdateList;

    /// <summary>总公需要下载的大小</summary>
    private ulong totalSizeVer = 0;

    /// <summary>已经下载的大小</summary>
    private ulong downSizeVer = 0;

    /// <summary>更新检测是否完成</summary>
    public bool isUpdateCheckComplete = false;

    /// <summary>
    /// 核对版本信息入口初始化
    /// </summary>
    /// <param name="initOK"></param>
    public async Task Check()
    {
      
        if (AppSetting.IsVersionCheck == false) 
        {
            remoteVersion = new VersionInfo();
            UnityEngine.Debug.LogError("不启用版本检测!");
            isUpdateCheckComplete = true;
            return;
        }
             
        PanelMgr.Instance.OpenPanel<VersionCheckPl>();
        PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).VersionInfo.text = "检查网络情况";
        SetVersion();
        //版本验证并更新
        CheckNetWork();
        await new WaitForEndOfFrame();
    }
    /// <summary>
    /// 检查网络
    /// </summary>
    private void CheckNetWork()
    {
        //检查网络
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            object[] args = new object[6];
            args[0] = "错误提示";
            args[1] = "当前没有网络，请检查网络配置!";
            args[2] = "退出";
            args[3] = "重试";
            args[4] = new UnityAction(Application.Quit);
            args[5] = new UnityAction(CheckNetWork);
            //网络异常
            PanelMgr.Instance.OpenPanel<CommonConfirmPl>("", args);
        }
        else
        {
            //检查热更版本
            CheckVersion();
        }     
    }

    /// <summary>
    /// 验证热更版本信息
    /// </summary>
    private async void CheckVersion()
    {
        PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).VersionInfo.text = "检查更新版本";
        //获取远程版本信息
        string versionFilesURL = AppSetting.TestHttpServer + AppSetting.VersionFile;      
        UnityEngine.Debug.LogError("VersionURL:" + versionFilesURL);
        UnityWebRequest request = UnityWebRequest.Get(versionFilesURL);
        await request.SendWebRequest();
        if (request.error != null)
        {
            //请求资源信息错误   
            UnityEngine.Debug.LogError($"URL Error[{versionFilesURL}]:{request.error} ");
            object[] args = new object[6];
            args[0] = "错误提示";
            args[1] = "请求版本数据错误，请检查网络配置!";
            args[2] = "退出";
            args[3] = "重试";
            args[4] = new UnityAction(Application.Quit);
            args[5] = new UnityAction(CheckVersion);
            //网络异常
            PanelMgr.Instance.OpenPanel<CommonConfirmPl>("", args);
        }
        else 
        {
            List<VersionInfo> verInfos = LitJson.JsonMapper.ToObject<List<VersionInfo>>(request.downloadHandler.text);
            string platformName = Utility.GetPlatformName().ToLower();          
            foreach (VersionInfo ver in verInfos)
            {              
                if (ver.Platform.ToLower() == platformName)
                {
                    remoteVersion = ver;
                    remoteVersion.ResURL = remoteVersion.ResURL.TrimEnd('/') + "/";
                    AppSetting.IsCheckVer = ver.IsCheckVer;
                    AppSetting.IsForcedUpdate = ver.IsForcedUpdate;
                }
            }
            if (remoteVersion == null)
            {           
                CheckVersion();
            }
            else 
            {
                //版本号过低,重新下载APK(https://UnityWebRequest.cnblogs.com/yby-blogs/p/4063530.html)
                if (Application.version.CompareTo(remoteVersion.AppVersion) < 0)
                {
                    object[] args = new object[6];
                    args[0] = "版本过低";
                    args[1] = "版本过低，需要重新下载安装包!";
                    args[2] = "退出";
                    args[3] = "下载";
                 
                    args[4] = new UnityAction(Application.Quit);
                    args[5] = new UnityAction(() =>
                    {
                        Application.OpenURL(remoteVersion.AppDownloadURL);
                    });

                    PanelMgr.Instance.OpenPanel<CommonConfirmPl>("", args);
                }
                else //版本号相同
                {
                    //更新本地MD5对比文件ABFiles
                    await loadPersistentABFiles();
                    if (persistentABRes != null)
                        SetVersion(persistentABRes.Version);
                    //本地资源版本较低(本地资源和远程资源版本不同即可更新，小于只能更新更高的版本，不等于可以更新更低的版本)
                    if (persistentABRes == null || persistentABRes.Version < remoteVersion.ResVersion)
                    {
                        //加载远程MD5对比文件ABFiles信息
                        loadRemoteABFiles();
                    }
                    else
                    {
                        PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).VersionInfo.text = "进入游戏...";
                        SetComplate(persistentABRes);
                    }
                }
            }
         
        }    
    }
    /// <summary>
    /// 获取本地资源版本信息
    /// </summary>
    private async Task loadPersistentABFiles()
    {
        //先获取Application.streamingAssetsPath下的MD5对比文件ABFiles
        await loadStreamingABFiles();
        //如果本地的版本大于等于服务器的的版本,则用本地的资源。（不需要更新）否则用PersistentData的资源（需要更新）
        if (streamingABRes != null && streamingABRes.Version >= remoteVersion.ResVersion)
        {
            persistentABRes = streamingABRes;
            UnityEngine.Debug.LogError("Use StreamingABFiles");
        }
        else
        {
            //Application.streamingAssetsPath下没有MD5对比文件ABFiles，就用Application.persistentDataPath(热更资源)路径下的abfiles
            string abFiles = AppSetting.PersistentDataURL + AppSetting.ABFiles;
            UnityWebRequest request = UnityWebRequest.Get(abFiles);
            await request.SendWebRequest();
            if (request.error == null)
            {
                string[] line = request.downloadHandler.text.Split('\n');
                persistentABRes = new ABResInfo(request.downloadHandler.text);
            }
            else
            {
                await loadStreamingABFiles();
                persistentABRes = streamingABRes;
            }
        }
    }

    /// <summary>
    /// 获取StreamingAssets目录AB文件信息
    /// </summary>
    private async Task loadStreamingABFiles()
    {
        string abFiles = AppSetting.StreamingAssetsPath + AppSetting.ABFiles;
        UnityWebRequest request = UnityWebRequest.Get(abFiles);
        await request.SendWebRequest();
        if (request.error == null)
        {
            streamingABRes = new ABResInfo(request.downloadHandler.text, true);
        }
        else
        {
            streamingABRes = null;
            UnityEngine.Debug.LogError("Load StreamingAssets Error:" + request.error);
        }
    }

    /// <summary>
    /// 获取远程AB文件信息
    /// </summary>
    /// <returns></returns>
    private async void loadRemoteABFiles()
    {
        //获取远程版本信息
        string abFiles = remoteVersion.ResURL + AppSetting.ABFiles;
        UnityWebRequest request = UnityWebRequest.Get(abFiles);
        await request.SendWebRequest();
        if (request.error != null)
        {
            UnityEngine.Debug.LogError($"URL Error[{abFiles}]:{request.error} ");
            isUpdateCheckComplete = false;
            //请求资源信息错误重新请求
            object[] args = new object[6];
            args[0] = "错误提示";
            args[1] = "获取远程ABfiles信息失败!";
            args[2] = "退出";
            args[3] = "重试";           
            args[4] = new UnityAction(Application.Quit);
            args[5] = new UnityAction(loadRemoteABFiles);
            PanelMgr.Instance.OpenPanel<CommonConfirmPl>("", args);
            
            return;
        }
        remoteABRes = new ABResInfo(request.downloadHandler.text);
        needUpdateList = new Queue<ABResFile>();
        totalSizeVer = 0;
        int exists = 0;
        if (persistentABRes == null) //全部需要重新下载
        {
            foreach (ABResFile file in remoteABRes.dicFileInfo.Values)
            {
                file.Version = remoteABRes.Version;
                needUpdateList.Enqueue(file);
                totalSizeVer += file.Size;
            }
        }
        else
        {
            ABResFile localFile;
            foreach (ABResFile file in remoteABRes.dicFileInfo.Values)
            {
                //判断本地是否存在同名文件。Trygetvalue(https://blog.csdn.net/qq_38721111/article/details/83508909?utm_medium=distribute.pc_relevant_t0.none-task-blog-BlogCommendFromMachineLearnPai2-1.channel_param&depth_1-utm_source=distribute.pc_relevant_t0.none-task-blog-BlogCommendFromMachineLearnPai2-1.channel_param)
                persistentABRes.dicFileInfo.TryGetValue(file.File, out localFile);
                //比对热更MD5文件(本地资源和远程资源版本不同即可更新，大于只能更新更高的版本，不等于可以更新更低的版本)
                if (localFile == null || (localFile.MD5 != file.MD5 && remoteABRes.Version > localFile.Version))
                {
                    file.Version = remoteABRes.Version;
                    file.isStreaming = false;
                    needUpdateList.Enqueue(file);
                    totalSizeVer += file.Size;
                }
                else
                {
                    //处理已存在的资源文件
                    bool isExists = false;
                    string path;
                    if (localFile.isStreaming)
                    {                      
                        isExists = true;
                    }
                    else
                    {
                        path = Path.Combine(AppSetting.PersistentDataPath, localFile.File);
                        isExists = File.Exists(path);
                    }
                    if (!isExists) //配置里存在，但文件不存在，重新下载
                    {
                        file.Version = remoteABRes.Version;
                        file.isStreaming = false;
                        needUpdateList.Enqueue(file);
                        totalSizeVer += file.Size;
                        exists++;
                    }
                    else
                    {
                        //配置里存在文件也存在。版本不一致的话区分资源写入位置
                        if (localFile.Version != remoteABRes.Version)
                        {
                            file.isStreaming = localFile.isStreaming;
                            file.Version = localFile.Version;
                        }
                    }
                }
            }
        }
        if (needUpdateList.Count == 0)
        {
            SetComplate(persistentABRes);
        }
        else
        {
            object[] args = new object[6];
            args[0] = "需要更新资源";
            args[1] = "更新资源数："+needUpdateList.Count+"\n需要更新资源大小：" + getSizeString(totalSizeVer);
            args[2] = "更新";
            args[3] = "退出";
            args[4] = new UnityAction(() =>
            {
                UpdateABFile();
            });
            args[5] = new UnityAction(Application.Quit);
            PanelMgr.Instance.OpenPanel<CommonConfirmPl>("", args);          
        }
    }
    /// <summary>
    /// 开始更新资源
    /// </summary>
    private async void UpdateABFile()
    {
        float value = (downSizeVer) / (float)totalSizeVer;
        //每下载downloadNum个数的文件更新一次Ui
        int downloadNum = Math.Min(5, needUpdateList.Count);
        //需要下载的文件list
        List<ABDownLoad> downList = new List<ABDownLoad>();
        ABDownLoad downLoad;
        ABResFile file;
        //已完成的下载
        ulong loadCompleteSize = 0;
        //正在进行的下载
        ulong loadingSize = 0;   
        //已经下载的总大小
        ulong totalLoadSize = 0;
        WaitForUpdate waitUpdate = new WaitForUpdate();
        bool isAllLoadComplate = false;
        //计算已下载的文件数
        int newDownloadSuccFile = 0; 
        for (int i = 0; i < downloadNum; i++)
        {
            downLoad = new ABDownLoad(remoteVersion.ResURL);
            downList.Add(downLoad);
        }
        //Stopwatch(https://blog.csdn.net/qq_33574890/article/details/83344307)
        while (!isAllLoadComplate)
        {
            loadingSize = 0;
            for (int i = downList.Count; --i >= 0;)
            {               
                downLoad = downList[i];
                //如果可以下载
                if (downLoad.IsCanDownload)
                {                 
                    //加到下载完成列表
                    if (downLoad.IsDownload) 
                    {                     
                        newDownloadSuccFile++;
                    }                
                    loadCompleteSize += downLoad.ByteDownloaded;
                    //还有资源没下载完成
                    if (needUpdateList.Count > 0)
                    {                      
                        file = needUpdateList.Dequeue();                      
                        downLoad.DownloadAsync(file);
                        await waitUpdate;
                    }
                    else
                    {
                        downList.RemoveAt(i);
                        break;
                    }
                }
                else
                {                 
                    loadingSize += downLoad.ByteDownloaded;
                }
            }
            //当前下载的总大小
            totalLoadSize = loadCompleteSize + loadingSize + downSizeVer;
            string str = string.Format("{0}/{1}", getSizeString(totalLoadSize), getSizeString(totalSizeVer));
            PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).VersionInfo.text = "正在下载中...";
            PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).SpeedText.text = str;
            SetValue((totalLoadSize) / (float)totalSizeVer);
            await waitUpdate;
            if (downList.Count == 0)
            {
                saveABFileInfo(true);
                isAllLoadComplate = true;             
                await new WaitForSeconds(0.5f);
                SetComplate(remoteABRes);
            }
        }
    }
    /// <summary>
    /// 设置热更进度条
    /// </summary>
    /// <param name="val"></param>
    /// <param name="immediately"></param>
    public void SetValue(float val, bool immediately = false)
    {
        if (immediately)
        {
            PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).ImageSlider.fillAmount = val;
        }
        else
        {
            if (val < PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).ImageSlider.fillAmount && val != 0) return;
            PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).ImageSlider.fillAmount = val;
        }
              
    }
    /// <summary>
    /// 保存ABFile信息，只在最后更新完写一次即可
    /// </summary>
    private void saveABFileInfo(bool isAllUpdate)
    {
        string path = Path.Combine(AppSetting.PersistentDataPath, AppSetting.ABFiles);
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            StreamWriter sWriter = new StreamWriter(fs);//Encoding.GetEncoding("UTF-8")
            StringBuilder sb = new StringBuilder();
            sb.AppendLine((isAllUpdate ? remoteABRes.Version : 0) + "|" + remoteABRes.VersionData);      
            foreach (ABResFile file in remoteABRes.dicFileInfo.Values)
            {
                sb.AppendLine(file.GetFileString());
            }
            sWriter.Write(sb);
            sWriter.Flush();
            sWriter.Close();
        }
    }

    /// <summary>
    /// ui上显示设置版本号
    /// </summary>
    public void SetVersion(int resVersion = 0)
    {
        string ver = Application.version;
        if (resVersion > 0)
            ver += "." + resVersion;
        //checkUI.txtVersion.text = ver;
    }

    /// <summary>
    /// 计算文件大小
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private string getSizeString(ulong size)
    {
        if (size > 1048576)
            return (size / 1048576f).ToString("f2") + "M";
        else
            return (size / 1024f).ToString("f2") + "K";
    }
    /// <summary>
    /// 设置更新完成状态,把AB资源信息存一下
    /// </summary>
    /// <param name="abRes"></param>
    private void SetComplate(ABResInfo abRes)
    {
        ResVersion = abRes.Version;
        foreach (ABResFile file in abRes.dicFileInfo.Values)
        {
            if (file.isStreaming)
                AssetBundleManager.assetBundleURL.Add(file.File, AppSetting.StreamingAssetsPath + file.File);
            else
                AssetBundleManager.assetBundleURL.Add(file.File, AppSetting.PersistentDataURL + file.File);
        }
        if (remoteABRes != null)
            remoteABRes.Dispose();
        if (streamingABRes != null)
            streamingABRes.Dispose();
        if (persistentABRes != null)
            persistentABRes.Dispose();
        PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).VersionInfo.text = "更新完成";
        isUpdateCheckComplete = true;
    }
}
