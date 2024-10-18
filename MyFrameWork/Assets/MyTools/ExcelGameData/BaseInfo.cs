using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class BaseInfo<T> where T : BaseInfo<T>
{
    protected static Dictionary<int, T> dictionary = new Dictionary<int, T>();
    protected static List<T> KeyList = new List<T>();

    /// <summary>
    /// 通过EquipId获取Csv1Config的实例
    /// </summary>
    /// <param name="InfoID">索引</param>
    /// <returns>Csv1Config的实例</returns>
    public static T Get(int InfoID)
    {
        if (dictionary.ContainsKey(InfoID))
        {
            return dictionary[InfoID];
        }

        return default(T);
    }

    /// <summary>
    /// 获取字典
    /// </summary>
    /// <returns>字典</returns>
    public static Dictionary<int, T> GetDictionary()
    {
        return dictionary;
    }

    public static List<T> GetList()
    {
        return KeyList;
    }
}