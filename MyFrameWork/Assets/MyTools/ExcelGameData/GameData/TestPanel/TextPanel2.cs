using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextPanel2 : BaseDataConfig
{
    public override object UniqueID => Id;
    public string Id;
    public string cn;
    public string en;
    public List<string> any;
    public TestDicExcel testDicExcel;
}