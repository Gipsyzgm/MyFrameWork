using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ITest: MonoBehaviour
{
  


    async void Start() 
    {
        string url = "C:/Users/ZGM/Desktop/test/MyFrame_Data/StreamingAssets/Windows/Windows";
        UnityWebRequest  request = UnityWebRequest.Get(url);
     
        await request.SendWebRequest();
        if (request.error!=null)
        {
            Debug.LogError("下载空的数据：" + request.error);
            return;
        }
        if (request.isDone)
        {
            Debug.LogError("下载的什么数据：" + request.downloadHandler.data.Length);
            DownloadHandlerAssetBundle downloadHandler = request.downloadHandler as DownloadHandlerAssetBundle;
            AssetBundle bundle = downloadHandler.assetBundle;
            //AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
           

            Debug.LogError("下载的什么数据：" + request.downloadHandler.data.Length);
            Debug.LogError("下载的什么数据：" + request.url);

        }
        WWW download = new WWW(url);
        await  download;
        if (download.error != null)
        {
            Debug.LogError("WWW下载空的数据：" + request.error);
            return;
        }

        // If downloading succeeds.
        if (download.isDone)
        {
            Debug.LogError("WWW下载空的数据：" + download.bytes.Length);
           
            AssetBundle bundle = download.assetBundle;

            if (bundle == null)
            {

                Debug.Log("www bundle is null ");
                return;
            }
            else 
            {
                Debug.Log("www bundle 不为4 null ");

            }

            Debug.Log("Downloading is done at frame " + Time.frameCount);
            Debug.LogError("www下载的什么数据：" + download.url);
            Debug.LogError("www下载的什么数据：" + bundle.name);
            Debug.LogError("www下载的什么数据：" + bundle.isStreamedSceneAssetBundle); 
            Debug.LogError("www下载的什么数据：" + bundle.Contains("Windows"));
            Debug.LogError("www下载的什么数据："  );

            foreach (var item in bundle.GetAllScenePaths())
            {
                Debug.LogError(item);
            }

        }

    }
 

    private void Update()
    {
        
    }
    // Start is called before the first frame update

}
