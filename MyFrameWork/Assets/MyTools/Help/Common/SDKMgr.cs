/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.1.16
 *  描述信息：封装的Unity调用安卓原生方法的方法
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SDKMgr:MonoBehaviour{

  

    /// <summary>
    /// Unity调用安卓接口，参数不限制
    /// </summary>
    /// <param name="funcName"></param>
    /// <param name="parms"></param>

    private static AndroidJavaClass m_AndroidJavaClass = null;

    private static void CallJavaFunction(string funcName, params object[] parms)
    {
#if UNITY_ANDROID
        if (m_AndroidJavaClass == null)
        {
            m_AndroidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        }
        AndroidJavaObject jo = m_AndroidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        if (jo != null)
        {
            jo.Call(funcName, parms);
        }
#endif
    }
    /// <summary>
    /// Unity调用安卓方法 带返回值，只能返回一个值。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="funcName"></param>
    /// <param name="parms"></param>
    /// <returns></returns>
    public static T CallJavaFunction<T>(string funcName, params object[] parms)
    {
#if UNITY_ANDROID
        if (m_AndroidJavaClass == null)
        {
            m_AndroidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        }
        AndroidJavaObject jo = m_AndroidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        if (jo != null)
        {
            T returnVar = jo.Call<T>(funcName, parms);
            return returnVar;
        }
#endif
        return default(T);
    }


    /// <summary>
    /// 测试接口
    /// </summary>
    public static void ClickOtherTest()
    {
        CallJavaFunction("ClickOtherTest", " OtherTest方法调用");
    }

    /// <summary>
    /// 测试接口(带返回值)
    /// </summary>
    /// <param name="Slotid"></param>
    public static bool IsRewardLoaded(string Slotid)
    {
        return CallJavaFunction<bool>("ClickRewardLoaded", Slotid);
    }
}
