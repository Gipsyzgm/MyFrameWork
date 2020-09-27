using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextPanel2 : BaseDataConfig
{
    public override object UniqueID => Id;
    public int Id;
    public string cn;
    public string en;
    private static Dictionary<int, TextPanel2> dictionary = new Dictionary<int, TextPanel2>();
    /// <summary>
    /// 通过EquipId获取Csv1Config的实例
    /// </summary>
    /// <param name="EquipId">索引</param>
    /// <returns>Csv1Config的实例</returns>
    public static TextPanel2 Get(int EquipId)
    {
        return dictionary[EquipId];
    }
    /// <summary>
    /// 获取字典
    /// </summary>
    /// <returns>字典</returns>
    public static Dictionary<int, TextPanel2> GetDictionary()
    {
        return dictionary;
    }
}