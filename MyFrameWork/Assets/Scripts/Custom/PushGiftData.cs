using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PushGiftData : BaseInfo<PushGiftData>
{
    public int Id;
    // 标题
    public int SoldierId;
    // 内容
    public string Name;
    // 日期
    public int Price;
    // 版本号
    public int Count;
    // 排序(小的在前面)
    public int Key;
}