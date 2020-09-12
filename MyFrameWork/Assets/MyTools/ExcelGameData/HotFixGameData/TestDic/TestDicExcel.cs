using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestDicExcel : BaseDataConfig
{
    public override object UniqueID => Id;
    public string Id;
    public string level;
    public List<string> testDic; 
}
