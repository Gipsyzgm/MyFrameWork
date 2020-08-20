using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
/// <summary>
/// APP配置
/// </summary>
public class AppSetting
{
    public static bool IsVersionCheck = true;                        //是否启用版本检测 正式包改为true
    public static bool IsRelease = true;                             //走正式流程设为true(发布会强制修改)
    public static bool IsLocalResServer = false;                      //是否为本地资源测试
    public static bool LoaddelayTest = false;                       //延时加载UI测试(只有编辑器下有效)
    public static bool ILRNotABTest = false;                        //不使用AB资源加载ILR(只有编辑器下有效)  
    public static bool EditorVerCheckt = false;                     //编辑器下是否启用版本检测(只有编辑器下有效)  

    public const string ProjectName = "MyFrame";
    public const string ExtName = ".unity3d";                      //(.unity3d)素材扩展名
    public const string UIAtlasDir = "MyUIAtlas/";
    public const string HotFixName = "HotFix_HorseRacing";  //热更工程名
    public const bool HotFixUnbound = true;                        //(true)是否可使用未绑定的方法,禁用后没有CLR的方法会抛异常
    public const string ConfigBundleDir = "Data/Config/";       //配置文件目录(相对于BundleResDir)
    public const string HoxFixBundleDir = "Data/HotFix/";      //配置文件目录(相对于BundleResDir)
    public const string ABFiles = "ABFiles.txt";                    //AB资源文件信息  资源路径|MD5值|大小
    public const string VersionFile = "Version.txt";                //版本信息文件      

    public static bool IsForcedUpdate = false;      //是否强制更新
    public static bool IsCheckVer = false;             //是否为审核版本 
  
    public static string TestHttpServer = "http://localhost/hotfix/";
 

    //是否进入服务器调试模式
    public static bool IsDebugServer => PlayerPrefs.GetInt("IsDebugServer", 0) == 1;


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
        get { return Application.persistentDataPath + "/" + Utility.GetPlatformName() + "/"; }
    }


    /// <summary>
    /// streamingAssetsPath
    /// </summary>
    public static string StreamingAssetsPath
    {
        get {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_WIN        
                return Application.streamingAssetsPath + "/" + Utility.GetPlatformName() + "/";
#else
                return "file:///" + Application.streamingAssetsPath + "/" + Utility.GetPlatformName()+ "/";
#endif
        }
    }
    //ILR逻辑代码目录,只用于编辑环境
    public static string ILRCodeDir
    {
        get { return Path.GetFullPath("../Product/ILR/").Replace("\\", "/"); }
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

    public const string BundleArtResDir = "Assets/GameRes/ArtRes/";

    public static string[] BundleArtResFolders = new string[] { "Textures", "Prefabs", "Materials" };

#region 游戏内相关路径

    /// <summary>
    /// 编辑器环境下所有需要挂在的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public  static readonly string EditorEnvPath = "Assets/MyTools/Common/EditorEnv";
    /// <summary>
    /// 正式环境下所有需要挂在的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public  static readonly string FormalEnvPath = "Assets/MyTools/Common/FormalEnv";
    /// <summary>
    /// 公共文件所有需要挂在的不区分环境的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public  static readonly string PublicEnvPath = "Assets/MyTools/Common/PublicEnv";

    /// <summary>
    /// EXCEL表格生成资源的路径
    /// </summary>
    public static string ExcelAssetDir = "Assets/GameRes/Resources/";
    /// <summary>
    /// 音乐存放路径
    /// </summary>
    public static string MusicPath = "Assets/GameRes/Resources/MySource/";

    //UI预制体存储位置
    public static string UiPrefabDir = "Assets/GameRes/Resources/MyUI/View/";
    //Item预制体存储位置
    public static string ItemPrefabDir = "Assets/GameRes/Resources/MyUI/Item/";
    /// <summary>
    /// 需要打AssetBundle的资源路径
    /// </summary>
    public static string BundleResPath = Application.dataPath + "/GameRes/BundleRes/";
    /// <summary>
    /// 生成打AssetBundle的资源路径
    /// </summary>
    public static string BundleOutPut = Application.dataPath;

#endregion
}
