using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmsInfo : BaseInfo<ArmsInfo>
{
    public int Id;

    // 名字
    public string Name;

    // 描述信息
    public string Des;

    //头像地址
    public string HeadImg;

    //职业
    public string Duty;

    //职业图片地址
    public string DutyImg;

    //技能
    public int[] Skill;

    //品质
    public int level;

    //合成对象ID
    public int[] TarGetID;

    //价格
    public int Price;

    //物体位置
    public string PrefabsObj;

    //整个合成树对象ID
    public int[] MixTreeID;

    //基础HP
    public int HP;

    //基础攻击
    public int Attack;

    //防御
    public int Defense;

    //攻速
    public float Speed;

    //闪避
    public float Miss;

    //爆率
    public float Critical;

    //种族
    public string Race;
}