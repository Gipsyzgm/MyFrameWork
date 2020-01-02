using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IFile : MonoBehaviour {

    //bundle文件夹
#if UNITY_ANDROID
    public static string bundleDir = "Android";
#elif UNITY_IOS
    public static string bundleDir = "IOS";
#else
    public static string bundleDir = "Windows";
#endif
    //初始bundle路径
    public static string bundleFile = IFile.GetStreammingAssetsFile() + "/" + bundleDir + ".zip";

    //经过测试的StreammingAssets路径
    public static string GetStreammingAssetsFile()
    {
        string path="";
#if UNITY_STANDALONE_WIN
        path= "file:///" + Application.streamingAssetsPath;
#elif UNITY_ANDROID
        path= Application.streamingAssetsPath;
#elif UNITY_IOS
        path= "file://" + Application.streamingAssetsPath;  //IOS前面加file://
#endif
        return path ;
    }


    //读取streamingAssetsPath文件
    public static byte[] LoadBytes(string file)
    {
        //#if UNITY_ANDROID
        WWW www = new WWW(file);
        while (!www.isDone)
        {
        }
        return www.bytes;
        //安卓这种是不能读的,安卓streamingAssetsPath里面文件夹自动转换成小写
        //return File.ReadAllBytes(file + "house.bin")
        //这种也不能读,只支持WWW,WWW效率不高耗内存;
        //return HOUSE_ARRAY.Parser.ParseFrom(File.OpenRead(file + "house.bin"));
        //#else
        //        file += name;
        //        FileStream fs = new FileStream(file, FileMode.Open);
        //        byte[] bytes = new byte[(int)fs.Length];
        //        fs.Read(bytes, 0, (int)fs.Length);
        //        fs.Close();
        //        return bytes;
        //#endif
    }
}
