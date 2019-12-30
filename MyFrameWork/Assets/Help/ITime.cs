using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ITime : MonoBehaviour {


    /// <summary>
    /// 程序退到后台-----记下时间
    /// </summary>
    public static void OfflineTime()
    {
        string offlineTime = DateTime.Now.ToString();
        PlayerPrefs.SetString("IOfflineTime", offlineTime);
    }
    /// <summary>
    /// 打开游戏-----离线时长
    /// </summary>
    /// <returns></returns>
    public static double OnlineTime()
    {
        DateTime onlineTime = DateTime.Now;
        string offlineTimeString = PlayerPrefs.GetString("IOfflineTime", string.Empty);
        if (offlineTimeString == string.Empty) return 0;

        return DateTime.Now.Subtract(Convert.ToDateTime(offlineTimeString)).TotalSeconds;
    }
    /// <summary>
    /// 距离明天多少秒
    /// </summary>
    /// <returns></returns>
    public static double TimeFromTomorrow()
    {
        return 24 * 3600 - DateTime.Now.TimeOfDay.TotalSeconds;
    }
    /// <summary>
    /// 秒转换成string
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    public static string TimeSecondToString(int sec)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}", sec / 3600, sec % 3600 / 60, sec % 60);
    }
    /// <summary>
    /// 两个时间差
    /// </summary>
    /// <param name="contrast0"></param>
    /// <param name="contrast1"></param>
    /// <returns></returns>
    public static double TimeContrastSecond(DateTime contrast0, DateTime contrast1)
    {
        TimeSpan timeSpan = contrast0.Subtract(contrast1);
        return timeSpan.TotalSeconds;
    }
    public static double TimeContrastDay(DateTime contrast0, DateTime contrast1)
    {
        TimeSpan timeSpan = contrast0.Subtract(contrast1);
        return timeSpan.TotalDays;
    }
    /// <summary>
    /// 隔天登陆-------Awake...do something
    /// </summary>
    /// <returns></returns>
    public static bool LoginSpacing()
    {
        DateTime now = DateTime.Now;
        string lastString = PlayerPrefs.GetString("IGameLastTimeLogin", string.Empty);
        if (lastString.Equals(string.Empty))
        {
            //第一次登陆记录
            PlayerPrefs.SetString("IGameLastTimeLogin", now.ToString());
            return true;
        }
        else
        {
            //有记录就比对
            DateTime last = Convert.ToDateTime(lastString);
            double dayInterval = TimeContrastDay(DateTime.Now, last);
            //天日期不一样||另一个月的同一天
            if (now.Day != last.Day || dayInterval >= 1)
            {
                //记录
                PlayerPrefs.SetString("IGameLastTimeLogin", now.ToString());
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 时间戳,精确到秒/毫秒
    /// </summary>
    /// <returns></returns>
    public static long TimeMilliseconds()
    {
        //TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        //long second = Convert.ToInt64(span.TotalSeconds);
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (long)(DateTime.Now - startTime).TotalMilliseconds;
    }
}
