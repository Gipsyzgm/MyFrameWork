using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestDicExcel : BaseDataConfig
{
    public override object UniqueID => Id;
    public int Id;
    public string level;
    public int[] testDic2;
    public string[] testDic3;
    public float testDic4;
    public bool testDic5;


    private static Dictionary<int, TestDicExcel> dictionary = new Dictionary<int, TestDicExcel>();
    /// <summary>
    /// 通过EquipId获取Csv1Config的实例
    /// </summary>
    /// <param name="EquipId">索引</param>
    /// <returns>Csv1Config的实例</returns>
    public static TestDicExcel Get(int EquipId)
    {
        return dictionary[EquipId];
    }
    /// <summary>
    /// 获取字典
    /// </summary>
    /// <returns>字典</returns>
    public static Dictionary<int, TestDicExcel> GetDictionary()
    {
        return dictionary;
    }

}
