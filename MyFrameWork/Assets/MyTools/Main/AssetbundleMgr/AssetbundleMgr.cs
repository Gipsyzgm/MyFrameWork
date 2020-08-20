using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UObject = UnityEngine.Object;
using UnityEngine.U2D;
using AssetBundles;
using System.Threading.Tasks;

    /// <summary>
    /// 资源包管理器
    /// 全部资源包加载都使用异步加载
    /// </summary>
    public class AssetbundleMgr : MonoSingleton<AssetbundleMgr>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="initOK"></param>
        public async Task Initialize()
        {
            Debug.LogError("AssetbundleMgrInitializeStart");        
            AssetBundleManager.logMode = AssetBundleManager.LogMode.All;
            AssetBundleManager.BaseDownloadingURL = GetRelativePath();
            Debug.LogError("BaseDownloadingURL:" + AssetBundleManager.BaseDownloadingURL);
            AssetBundleLoadManifestOperation request = AssetBundleManager.Initialize();
            if (request == null) return;          
            Debug.LogError("AssetbundleMgrInitializeStartAHarf");
            //await request;    
            while (AssetBundleManager.AssetBundleManifestObject == null)
            {
                await new WaitForEndOfFrame();
            }
            Debug.LogError("AssetbundleMgrInitialized");       
        }
    
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetBundleName">完整资源包名,注：带文件扩展名</param>
        /// <param name="assetName">资源名</param>
        public async Task<T> LoadAsset<T>(string assetBundleName, string assetName) where T : UObject
        {
            assetBundleName = assetBundleName.ToLower();
            assetBundleName += AppSetting.ExtName;
            return await _LoadAsset<T>(assetBundleName, assetName);
        }

        /// <summary>
        /// 异步加载预制
        /// </summary>
        /// <param name="assetBundleName">资源包名</param>
        /// <param name="assetName">资源名,不填自动获取资源包最后的名字</param>
        public async Task<GameObject> LoadPrefab(string assetBundleName, string assetName=null)
        {
            if (string.IsNullOrEmpty(assetName))
                assetName = assetBundleName.Substring(assetBundleName.LastIndexOf("/")+1);
            assetBundleName += ".prefab";
            assetBundleName = assetBundleName.ToLower();
            assetBundleName += AppSetting.ExtName;
            Debug.LogError("最终位置："+assetBundleName+"最后的名字："+assetName);
            GameObject obj = await _LoadAsset<GameObject>(assetBundleName, assetName, true);
            GameObject rtn  = Instantiate(obj) as GameObject;
            Utils.ResetShader(rtn);
            return rtn;
        }
        /// <summary>
        /// 异步加载图集
        /// </summary>
        /// <param name="assetBundleName"></param>
        public async Task<SpriteAtlas> LoadSpriteAtlas(string assetBundleName)
        {
            string assetName = assetBundleName.Substring(assetBundleName.LastIndexOf("/") + 1);
            assetBundleName = AppSetting.UIAtlasDir + assetBundleName + ".spriteatlas";            
            return await LoadAsset<SpriteAtlas>(assetBundleName, assetName);
        }
        /// <summary>
        /// 异步加载材质球
        /// </summary>
        /// <param name="assetBundleName"></param>
        public async Task<Material> LoadMaterial(string assetBundleName)
        {
            string assetName = assetBundleName.Substring(assetBundleName.LastIndexOf("/") + 1);
            assetBundleName = "Materials/" + assetBundleName + ".mat";
            return await LoadAsset<Material>(assetBundleName, assetName);
        }
        
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="isAdditive">是否叠加场景</param>
        /// <param name="cbProgress">进度回调</param>
        public async Task LoadScene(string sceneName, bool isAdditive = false, Action<float> cbProgress = null)
        {
            string sceen = sceneName.Substring(sceneName.LastIndexOf("/") + 1);
            string assetBundleName = "scene/" + sceneName.ToLower() + ".unity";          
            if (!assetBundleName.EndsWith(AppSetting.ExtName))
                assetBundleName += AppSetting.ExtName;
            await _LoadScene(assetBundleName, sceen, isAdditive, cbProgress);
        }

        #region 私有协同方法
        /// <summary>
        /// 异步加载资源
        /// </summary>
        private async Task<T> _LoadAsset<T>(string assetBundleName, string assetName, bool isWait = false) where T : UObject
        {
            // Load asset from assetBundle.
            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(T));
            if (request == null) return default(T);
            //await request;
            while (!request.IsDone())
            {
                await new WaitForEndOfFrame();
            }
            T obj = request.GetAsset<T>();
            if (obj == null)
                Debug.LogError($"加载资源失败:{assetBundleName}  AssName:{assetName}");
            return obj;
        }

        private WaitForEndOfFrame waitFrame =  new WaitForEndOfFrame();
        /// <summary>
        /// 异步加载场景
        /// </summary>
        private async Task _LoadScene(string sceneAssetBundle, string levelName, bool isAdditive, Action<float> cbProgress)
        {
            //float startTime = Time.realtimeSinceStartup;
            AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(sceneAssetBundle, levelName, isAdditive);
            if (request != null && cbProgress!=null) 
            {
                while (request.Progress() < 1f)
                {
                    await waitFrame;
                    cbProgress(request.Progress());
                    if (request.IsDone())
                        break;
                }
                cbProgress(1f);
            }
            //float elapsedTime = Time.realtimeSinceStartup - startTime;
            Utils.ResetShader(null);
            //Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
        }
    #endregion
    //真实环境返回空
    public static string GetRelativePath()
    {
        if (!AppSetting.IsRelease)   
            return "file://" + AppSetting.ExportResBaseDir + Utility.GetPlatformName() + "/";       
        return string.Empty;
    }

}
