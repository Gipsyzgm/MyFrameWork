using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GradeInnerConfig : BaseInfo<GradeInnerConfig>
{
    public int Id;

    // 局内等级需要赠送礼物累计价值/毛
    public int needMoney;

    // 境界名称
    public string name;

    // 下方头像放大倍数(横向)
    public float xCoeffi;

    // 下方头像放大倍数(高度)
    public float yCoeffi;

    //上方阵营额外产方块血量
    public int blockLift;

    // 下方阵营多管射击  （管道数量;几排管道）
    public string shootPara;

    // 弹药数量字体放大倍数
    public float fontSize;

    // 下方射击角度
    public int spreadAngle;

    // 每发子弹间隔多少秒
    public float frequency;

    // 玩家头顶头像的法宝
    public int playerHead;

  
}