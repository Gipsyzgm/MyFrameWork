using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UObject = UnityEngine.Object;

public class ABMgr : MonoSingleton<ABMgr>
{
    public static Dictionary<string, string> assetBundleURL = new Dictionary<string, string>();
    public static string BaseDownloadingURL = GetRelativePath();
    public static AssetBundleManifest AssetBundleManifest = null;

    static Dictionary<string, UnityWebRequest> m_DownloadingWWWs = new Dictionary<string, UnityWebRequest>();
    static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
    static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

    public void Initialize()
    {
        var go = new GameObject("ABMgr", typeof(ABMgr));
        DontDestroyOnLoad(go);
        string PlatformName = Utility.GetPlatformName();
        string url;
        if (!assetBundleURL.TryGetValue(PlatformName,out url))
            url = BaseDownloadingURL + PlatformName;
        Debug.LogError(url);
        AssetBundle manifestAB = AssetBundle.LoadFromFile(url);
        AssetBundleManifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        Debug.LogError("多少个资源："+AssetBundleManifest.GetAllAssetBundles().Length);

    }

    public async Task<GameObject> LoadPrefab(string assetBundleName, string assetName = null)
    {
        if (string.IsNullOrEmpty(assetName))
            assetName = assetBundleName.Substring(assetBundleName.LastIndexOf("/") + 1);
        assetBundleName += ".prefab";
        assetBundleName = assetBundleName.ToLower();
        assetBundleName += AppSetting.ExtName;
        Debug.LogError("最终位置：" + assetBundleName + "最后的名字：" + assetName);
        CheckDependencies(assetBundleName);
        string url;
        if (!assetBundleURL.TryGetValue(assetBundleName, out url))
            url = BaseDownloadingURL + assetBundleName;
        Debug.LogError(url);
        AssetBundle assetBundle = AssetBundle.LoadFromFile(url);
        Object obj = await assetBundle.LoadAssetAsync<GameObject>(assetName);    
        return (GameObject)obj;
    }

    public void CheckDependencies(string assetBundleName) 
    {
        string[] strs = AssetBundleManifest.GetAllDependencies(assetBundleName);
        Debug.LogError("依赖资源数："+strs.Length);
        foreach (var name in strs)
        {
            Debug.LogError(name);
            string url;
            if (!assetBundleURL.TryGetValue(name, out url))
                url = BaseDownloadingURL + name;
            Debug.LogError(url);        
            AssetBundle.LoadFromFile(url);
        }
    }
    #region 私有协同方法
    /// <summary>
    /// 异步加载资源
    /// </summary>
    //private async Task<T> _LoadAsset<T>(string assetBundleName, string assetName, bool isWait = false) where T : UObject
    //{

    //   // T obj = request.GetAsset<T>();
    //    if (obj == null)
    //        Debug.LogError($"加载资源失败:{assetBundleName}  AssName:{assetName}");
    //    return obj;
    //}

    private WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
    /// <summary>
    /// 异步加载场景
    /// </summary>
    //private async Task _LoadScene(string sceneAssetBundle, string levelName, bool isAdditive, Action<float> cbProgress)
    //{
    //    //float startTime = Time.realtimeSinceStartup;
    //    AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(sceneAssetBundle, levelName, isAdditive);
    //    if (request != null && cbProgress != null)
    //    {
    //        while (request.Progress() < 1f)
    //        {
    //            await waitFrame;
    //            cbProgress(request.Progress());
    //            if (request.IsDone())
    //                break;
    //        }
    //        cbProgress(1f);
    //    }
    //    //float elapsedTime = Time.realtimeSinceStartup - startTime;
    //    Utils.ResetShader(null);
    //    //Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
    //}
    #endregion
    public static string GetRelativePath()
    {
        if (!AppSetting.IsRelease)
            return "file://" + AppSetting.ExportResBaseDir + Utility.GetPlatformName() + "/";
        return string.Empty;
    }
}

