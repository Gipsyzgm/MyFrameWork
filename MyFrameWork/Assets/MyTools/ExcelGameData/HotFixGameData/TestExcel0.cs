using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TestExcel0 : BaseDataConfig
{
    public override object UniqueID => Id;
    /// <summary>
    /// 组编号
    /// </summary>
    public int Id;
    /// <summary>
    /// 具体信息。0/空 1/State1 2/State2
    /// </summary>
    public string GroupInfo;
    



    private static Dictionary<int, TestExcel0> dictionary = new Dictionary<int, TestExcel0>();
    /// <summary>
    /// 通过EquipId获取Csv1Config的实例
    /// </summary>
    /// <param name="EquipId">索引</param>
    /// <returns>Csv1Config的实例</returns>
    public static TestExcel0 Get(int EquipId)
    {
        return dictionary[EquipId];
    }
    /// <summary>
    /// 获取字典
    /// </summary>
    /// <returns>字典</returns>
    public static Dictionary<int, TestExcel0> GetDictionary()
    {
        return dictionary;
    }

}

