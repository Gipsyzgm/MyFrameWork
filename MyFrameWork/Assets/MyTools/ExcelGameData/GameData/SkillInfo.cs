using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SkillInfo : BaseInfo<SkillInfo>
{
    public int Id;

    // 名字
    public string Name;

    // 描述信息
    public string Des;

    //图标地址
    public string SkillImg;
}