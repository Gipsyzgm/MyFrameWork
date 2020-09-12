using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestLanguage : BaseDataConfig
{
    public override object UniqueID => Id;
    public string Id;
    public string cn;
    public string en;   
}

