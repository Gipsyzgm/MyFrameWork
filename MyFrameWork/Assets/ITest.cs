using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ITest: MonoBehaviour
{
  


    async void Start() 
    {
        UnityWebRequest  request = UnityWebRequest.Get("http://localhost/hotfix/AssetBundles/Windows/framework/assets/gameres/bundleres/mysource/欢快渐进bg.unity3d");
        await request.SendWebRequest();
        if (request.error!=null)
        {
            Debug.LogError("下载空的数据：" + request.downloadHandler.data.Length);
            return;
        }
        Debug.LogError("下载的什么数据："+request.downloadHandler.data.Length);
        Byte[] test = new Byte[0];
        Debug.LogError(MD5Utils.MD5ByteFile(request.downloadHandler.data));
        Debug.LogError(MD5Utils.MD5ByteFile(request.downloadHandler.data).Length);
        Debug.LogError(MD5Utils.MD5ByteFile(test));
        Debug.LogError(MD5Utils.MD5ByteFile(test).Length);
    }
 

    private void Update()
    {
        
    }
    // Start is called before the first frame update

}
