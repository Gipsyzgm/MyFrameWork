using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ITest: MonoBehaviour
{
  


    async void Start() 
    {
        UnityWebRequest  request = UnityWebRequest.Get("C:/Users/ZGM/Desktop/test/MyFrame_Data/StreamingAssets/Windows/Windows");
        UnityWebRequest download = request;
        await download.SendWebRequest();
        if (download.error!=null)
        {
            Debug.LogError("下载空的数据：" + download.error);
            return;
        }
        if (download.isDone)
        {
            Debug.LogError("下载的什么数据：" + download.downloadHandler.data.Length);
            Debug.LogError("下载的什么数据：" + download.url);

        }
       
       
    }
 

    private void Update()
    {
        
    }
    // Start is called before the first frame update

}
