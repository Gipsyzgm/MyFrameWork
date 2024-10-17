using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipInfo : BaseInfo<EquipInfo>
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
}