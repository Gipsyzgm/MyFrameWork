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


    //设置ab资源路径
    static string resPath = AppSetting.BundleResDir;
    //ab资源输出路径
    static string outPut = GetExportPath();

    /// <summary>
    /// 使用本地AB资源（需要给ab资源命名）/模拟加载ab资源
    /// </summary>
    [MenuItem("我的工具/AssetBundle/本地资源(F)模拟ab资源(T)")]
    public static void SimulateAssetBundle()
    {
        ABMgr.SimulateAssetBundleInEditor = !ABMgr.SimulateAssetBundleInEditor;  
    }
    [MenuItem("我的工具/AssetBundle/本地资源(F)模拟ab资源(T)", true)]
    public static bool SimulateAssetBundleValidate()
    {
        Menu.SetChecked("我的工具/AssetBundle/本地资源(F)模拟ab资源(T)", ABMgr.SimulateAssetBundleInEditor);
        return true;
    }

    [MenuItem("我的工具/AssetBundle/打开AB资源文件夹")]
    public static void ShowInExplorer()
    {
        MyEditorTools.ShowExplorer(GetExportPath());
    }
    /// <summary>
    /// 把热更资源移除/放进StreamingAssets文件夹
    /// </summary>
    [MenuItem("我的工具/AssetBundle/Link StreamingAssets")]
    public static void MkLinkStreamingAssets()
    {
        LinkHelper.IsLinkStreamingAssets = !LinkHelper.IsLinkStreamingAssets;
        LinkHelper.MkLinkStreamingAssets();
    }
    [MenuItem("我的工具/AssetBundle/Link StreamingAssets", true)]
    public static bool MkLinkStreamingAssetsValidate()
    {
        Menu.SetChecked("我的工具/AssetBundle/Link StreamingAssets", LinkHelper.IsLinkStreamingAssets);
        return true;
    }
    /// <summary>
    /// 删除ab包资源
    /// </summary>
    /// <returns></returns>
    [MenuItem("我的工具/AssetBundle/删除Ab资源文件")]
    public static void ReBuildAllAssetBundles()
    {
        if (MyEditorTools.IsPlaying()) return;
        string outputPath = outPut;
        Directory.Delete(outputPath, true);
        Debug.LogError("删除目录: " + outputPath);
    }
    [MenuItem("我的工具/AssetBundle/打Ab包")]
    public static void BuidldBundles()
    {
        if (MyEditorTools.IsPlaying()) return;
        if (!Directory.Exists(resPath))
            Debug.LogError("资源路径不存在："+resPath);
        if (!Directory.Exists(outPut))
            Directory.CreateDirectory(outPut);
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);
        //把热更文件拷贝进来
        //CopyHotFix();

        //清除Ab包资源名
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < abNames.Length; i++)
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        Debug.LogError("清除全部资源AssetBundle名称完成!");
        SetAssetsBundleName(resPath);
        Debug.LogError("设置AssetBundle名称完成!");
        EditSpritAtlas.SetUIAtlas();
        BuildPipeline.BuildAssetBundles(outPut, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        CreateAssetBundleFileInfo();
        AssetDatabase.Refresh();
        Debug.LogError("AssetBundle打包完成!");

    }
    /// <summary>
    /// 生成AB资源文件列表
    /// </summary>
    public static void CreateAssetBundleFileInfo()
    {
        string abRootPath = GetExportPath();
        string abFilesPath = abRootPath + "/" + AppSetting.ABFiles;
        if (File.Exists(abFilesPath))
            File.Delete(abFilesPath);

        var abFileList = new List<string>(Directory.GetFiles(abRootPath, "*"+AppSetting.ExtName, SearchOption.AllDirectories));
        abFileList.Add(abRootPath + Utility.GetPlatformName());
        FileStream fs = new FileStream(abFilesPath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);

        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2018, 1, 1));
        int ver = ((int)((DateTime.Now - startTime).TotalMinutes));
        sw.WriteLine(ver + "|" + DateTime.Now.ToString("u"));
        for (int i = 0; i < abFileList.Count; i++)
        {
            string file = abFileList[i];
            long size = 0;
            string md5 = MD5Utils.MD5File(file, out size);
            string value = file.Replace(abRootPath, string.Empty).Replace("\\", "/");
            sw.WriteLine(value + "|" + md5 + "|" + size);
        }
        sw.Close();
        fs.Close();
        Debug.LogError("资源版本Version:" + ver + "  已复制到剪切板");
        Debug.LogError("ABFiles文件生成完成");
        MyEditorTools.CopyString(ver.ToString());
    }
    /// <summary>
    /// Unity 5新AssetBundle系统，需要为打包的AssetBundle配置名称
    /// 配置名称文件夹下所有资源的Ab名 命名成资源加载的路径
    /// </summary>
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
                AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
                //string name = assetPath.Substring(resPath.Length).Replace("\\", "/");
                string name = assetPath.Substring(resPath.Length).Replace(@"\", "/");
                //移除后缀方便加载资源不用区分后缀
                if (files[i].Name.Contains("."))
                    name = name.Remove(name.LastIndexOf("."));
                //添加后缀方便把更新资源信息写进ABfiles
                assetImporter.assetBundleName = name+AppSetting.ExtName;              
            }
        }
    }

    /// <summary>
    /// 复制热更文件
    /// </summary>
    private static void CopyHotFix()
    {
        string fileDll = AppSetting.ILRCodeDir + AppSetting.HotFixName + ".dll";
        string filePdb = AppSetting.ILRCodeDir + AppSetting.HotFixName + ".pdb";
        FileInfo file = new FileInfo(fileDll);
        if (file.Exists)
        {
            string targetPaht = Path.Combine(AppSetting.BundleResDir, AppSetting.HoxFixBundleDir, AppSetting.HotFixName);
            file.CopyTo(targetPaht + ".bytes", true);
            new FileInfo(filePdb).CopyTo(targetPaht + "_pdb.bytes", true);
        }
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 获取导出资源路径目录
    /// </summary>
    /// <returns></returns>
    public static string GetExportPath()
    {
        BuildTarget platfrom = EditorUserBuildSettings.activeBuildTarget;
        string basePath = AppSetting.ExportResBaseDir;
        if (File.Exists(basePath))
        {
            MyEditorTools.ShowDialog("路径配置错误: " + basePath);
            throw new System.Exception("路径配置错误");
        }
        string path = null;
        var platformName = Utility.GetPlatformName();
        path = basePath + platformName + "/";
        MyEditorTools.CreateDir(path);
        return path;
    }

}
