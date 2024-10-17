using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UpdateNotice : BaseInfo<UpdateNotice>
{
    public int Id;
    // 标题
    public string Name;
    // 内容
    public string Content;
    // 日期
    public string Date;
    // 版本号
    public string Version;
    // 排序(小的在前面)
    public int Sort;
}