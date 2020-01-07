using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTools : MonoBehaviour {

    /// <summary>
    /// 判断手机语言
    /// </summary>
    /// <returns></returns>
    public static string GetUserLanguage()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.ChineseSimplified:
                return "CN";
            case SystemLanguage.ChineseTraditional:
                return "TW";
            case SystemLanguage.English:
                return "EN";
            case SystemLanguage.Japanese:
                return "JP";
            case SystemLanguage.Korean:
                return "KO";
            case SystemLanguage.German:
                return "DE";
            case SystemLanguage.French:
                return "FR";
            case SystemLanguage.Russian:
                return "RU";
            case SystemLanguage.Swedish:
                return "SV";
            case SystemLanguage.Portuguese:
                return "PT";
            case SystemLanguage.Spanish:
                return "ES";
            case SystemLanguage.Thai:
                return "TH";
            case SystemLanguage.Unknown:
                return "EN";
            default:
                return "EN";
        };
    }

    /// <summary>
    /// 是否为同一天，每次打开判定一次
    /// </summary>
    /// <returns></returns>
    public static bool IsToday()
    {
        System.DateTime currenTime = new System.DateTime();
        currenTime = System.DateTime.Now;
        int Month = currenTime.Month;
        int Day = currenTime.Day;
        if (!PlayerPrefs.HasKey("DayTime"))
        {
            PlayerPrefs.SetInt("DayTime", Day);
            PlayerPrefs.SetInt("MonthTime", Month);
        }
        if (Day == PlayerPrefs.GetInt("DayTime") && Month == PlayerPrefs.GetInt("MonthTime"))
        {
            //Debug.LogError("Daytime" + Day+"MonthTime"+Month);
            return true;
        }
        else
        {
            Debug.LogError("Daytime111" + Day + "MonthTime" + Month);
            PlayerPrefs.SetInt("DayTime", Day);
            PlayerPrefs.SetInt("MonthTime", Month);
            return false;
        }

    }


}
