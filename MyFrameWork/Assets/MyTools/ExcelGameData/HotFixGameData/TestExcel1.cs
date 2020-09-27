using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestExcel1:BaseDataConfig
{
    public override object UniqueID => Id;

    public int Id;
    /// 根据此参数确定当前阶段
    /// </summary>
    public string Stage;
    /// <summary>
    /// item数量
    /// </summary>
    public string ItemNum; 
    /// <summary>
    /// 看的距离
    /// </summary>
    public string LookArea;
    /// <summary>
    /// 最小坐标
    /// </summary>
    public string Min;
    /// <summary>
    /// 最大坐标
    /// </summary>
    public string Max;

    private static Dictionary<int, TestExcel1> dictionary = new Dictionary<int, TestExcel1>();
    /// <summary>
    /// 通过EquipId获取Csv1Config的实例
    /// </summary>
    /// <param name="EquipId">索引</param>
    /// <returns>Csv1Config的实例</returns>
    public static TestExcel1 Get(int EquipId)
    {
        return dictionary[EquipId];
    }
    /// <summary>
    /// 获取字典
    /// </summary>
    /// <returns>字典</returns>
    public static Dictionary<int, TestExcel1> GetDictionary()
    {
        return dictionary;
    }
}

