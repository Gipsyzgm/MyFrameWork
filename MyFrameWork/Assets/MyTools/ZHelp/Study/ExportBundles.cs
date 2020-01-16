using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.IO;
using UnityEditor;

/* 
 * 学习AssetBundles
 * 资源打包:不要取中文名
 * 将Resources下的资源达成Bundles
 *    O BuildAssetBundleOptions.None  --杓建AssetBundle投有任何持殊的选项
 *    1 BuildissetBundleOptions.UnccmpressedAssetBundle  --不进行数据圧縮。如果使用垓項,因カ投有圧縮解圧縮的辻程,AssetBundle的友布和加戟会很快,但是AssetBundle也会更大,下載変慢
 *    2 BuildAssetBundleOptions.ColleetDependencies  --包合所有依頼美系
 *    4 BuildAssetBundleOptions.CompleteAssets  --強制包括整个資源
 *    8 BuildAssetBundleOptions.DisableriteTypeTree  blog--在AssetBundle中不包合業型信息 岌布web平台吋,不能使用垓項
 *   16 BuildassetBundleoptions.DeterministickssetBundle -- 使毎个object具有唯-不変的hash ID, 可用干増量式安布AssetBundle
 *   32 BuildssetBundleptions.ForceRebuildAssetBundle-強 制重新Bui1d所有的AssetBundle
 *   64 BuildissetBundleoptions.IgnoreTypeTreeChanges  -- 忽略TypeTree的変化, 不能与DisablerypeTree同吋使用
 *  128 BuildAssetBundleOptions.AppendlashToAssetBundleName --附hash到AssetBundle名称中
 *  256 BuildAssetBundleOptions.ChunkBasedCompression - Assetbundle的圧鏥格式カ1z4.默圦的是lzaa格式, 下戟assetbundle后 立即解圧。
 *                                                        而l格式的Assetbundle会在加載資源的吋候オ迸行解圧, 只是解圧資源的吋机不一祥
 */

public class ExportBundles : MonoBehaviour {


#if UNITY_ANDROID
    static string bundleDir = "Android";
#elif UNITY_IOS
    static string bundleDir = "IOS";
#else
    static string bundleDir = "Windows";
#endif
    static string resPath = Application.dataPath + "/Resources/";
    static string outPut = Application.dataPath + "/" + bundleDir + "/";

    //[MenuItem("EasyCode/ExportBundles")]
    public static void BuidldBundles()
    {
        if (!Directory.Exists(resPath))
            Directory.CreateDirectory(resPath);
        if (!Directory.Exists(outPut))
            Directory.CreateDirectory(outPut);
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);
       
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < abNames.Length; i++)
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);        
        SetAssetsBundleName(resPath);
        BuildPipeline.BuildAssetBundles(outPut, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        //可以把Lua代码文件夹挪进去一并打包
        AssetDatabase.Refresh();
    }
    static void SetAssetsBundleName(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] is DirectoryInfo)
                SetAssetsBundleName(files[i].FullName);
            else
            {
                if (files[i].FullName.EndsWith(".meta")) continue;

                string assetPath = "Assets" + files[i].FullName.Substring(Application.dataPath.Length);
                AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);//AssetImporter

                string name = files[i].FullName.Substring(resPath.Length).Replace(@"\", "/");
                if (files[i].Name.Contains("."))
                    name = name.Remove(name.LastIndexOf("."));

                assetImporter.assetBundleName = name;
            }
        }
    }

    

}
