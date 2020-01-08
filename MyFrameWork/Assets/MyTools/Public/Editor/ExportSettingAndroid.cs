using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using System.Xml;

public class ExportSettingAndroid : MonoBehaviour
{
    //把要替换的文件都放到Setting/Android里面，要换什么就往下面加
    static string androidSettingPath = Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/Setting/Android/";
    static string projectPath;
    static bool openCopy = true;

    [PostProcessBuild(999)]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string path)
    {
        if (BuildTarget != BuildTarget.Android) return;
        if (openCopy == false) return;
        projectPath = path + "/" + Application.productName + "/";

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
    }

    //拷贝单个文件(路径)
    public static void Copy(string from,string to)
    {
        from = androidSettingPath + from;
        to = projectPath + to;
        if (File.Exists(from))
        {
            Debug.Log("from:" + from);
            File.Copy(from, to, true);
        }
    }
    //拷贝文件夹
    public static void CopyDir(string fromDir, string toDir)
    {
        fromDir = androidSettingPath + fromDir;
        toDir = projectPath + toDir;
        if (Directory.Exists(fromDir))
        {
            if(Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }
            FileUtil.CopyFileOrDirectory(fromDir, toDir);
        }
    }
    //拷贝文件夹里的文件
    public static void CopyDirFile(string fromDir, string toDir)
    {
        fromDir = androidSettingPath + fromDir;
        toDir = projectPath + toDir;
        if(Directory.Exists(fromDir))
        {
            if(!Directory.Exists(toDir))
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
    //拷贝Java，把文件夹里的Java都拷贝过去
    static void CopyJaveDirFile(string fromDir = "Java")
    {
        fromDir = androidSettingPath + fromDir;
        if (!Directory.Exists(fromDir))
            return;
        string toDir = "src/main/java/" + Application.identifier + "/";
        if (!Directory.Exists(toDir))
        {
            toDir = "src/main/java/";
            string[] flood = Application.identifier.Split('.');
            for (int j = 0; j < flood.Length; j++)
                toDir += flood[j] + "/";
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
}
