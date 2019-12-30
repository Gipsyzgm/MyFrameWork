using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IString : MonoBehaviour {

    /// <summary>
    /// string转json格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string StringToJson(Dictionary<string, object> dic)
    {
        //@"\"
        string json = "{";
        foreach (var d in dic)
        {
            json += '\"' + d.Key + '\"' + ":";
            if (d.Value.GetType() == typeof(string))
            {
                json += '\"' + d.Value.ToString() + '\"';
            }
            else
            {
                json += d.Value.ToString();
            }
            json += ',';
        }
        json = json.TrimEnd(',');
        json += "}";
        return json;
    }
    public static string StringToJson(params object[] str)
    {
        //@"\"
        string json = "{";
        for (int i = 0; i < str.Length; i++)
        {
            if (i % 2 == 0)
            {
                json += '\"' + str[i].ToString() + '\"' + ":";
            }
            else
            {
                if (str[i].GetType() == typeof(string))
                {
                    json += '\"' + str[i].ToString() + '\"';
                }
                else
                {
                    json += str[i].ToString();
                }
                json += ',';
            }
        }
        json = json.TrimEnd(',');
        json += "}";
        return json;
    }
}
