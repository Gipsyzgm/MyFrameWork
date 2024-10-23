using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GiftEffectConfig : BaseInfo<GiftEffectConfig>
{
    public int Id;

    // 礼物id
    public int giftId;

    // 礼物名称
    public string name;

    // 阵营（1=上方 2=下方）
    public int camp;

    // 模型
    public string model;

    // 召唤模型数量/增加基础弹药数量
    public int num;

    // 方块血量/增加栅栏血量
    public int life;

    // 方块防御
    public int defense;

    // 效果持续时间
    public int time;

    // 其他参数
    public string para;

    // 礼物台词音效
    public int voice;

    // 礼物道具图片
    public string picName;

    // 礼物对应英雄图片
    public string heroName;

    // 英雄名称
    public string heroNameId;
}