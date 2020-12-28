using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipInfo
{
    public int Id;
    // 名字
    public string Name;
    // 描述信息
    public string Des;
    //图标地址
    public string HeadImg;
    //品质
    public int level;
    //技能
    public int attack;
    //价格
    public int Price;


    private static Dictionary<int, EquipInfo> dictionary = new Dictionary<int, EquipInfo>();
    private static List<int> KeyList = new List<int>();
    /// <summary>
    /// 通过EquipId获取Csv1Config的实例
    /// </summary>
    /// <param name="EquipId">索引</param>
    /// <returns>Csv1Config的实例</returns>
    public static EquipInfo Get(int EquipId)
    {
        return dictionary[EquipId];
    }
    /// <summary>
    /// 获取字典
    /// </summary>
    /// <returns>字典</returns>
    public static Dictionary<int, EquipInfo> GetDictionary()
    {
        return dictionary;
    }

    public static List<int> GetAllKey()
    {
        return KeyList;
    }
}