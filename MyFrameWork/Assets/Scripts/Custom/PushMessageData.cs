using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PushMessageData : BaseInfo<PushMessageData>
{
    public string Key;
    // 内容
    public string Value;
    // 日期
    public string Name;
    // 类型
    public PushUseType UseType;
}
public enum PushUseType
{
    None = 0,
    GenerateSoldier = 1,
    ChangeRoad = 2,
    Crazy = 3,
    SuperCrazy = 4,
}