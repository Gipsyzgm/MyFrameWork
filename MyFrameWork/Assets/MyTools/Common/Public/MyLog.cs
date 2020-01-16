using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 简单的封装log类，提供带颜色和带标记的log信息
/// </summary>
public class MyLog{

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
