using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/*
 * 把音效放在Assets/Resources/MySource,点击我的工具/导入声音 即可通过
 */

public class ExportAudio : MonoBehaviour {
    //代码目录
    static string scriptDir = Application.dataPath + "/MyTools/MyAudio/";
    //音效目录
    static string clipPath = "Assets/Resources/MySource/";

    [MenuItem("我的工具/导入声音")]
    public static void Export()
    {
        if (!Directory.Exists(scriptDir))
            Directory.CreateDirectory(scriptDir);
        if (!Directory.Exists(clipPath))
            Directory.CreateDirectory(clipPath);
        StringBuilder sbPath = new StringBuilder();
        string csName = "MyAudioPath";
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");
        sbPath.AppendLine("public class " + csName);
        sbPath.AppendLine("{");
        string resPath = "";
        string[] str = clipPath.Split('/');
        for (int i = 2; i < str.Length; i++)
            resPath += str[i] + "/";
        resPath = resPath.Substring(0, resPath.Length - 2);       
        DirectoryInfo direction = new DirectoryInfo(clipPath);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        sbPath.AppendLine("    public static string Path" + " = " + '"' + resPath + "/" +'"' + ";");        
        sbPath.AppendLine("}");
        string scriptFilePath = scriptDir + csName + ".cs";
        string tempName = "MyAudioName";
        sbPath.AppendLine("public enum " + tempName);
        sbPath.AppendLine("{");
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string prefabName = files[i].Name.Split('.')[0];
            sbPath.AppendLine("    " + prefabName  + ",");
        }     
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        Debug.LogError("音乐导入成功");
    }
}
