using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLog{

    public static void Log(string info)
    {
        Debug.Log(info);
    }
    public static void LogError(string info)
    {
        Debug.LogError(info);
    }
    public static void LogWarning(string info)
    {
        Debug.LogWarning(info);
    }
    public static void LogWithTag(string info, LogTag logTag)
    {
        string temp = logTag.ToString();
        Debug.Log(temp + ":" + info);
    }
    public static void LogWithColor(string info,Color color)
    {
        string MyColor = ColorUtility.ToHtmlStringRGB(color);
        Debug.Log("-><color=#"+MyColor+">" + info + "</color>");     
    } 
}
public enum LogTag
{
    UnityTag_Info,
    UnityTag_Sdk,
    UnityTag_Ads,
    UnityTag_Editor,
}
