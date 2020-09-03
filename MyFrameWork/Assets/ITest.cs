using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ITest: MonoBehaviour
{
  


    async void Start() 
    {
        string url = "C:/Users/ZGM/AppData/LocalLow/Jsf/MyFrame/Windows/Windows";
        string url2 = "C:/Users/ZGM/Desktop/test/MyFrame_Data/StreamingAssets/Windows/Windows";
        string url3 = "F:/anyelse/MyFrameWork/MyFrameWork/Assets/StreamingAssets/Windows/Windows";
        //AssetBundle manifestAB = AssetBundle.LoadFromFile(url);
        //AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //foreach (var item in manifest.GetAllAssetBundles())
        //{
        //    Debug.LogError(item);
        //}
        //WWW download = new WWW(url2);
        //await download;
        //AssetBundle bundle = download.assetBundle;

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url3);
        await request.SendWebRequest();
        AssetBundle bundle2 = DownloadHandlerAssetBundle.GetContent(request);
        Debug.LogError(request.url);
        AssetBundleManifest AssetBundleManifest2 = bundle2.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        Debug.LogError(AssetBundleManifest2.GetAllAssetBundles().Length);
    }
 

    private void Update()
    {
        
    }
    // Start is called before the first frame update

}
