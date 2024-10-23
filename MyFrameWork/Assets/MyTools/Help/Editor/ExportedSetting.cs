/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.1.9
 *  描述信息：安卓Exprot自动化打包工具，快速测试项目必备,主要针对使用androidstudio打包工程。
 *  
 *  功能1：自动打包，通过在androidSettingPath位置添加配置文件，自动添加和替换一些需要手动修改和替换的文件。（弊：安卓工程每次都需要重新编译）
 *  原理：通过增加和替换配置文件对应的工程文件达到不用每次都手动添加一些文件。
 *  使用方法：1,添加配置文件。2,脚本内配置替换部分添加对应文件的替换方法。CommonCopy设置为true即可。
 *  
 *  功能2：急速打包。只替换资源文件，达到打包完成安卓立刻可以安装测试的效果。
 *  原理：通过把打包的新工程文件内的资源文件和原工程的资源文件替换达到免重新配置和编译的效果。
 *  使用方法：1,Unity项目需要有一个可以正常打包的安卓工程作为原工程。配置参数原工程地质OriginalProjectPath。2，QyickAndroidCopy设置为true即可。
 */
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using System.Xml;

public class ExportedSetting : MonoBehaviour
{
    [MenuItem("我的工具/自动打包支持")]
    public static void AutoBuild()
    {
        Debug.LogError("自动打包需要自己配置文件，双击进入ExportSetting查看，现只支持安卓。");
    }
    /// <summary>
    /// 是否开启自动打包（需配置）
    /// </summary>
    static bool CommonCopy = false;

    /// <summary>
    /// 急速打包（需配置）
    /// </summary>
    static bool QyickAndroidCopy = false;
    /// <summary>
    /// 急速打包true需配置，原始工程路径。
    /// </summary>
    static string OriginalProjectPath = "C:/Users/EDZ/Desktop/AndroidTest/NewTestUnityProject";

    /// <summary>
    /// 需要替换文件的路径(可自行设置)
    /// </summary>
    static string androidSettingPath = Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/Setting/Android/";

    /// <summary>
    /// 是否存在配置文件（用于检测配置文件，不要更改）
    /// </summary>
    static bool isExicted = false;

    /// <summary>
    /// 工程路径（不用设置）
    /// </summary>
    static string projectPath;

    /// <summary>
    /// PostProcessBuild为打包成功的回调，后面的数字代表回调的优先级，越小越先执行。可同时存在多个。
    /// </summary>
    /// <param name="BuildTarget"></param>
    /// <param name="path"></param>
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string path)
    {
        if (BuildTarget != BuildTarget.Android) return;     
        if (CommonCopy == false) return;
        if (QyickAndroidCopy == true) return;
        if (!Directory.Exists(androidSettingPath))
            Directory.CreateDirectory(androidSettingPath);
        Debug.LogError("开始安卓自动打包");
        projectPath = path + "/" + Application.productName + "/";

        //—————————配置替换部分————————

        //Java
        CopyJaveDirFile();
        //libs
        CopyDirFile("libs", "libs");
        //manifest
        Copy("AndroidManifest.xml", "src/main/AndroidManifest.xml");
        //gradle
        Copy("build.gradle", "build.gradle");
        //google,firebase
        Copy("google-services.json", "google-services.json");
        //string.xml
        Copy("strings.xml", "src/main/res/values/strings.xml");
        //Dir
        CopyDir("xml", "src/main/res/xml");

        //—————————配置替换部分————————

        if (isExicted)
        {
            Debug.LogError("自动替换完成");
        }
        else
        {
            Debug.LogError("打包完成，未检测到配置文件");
        }       
    }

    /// <summary>
    /// 拷贝单个文件(路径)
    /// </summary>
    /// <param name="from">文件名，带后缀</param>
    /// <param name="to">目标文件项目内绝对路径</param>

    public static void Copy(string from,string to)
    {
        from = androidSettingPath + from;
        to = projectPath + to;
        if (File.Exists(from))
        {
            isExicted = true;
            Debug.Log("from:" + from);
            File.Copy(from, to, true);
        }
    }
    //拷贝文件夹，用于整个替换文件夹
    public static void CopyDir(string fromDir, string toDir)
    {
        fromDir = androidSettingPath + fromDir;
        toDir = projectPath + toDir;
        if (Directory.Exists(fromDir))
        {
            isExicted = true;
            if (Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }
            FileUtil.CopyFileOrDirectory(fromDir, toDir);
        }
    }
    //拷贝文件夹里的文件，用于在原文件夹基础上增加新文件和替换同名文件
    public static void CopyDirFile(string fromDir, string toDir)
    {
        fromDir = androidSettingPath + fromDir;
        toDir = projectPath + toDir;
        if(Directory.Exists(fromDir))
        {
            isExicted = true;
            if (!Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
            }
            DirectoryInfo direction = new DirectoryInfo(fromDir);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string fromFile = files[i].FullName;//绝对路径
                string toFile = toDir + "/" + files[i].Name;
                File.Copy(fromFile, toFile,true);
            }
        }
    }
    //拷贝Java，把文件夹里的Java都拷贝至UnityPlayerActivity层级
    static void CopyJaveDirFile(string fromDir = "Java")
    {
        fromDir = androidSettingPath + fromDir;
        if (!Directory.Exists(fromDir))
            return;
        isExicted = true;       
        string toDir = "src/main/java/" + Application.identifier + "/";
        if (!Directory.Exists(toDir))
        {
            toDir = toDir.Replace('.', '/');      
        }
        toDir = projectPath + toDir;
        DirectoryInfo direction = new DirectoryInfo(fromDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            string fromFile = files[i].FullName;//绝对路径
            string toFile = toDir + files[i].Name;
            Debug.Log("From:" + fromFile);
            Debug.Log("To:" + toFile);
            File.Copy(fromFile, toFile, true);
        }
    }



    [PostProcessBuild(2)]
    public static void QuickBuild(BuildTarget BuildTarget, string path)
    {

        if (BuildTarget != BuildTarget.Android) return;
        if (QyickAndroidCopy == false) return;
        Debug.LogError("开始急速替换");
        if (!Directory.Exists(OriginalProjectPath))
        {
            Debug.LogError("急速打包失败，原工程文件不存在，请检查文件路径");
        }
        projectPath = path + "/" + Application.productName + "/";
        string tempPath = "src/main/assets/bin";
        string FromPath = path + "/" + Application.productName + "/"+tempPath;
        string ToPath = OriginalProjectPath + "/" + tempPath;
        Debug.LogError("from:"+ FromPath+"to:"+ ToPath);
        if (!Directory.Exists(FromPath))
        {
            Debug.LogError("急速打包失败，获取目标文件失败");
            return;
        }
        if (Directory.Exists(ToPath))
        {
            Directory.Delete(ToPath, true);
        }
        else
        {
            Debug.LogError("急速打包失败，目标路径没有对应文件夹,手动创建文件夹或检查路径是否正确");
            return;
        }
        FileUtil.CopyFileOrDirectory(FromPath, ToPath);       
        Debug.LogError("急速打包成功");
    }
}
