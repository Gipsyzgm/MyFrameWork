using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ILoading : MonoBehaviour {

    public static ILoading instance;
    void Awake()
    {
        instance = this;
    }

    //刚进入游戏初始化资源
    public void AssetInit()
    {
        string zipFile = IFile.bundleFile;
        string zipTo = Application.persistentDataPath;
        float progress = 0;
        if(File.Exists(zipFile))
        {
            IZip.UnZip(zipFile, zipTo, ref progress, unZipAction);
        }
        else
        {
            unZipAction(true);
        }
    }
    void unZipAction(bool success)
    {
        if(success)
        {
            if (File.Exists(IFile.bundleFile))
                File.Delete(IFile.bundleFile);

        }
    }

}
