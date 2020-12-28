using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelInfo
{
    public int Id;
    //可上阵玩家数量
    public int HeroCount;
    //敌方怪物信息及位置
    public string [] EnemyInfo;
    //关卡类型
    public int LevelType;
    //关卡后续关卡选择
    public int[] LevelSelect;

    private static Dictionary<int, LevelInfo> dictionary = new Dictionary<int, LevelInfo>();
    private static List<int> KeyList = new List<int>();
    /// <summary>
    /// 通过EquipId获取Csv1Config的实例
    /// </summary>
    /// <param name="EquipId">索引</param>
    /// <returns>Csv1Config的实例</returns>
    public static LevelInfo Get(int EquipId)
    {
        return dictionary[EquipId];
    }
    /// <summary>
    /// 获取字典
    /// </summary>
    /// <returns>字典</returns>
    public static Dictionary<int, LevelInfo> GetDictionary()
    {
        return dictionary;
    }

    public static List<int> GetAllKey()
    {
        return KeyList;
    }
}