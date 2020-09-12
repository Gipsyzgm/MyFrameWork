using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestExcel1:BaseDataConfig
{
    public override object UniqueID => Id;

    public string Id;
    /// 根据此参数确定当前阶段
    /// </summary>
    public string Stage;
    /// <summary>
    /// item数量
    /// </summary>
    public string ItemNum; 
    /// <summary>
    /// 看的距离
    /// </summary>
    public string LookArea;
    /// <summary>
    /// 最小坐标
    /// </summary>
    public string Min;
    /// <summary>
    /// 最大坐标
    /// </summary>
    public string Max;
}

