using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

    public class ABDownLoad
    {
        UnityWebRequest request;
        bool isDownload = false;
        WaitForSeconds dowLoadWait = new WaitForSeconds(2f);
        public ABResFile ABFile;
        private string rootURL;
        private int downErrorNum = 0;
        public ABDownLoad(string rooturl)
        {
            rootURL = rooturl;
        }
        public ulong ByteDownloaded
        {
            get
            {
                if (request == null|| request.downloadProgress==0)
                    return 0;
                return request.downloadedBytes;
            }
        }
        public bool IsCanDownload => isDownload || request == null;

        public bool IsDownload => isDownload;

        public byte[] Data => request.downloadHandler.data;


        public async void DownloadAsync(ABResFile file, bool isReLoad = false)
        {
            ABFile = file;
            isDownload = false;
            if(!isReLoad)
                downErrorNum = 0;
            //资源地址拼接
            string url = rootURL + ABFile.File;
            request = UnityWebRequest.Get(url);
            await request.SendWebRequest();
            if (request.error != null) //文件下载失败
            {
                Debug.LogError($"DownLoad Error:{url}" + " " + request.error);
                downErrorNum++;
                await dowLoadWait;
                DownloadAsync(file, true); //尝试重新下载
                if (downErrorNum >= 5)
                {
                    downErrorNum = 0;
                    PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).VersionInfo.text = string.Format("下载文件失败，可能网络情况不佳,正在重新下载!!!\n{0}", file.File);
                }
                return;
            }
            //下载的文件MD5和配置表里的MD5文件对比
            string downMD5 = MD5Utils.MD5ByteFile(Data);
            if (downMD5 == file.MD5)
            {
                try
                {
                    //保存文件
                    string path = Path.Combine(AppSetting.PersistentDataPath, file.File);
                    FileInfo info = new FileInfo(path);
                    //不存在文件夹则创建对应文件夹
                    if (!Directory.Exists(info.DirectoryName))
                        Directory.CreateDirectory(info.DirectoryName);
                    //指定位置创建文件，如果文件存在，将被改写
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        fs.Write(Data, 0, Data.Length);
                        fs.Flush();
                        fs.Close();
                    }                   
                    isDownload = true;                
            }
                catch (Exception ex)
                {
                    Debug.LogError($"文件保存失败！！！！{file.File}\n{ex.Message}");
                    await dowLoadWait;
                    DownloadAsync(file, true);     //尝试重新下载
                }
            }
            else //MD5效验失败
            {
                downErrorNum++;
                if (downErrorNum >= 5)
                {
                    downErrorNum = 0;
                    PanelMgr.Instance.GetPanel<VersionCheckPl>(PanelName.VersionCheckPl).VersionInfo.text = string.Format("文件MD5效验失败，正在重新下载\n{0}", file.File);
                }
                Debug.LogError($"文件MD5值错误 配置MD5:{downMD5}  实际下载MD5:{file.MD5}");
                await dowLoadWait;
                DownloadAsync(file, true);     //尝试重新下载
            }
        }


}
