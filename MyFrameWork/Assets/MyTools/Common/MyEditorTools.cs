using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class MyEditorTools
{
    /// <summary>
    /// 给物体添加一个路径下所有同尾缀的Component
    /// </summary>
    /// <param name="obj">物体</param>
    /// <param name="path">路径</param>
    /// <param name="end">尾缀比如.cs</param>
    public static void AddFileComment(GameObject obj, string path,string end)
    {
        DirectoryInfo direction = new DirectoryInfo(path);  
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].Name.EndsWith(end)) continue;
            string tempName = files[i].Name.Split('.')[0];
            Type t = Type.GetType(tempName);
            obj.AddComponent(t); 
        }
    }
    /// <summary>
    /// 在编辑器里代码添加tag
    /// </summary>
    /// <param name="tag"></param>
    public static void AddTag(string tag)
    {
#if UNITY_EDITOR
        UnityEditorInternal.InternalEditorUtility.AddTag(tag);
#endif
    }

}

