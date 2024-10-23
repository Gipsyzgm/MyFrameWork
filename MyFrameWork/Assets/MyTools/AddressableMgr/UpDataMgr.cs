using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class UpDataMgr : MonoBehaviour
{
    public Slider LoadProgress;

    public Text Status;
    void Start()
    {
        Addressables.InternalIdTransformFunc = InternalIdTransformFunc;
        //检查网络
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {

            Status.text = "网络连接失败...";
        }
        else
        {
            //更新Catalog文件
            CheckUpdate();
        }
     
    }
    //重新定向一下资源路径
    private string InternalIdTransformFunc(UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation location)
    {
        //判定是否是一个AB包的请求
        if (location.Data is AssetBundleRequestOptions)
        {
            //PrimaryKey是AB包的名字
            //path就是StreamingAssets/Bundles/AB包名.bundle,其中Bundles是自定义文件夹名字,发布应用程序时,、
#if UNITY_EDITOR
            var path = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, "StandaloneWindows64", location.PrimaryKey);//Path.Combine(Application.streamingAssetsPath, "Bundles", location.PrimaryKey);
#else
            var path = string.Format("{0}/{1}/{2}", Addressables.RuntimePath, "StandaloneWindows64", location.PrimaryKey);
#endif
      
            if (File.Exists(path))
            {
                return path;
            }
        }
        return location.InternalId;
    }
    async void CheckUpdate()
    {
        Status.text = "正在检测资源更新...";
        //初始化Addressable
        var init = Addressables.InitializeAsync();
        await init.Task;
        //开始连接服务器检查更新
        AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates(false);
        await checkHandle.Task;
        //检查结束，验证结果 
        if (checkHandle.Status == AsyncOperationStatus.Succeeded)
        {
            List<string> catalogs = checkHandle.Result;
            if (catalogs != null && catalogs.Count > 0)
            {
                Debug.Log("download catalogs start");
                var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                await updateHandle.Task;
                Debug.Log("download catalogs finish");
                List<object> keys = new List<object>();
                foreach (var item in updateHandle.Result)
                {
                    Debug.Log(item.LocatorId);

                    foreach (var key in item.Keys)
                    {
                        Debug.Log(item.LocatorId+":"+key);
                    }
                    keys.AddRange(item.Keys);
                }
                // 获取下载内容的大小
                var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
                await sizeHandle.Task;
                long totalDownloadSize = sizeHandle.Result;
                Debug.Log("下载大小：" + totalDownloadSize);
                if (totalDownloadSize > 0)
                {

                    LoadProgress.gameObject.SetActive(true);
                    Status.text = "正在更新中";
                    Debug.Log("download bundle start");
                    var downloadHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);                 
                    while (!downloadHandle.IsDone)
                    {
                        float percent = downloadHandle.PercentComplete;
                        LoadProgress.value = percent;                     
                        await Task.Yield();
                    }
                    LoadProgress.value = downloadHandle.PercentComplete;
                    Addressables.Release(downloadHandle);
     
                    Debug.Log("download bundle finish");
                }
                else
                {
                    Debug.Log("不需要更新");
                }
                Addressables.Release(updateHandle);
                //Debug.Log("download bundle start");
                //foreach (var item in updateHandle.Result)
                //{
                //    var sizeHandle = Addressables.GetDownloadSizeAsync(item.Keys);
                //    await sizeHandle.Task;
                //    if (sizeHandle.Result>0)
                //    {
                //        var downloadHandle = Addressables.DownloadDependenciesAsync(item.Keys, Addressables.MergeMode.Union);
                //        await downloadHandle.Task;
                //    }
                //}
                //Debug.Log("download bundle finish");
            }
            else 
            {
                Debug.Log("不需要更新");
            } 
        }
        Addressables.Release(checkHandle);
        Debug.Log("CheckUpdate结束");
        Status.text = "更新完成";
        Create();
    }
    /// <summary>
    /// 这个方法和Addressables.UpdateCatalogs获取到的资源是一致的。
    /// </summary>
    //async void StartUpdate()
    //{
    //    Debug.Log("开始更新资源");
    //    IEnumerable<IResourceLocator> locators = Addressables.ResourceLocators;
    //    foreach (var item in locators)
    //    {
    //        var sizeHandle  =  Addressables.GetDownloadSizeAsync(item.Keys);
    //        await sizeHandle.Task;
    //        if (sizeHandle.Result > 0) 
    //        {
    //            var downloadHandle = Addressables.DownloadDependenciesAsync(item.Keys, Addressables.MergeMode.Union);
    //            await downloadHandle.Task;
    //        }             
    //    }
    //    Debug.Log("更新完成");
    //}



    async void Create()
    {
        Debug.Log("开始加载");
        var handle = Addressables.LoadAssetAsync<GameObject>("tree1");
        await handle.Task;
        GameObject obj = Instantiate(handle.Result);

        obj.transform.position = Vector3.zero;
        obj.transform.localScale = Vector3.one * 10;
        Debug.Log("加载成功1");
        var handle2 = Addressables.LoadAssetAsync<GameObject>("tree2");
        await handle2.Task;
        GameObject obj2 = Instantiate(handle2.Result);
        obj2.transform.position = Vector3.one;
        obj2.transform.localScale = Vector3.one * 10;
        Debug.Log("加载成功2");
    }
   

   

}
