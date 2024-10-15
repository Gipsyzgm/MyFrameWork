using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// APP配置
/// </summary>
public class FilePathMgr
{
    public const string ProjectName = "MyFrame";

    //用于根据扩展名选择热更资源
    public const string ExtName = ".unity3d"; //(.unity3d)素材扩展名

    /// <summary>
    /// persistentDataPath
    /// </summary>
    public static string PersistentDataURL
    {
        get { return "file:///" + PersistentDataPath; }
    }

    /// <summary>
    /// persistentDataPath
    /// </summary>
    public static string PersistentDataPath
    {
        get
        {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN
            return Application.persistentDataPath + "/" + Utility.GetPlatformName() + "/";
#else
            return "file:///" + Application.persistentDataPath + "/" + Utility.GetPlatformName() + "/";
#endif
        }
    }


    /// <summary>
    /// streamingAssetsPath
    /// </summary>
    public static string StreamingAssetsPath
    {
        get
        {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN
            return Application.streamingAssetsPath + "/" + Utility.GetPlatformName() + "/";
#else
                return "file:///" + Application.streamingAssetsPath + "/" + Utility.GetPlatformName()+ "/";
#endif
        }
    }

    /// <summary>
    /// 导出资源根目录
    /// </summary>
    public static string ExportResBaseDir
    {
        get { return Path.GetFullPath("../Product/AssetBundles/").Replace("\\", "/"); }
    }

    /// <summary>
    /// 需要打包的资源目录
    /// </summary>
    public const string BundleResDir = "Assets/GameRes/BundleRes/";


    #region 游戏内相关路径

    /// <summary>
    /// 编辑器环境下所有需要挂在的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public static readonly string EditorEnvPath = "Assets/MyTools/Common/EditorEnv";

    /// <summary>
    /// 正式环境下所有需要挂在的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public static readonly string FormalEnvPath = "Assets/MyTools/Common/FormalEnv";

    /// <summary>
    /// 公共文件所有需要挂在的不区分环境的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public static readonly string PublicEnvPath = "Assets/MyTools/Common/PublicEnv";

    /// <summary>
    /// EXCEL表格生成资源的路径 "Assets/GameRes/BundleRes/GameData/"
    /// </summary>
    public static string ExcelAssetDir = "Assets/GameRes/BundleRes/GameData/";
    
    /// <summary>
    /// EXCEL数据脚本 "Assets/MyTools/ExcelGameData/GameData/"
    /// </summary>
    public static string ExcelScriptDir = "Assets/MyTools/ExcelGameData/GameData/";
    
    /// <summary>
    /// AllConfigInfo的文件存放地址 "Assets/MyTools/ExcelGameData/AllConfigInfo/"
    /// </summary>
    public static string ExcelAllConfigInfoDir = "Assets/MyTools/ExcelGameData/AllConfigInfo/";
    
    /// <summary>
    /// ReadExcelInfo的文件存放地址,必须editor下 "Assets/MyTools/ExcelGameData/Editor/"
    /// </summary>
    public static string ReadExcelInfoDir = "Assets/MyTools/ExcelGameData/Editor/";
    
    /// <summary>
    /// 音乐存放路径 "Assets/GameRes/BundleRes/MySource/"
    /// </summary>
    public static string MusicClipPath = "Assets/GameRes/BundleRes/MySource/";
    
    /// <summary>
    /// 音乐代码存放路径 "/MyTools/MyAudio/"
    /// </summary>
    public static string MusicScriptPath = Application.dataPath + "/MyTools/MyAudio/";
    
    //UI代码存储位置 "/MyTools/MyUI/View/"
    public static string UIScriptDir = Application.dataPath + "/MyTools/MyUI/View/";

    //Item代码存储位置 "/MyTools/MyUI/Item/"
    public static string UIItemScriptDir = Application.dataPath + "/MyTools/MyUI/Item/";
    
    //UI预制体存储位置 "Assets/GameRes/BundleRes/MyUI/View/"
    public static string UiPrefabDir = "Assets/GameRes/BundleRes/MyUI/View/";

    //Item预制体存储位置 "Assets/GameRes/BundleRes/MyUI/Item/"
    public static string ItemPrefabDir = "Assets/GameRes/BundleRes/MyUI/Item/";
    
    //自定义Asset位置 "Assets/GameRes/BundleRes/GameData/Custom/"
    public static string CustomAssetDir = "Assets/GameRes/BundleRes/GameData/Custom/";
    

    #endregion
}