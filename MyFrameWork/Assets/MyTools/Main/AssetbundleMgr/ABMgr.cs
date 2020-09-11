using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UObject = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

//所有拼接的assetBundleName都是BundleRes后的全路径。
public class ABMgr : MonoSingleton<ABMgr>
{
    /// <summary>
    /// 所有热更资源的路径
    /// </summary>
    public static Dictionary<string, string> assetBundleURL = new Dictionary<string, string>();
    public static AssetBundleManifest AssetBundleManifest = null;
    static string[] ActiveVariants = { };
    /// <summary>
    /// 所有热更资源对应的依赖
    /// </summary>
    static Dictionary<string, string[]> Dependencies = new Dictionary<string, string[]>();
    //已经下载的AssetBundles数据
    public static Dictionary<string, LoadedAssetBundle> LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
  
    public void Initialize()
    {
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor == false)
        {
            return;
        }     
#endif

        string PlatformName = Utility.GetPlatformName();
        string url;
        if (!assetBundleURL.TryGetValue(PlatformName,out url))
            url = GetRelativePath() + PlatformName;
        Debug.LogError(url);
        AssetBundle manifestAB = AssetBundle.LoadFromFile(url);
        AssetBundleManifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        Debug.LogError("多少个资源："+AssetBundleManifest.GetAllAssetBundles().Length);

    }


#if UNITY_EDITOR
    public static string SimulateAssetBundlesKey = "SimulateAssetBundles";
    // 使用本地资源（true）/模拟ab资源（false）
    public static bool SimulateAssetBundleInEditor
    {
        get
        {
            return EditorPrefs.GetBool(SimulateAssetBundlesKey, false);

        }
        set
        {
            EditorPrefs.SetBool(SimulateAssetBundlesKey, value);
        }
    }
#endif

    /// <summary>
    /// 加载场景
    /// 场景名
    /// </summary>
    /// <param name="sceneName">场景名大小写要和场景名一致</param>
    /// <param name="isAdditive">是否叠加场景</param>
    /// <param name="cbProgress">进度回调</param>
    public void LoadScene(string sceneName, bool isAdditive = false, Action<float> cbProgress = null)
    {

        string sceen = sceneName;
        string assetBundleName = "scene/" + sceneName.ToLower();
        if (!assetBundleName.EndsWith(AppSetting.ExtName))
            assetBundleName += AppSetting.ExtName;
       
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor == false)
        {
            string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, sceen);
            if (levelPaths.Length == 0)
            {
                ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
                //        from that there right scene does not exist in the asset bundle...

                Debug.LogError("There is no scene with name \"" + sceen + "\" in " + assetBundleName);
                return;
            }
            if (isAdditive)
                EditorSceneManager.LoadSceneAsyncInPlayMode(levelPaths[0], new LoadSceneParameters(LoadSceneMode.Additive));
            else
                EditorSceneManager.LoadSceneAsyncInPlayMode(levelPaths[0], new LoadSceneParameters(LoadSceneMode.Single));
            return;
        }
        else
#endif
        {
            assetBundleName = RemapVariantName(assetBundleName);
            AssetBundle assetBundle = LoadAssetBundle(assetBundleName);
            if (assetBundle != null)
            {
                SceneManager.LoadSceneAsync(sceen, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            }
            else
            {
                Debug.LogError($"加载场景失败:{sceen}  AssetName:{assetBundleName}");
            }
        }
    }

    /// <summary>
    /// 加载图集
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    public SpriteAtlas LoadSpriteAtlas(string assetBundleName)
    { 
        string assetName = assetBundleName.Substring(assetBundleName.LastIndexOf("/") + 1).ToLower();
        assetBundleName = (AppSetting.UIAtlasDir + assetBundleName).ToLower()+AppSetting.ExtName;
        return LoadAsset<SpriteAtlas>(assetBundleName,assetName);
    }
    /// <summary>
    /// 加载预制体
    /// BundleRes后的全路径。
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public GameObject LoadPrefab(string assetBundleName,string assetName = null)
    {      
        if (string.IsNullOrEmpty(assetName))        
            assetName = assetBundleName.Substring(assetBundleName.LastIndexOf("/") + 1).ToLower();                  
        assetBundleName = assetBundleName.ToLower(); 
        assetBundleName += AppSetting.ExtName;
        GameObject obj = LoadAsset<GameObject>(assetBundleName, assetName);
        return obj;
    }
/// <summary>
/// 加载资源
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="assetBundleName"></param>
/// <param name="assetName"></param>
/// <param name="isWait"></param>
/// <returns></returns>
    public T LoadAsset<T>(string assetBundleName, string assetName = null, bool isWait = false) where T : UObject
    {

#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor == false)
        {
            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
            if (assetPaths.Length == 0)
            {
                Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                return null;
            }
            // @TODO: Now we only get the main object from the first asset. Should consider type also.
            UObject target = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
            return target as T;
        }
        else
#endif

        {
            AssetBundle assetBundle = LoadAssetBundle(assetBundleName);
            T obj = (T)assetBundle.LoadAssetAsync<T>(assetName).asset;
            if (obj == null)
                Debug.LogError($"加载资源失败:{assetBundleName}  AssName:{assetName}");
            return obj;
        }
       
    }

    /// <summary>
    /// 加载AssetBundle
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    private AssetBundle LoadAssetBundle(string assetBundleName) 
    {
        AssetBundle assetBundle;
        LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName);
        if (bundle == null)
        {
            Debug.LogError("bundle为空");
            CheckDependencies(assetBundleName);
            string url;
            if (!assetBundleURL.TryGetValue(assetBundleName, out url))
                url = GetRelativePath() + assetBundleName;
            Debug.LogError(url);
            assetBundle = AssetBundle.LoadFromFile(url);
            Debug.LogError("存的名字：" + assetBundleName);
            LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(assetBundle));
        }
        else
        {
            assetBundle = bundle.m_AssetBundle;
        }
        return assetBundle;
    }


    /// <summary>
    /// 获取已加载的AssetBundle，只有当所有依赖项下载成功时才返回有效对象.
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    static public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName)
    {
        LoadedAssetBundle bundle = null;
        Debug.LogError("取的名字:"+assetBundleName);
        LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
        if (bundle == null)
            return null;
        Debug.LogError("已下载");
        // 不记录依赖项，只需要bundle本身.
        string[] dependencies = null;
        if (!Dependencies.TryGetValue(assetBundleName, out dependencies))
            return bundle;
        Debug.LogError("dependencies不全");
        // 确保已加载所有依赖项
        foreach (var dependency in dependencies)
        {
            // 等待加载所有依赖资产包。
            LoadedAssetBundle dependentBundle;
            LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null)
                return null;
            Debug.Log(dependency.Length + ":" + dependentBundle.m_AssetBundle.name);
        }
        return bundle;
    }

    /// <summary>
    /// 检查并处理依赖项
    /// </summary>
    /// <param name="assetBundleName"></param>
    public void CheckDependencies(string assetBundleName) 
    {
        if (AssetBundleManifest == null)
        {
            Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
            return;
        }
        string[] dependencies = AssetBundleManifest.GetAllDependencies(assetBundleName);
        if (dependencies.Length == 0)
            return;
        for (int i = 0; i < dependencies.Length; i++)
            dependencies[i] = RemapVariantName(dependencies[i]);
        // Record and load all dependencies.
        Dependencies.Add(assetBundleName, dependencies);
        for (int i = 0; i < dependencies.Length; i++)
            LoadAssetBundleInternal(dependencies[i]);
    }
    //都是本地资源，不走异步。
    static protected void LoadAssetBundleInternal(string assetBundleName)
    {
        // Already loaded.
        LoadedAssetBundle bundle = null;
        LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
        if (bundle != null)
        {
            bundle.m_ReferencedCount++;
            return;
        }
        AssetBundle download = null;
        string url;
        if (!assetBundleURL.TryGetValue(assetBundleName, out url))
            url = GetRelativePath() + assetBundleName;
        Debug.LogError("下载地址：" + url);
        download = AssetBundle.LoadFromFile(url);
        //download = UnityWebRequestAssetBundle.GetAssetBundle(url, m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0);
        Debug.LogError("存的名字："+ assetBundleName);
        LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(download)); 
    }

    static protected string RemapVariantName(string assetBundleName)
    {
        //GetAllAssetBundlesWithVariant获取最合适的assetBundle
        string[] bundlesWithVariant = AssetBundleManifest.GetAllAssetBundlesWithVariant();
        string[] split = assetBundleName.Split('.');
        int bestFit = int.MaxValue;
        int bestFitIndex = -1;
        //使用variant循环所有assetBundle以找到最适合的variant assetBundle.
        for (int i = 0; i < bundlesWithVariant.Length; i++)
        {
            string[] curSplit = bundlesWithVariant[i].Split('.');
            if (curSplit[0] != split[0])
                continue;
            int found = System.Array.IndexOf(ActiveVariants, curSplit[1]);
            // 如果没有找到有效变体。我们还是要用第一个 
            if (found == -1)
                found = int.MaxValue - 1;

            if (found < bestFit)
            {
                bestFit = found;
                bestFitIndex = i;
            }
        }
        if (bestFit == int.MaxValue - 1)
        {
            Debug.LogWarning("Ambigious asset bundle variant chosen because there was no matching active variant: " + bundlesWithVariant[bestFitIndex]);
        }
        if (bestFitIndex != -1)
        {
            return bundlesWithVariant[bestFitIndex];
        }
        else
        {
            return assetBundleName;
        }
    }

    // 卸载ab资源和它的依赖
    public void UnloadAssetBundle(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();
        assetBundleName += AppSetting.ExtName;
        UnloadAssetBundleInternal(assetBundleName);
        UnloadDependencies(assetBundleName);
    }

    static protected void UnloadDependencies(string assetBundleName)
    {
        string[] dependencies = null;
        if (!Dependencies.TryGetValue(assetBundleName, out dependencies))
            return;
        // Loop dependencies.
        foreach (var dependency in dependencies)
        {
            UnloadAssetBundleInternal(dependency);
        }
        Dependencies.Remove(assetBundleName);
    }

    static protected void UnloadAssetBundleInternal(string assetBundleName)
    {
        LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName);

        if (bundle == null)
            return;
        if (--bundle.m_ReferencedCount == 0)
        {
            bundle.m_AssetBundle.Unload(false);
            LoadedAssetBundles.Remove(assetBundleName);
            Debug.LogError( assetBundleName + " has been unloaded successfully");
        }
    }
    /// <summary>
    /// 获取AB资源的路径真实路径
    /// 非正式包的话用StreamingAssetsPath的资源
    /// </summary>
    /// <returns></returns>
    public static string GetRelativePath()
    {
        if (!AppSetting.IsRelease && SimulateAssetBundleInEditor)
            return AppSetting.StreamingAssetsPath;
        //if (!AppSetting.IsRelease && !SimulateAssetBundleInEditor)
        //    return "file://" + AppSetting.ExportResBaseDir + Utility.GetPlatformName() + "/";
        return string.Empty;
    }

}
// 加载的资产包包含引用计数，可用于自动卸载依赖的资产包.
public class LoadedAssetBundle
{
    public AssetBundle m_AssetBundle;
    //移动被引用多少次
    public int m_ReferencedCount;
    public LoadedAssetBundle(AssetBundle assetBundle)
    {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = 1;
    }
}

