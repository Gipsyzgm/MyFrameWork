using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextPanel1
{

    public int Id;
    public string cn;
    public string en;

    private static Dictionary<int, TextPanel1> dictionary = new Dictionary<int, TextPanel1>();
    /// <summary>
    /// 通过EquipId获取Csv1Config的实例
    /// </summary>
    /// <param name="EquipId">索引</param>
    /// <returns>Csv1Config的实例</returns>
    public static TextPanel1 Get(int EquipId)
    {
        return dictionary[EquipId];
    }
    /// <summary>
    /// 获取字典
    /// </summary>
    /// <returns>字典</returns>
    public static Dictionary<int, TextPanel1> GetDictionary()
    {
        return dictionary;
    }

}