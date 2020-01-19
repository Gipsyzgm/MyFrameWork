using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏数据类，暂定
/// </summary>
public class MyGameData : MonoBehaviour {

    /// <summary>
    /// 读表数据
    /// </summary>
    public static AllConfigInfo config;
    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    public static void InitGameData()
    {    
        config = Resources.Load<AllConfigInfo>("ConfigAsset");

       
    }

}
