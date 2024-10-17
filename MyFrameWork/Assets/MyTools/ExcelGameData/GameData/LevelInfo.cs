using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelInfo : BaseInfo<LevelInfo>
{
    public int Id;

    //可上阵玩家数量
    public int HeroCount;

    //敌方怪物信息及位置
    public string[] EnemyInfo;

    //关卡类型
    public int LevelType;

    //关卡后续关卡选择
    public int[] LevelSelect;
}