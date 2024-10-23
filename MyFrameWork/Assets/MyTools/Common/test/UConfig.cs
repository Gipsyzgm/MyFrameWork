//======================================
//	Author: Matrix
//  Create Time：3/13/2017 3:27:01 PM 
//  Function:
//======================================

using System;
using System.Collections.Generic;

public class UConfig
{
    public static char[] splitChar = new char[4] { ';', '|', '#', '$' };

    public enum CommonLayer
    {
        UI = 5,
        Building = 8,
        Troop = 9,
        SelfTroop = 10,
        DynamicObj = 14,
        TempObj = 19,
        Monster = 20,
    }

    //被点击到的优先级顺序
    public static Dictionary<int, int> layerSortDict = new Dictionary<int, int>
    {
        { 19, 1 },
        { 10, 2 },
        { 9, 3 },
        { 8, 4 },
        { 20, 5 },
        { 14, 6 }
    };

    //被点击到的优先级顺序
    public static Dictionary<int, int> innerCityLayerSortDict = new Dictionary<int, int>
    {
        { 19, 1 },
        { 8, 2 },
        { 10, 3 },
        { 9, 4 },
        { 20, 5 },
        { 14, 6 }
    };

    public const string PREFKEY_AUDIO_BG = "Audio_Bg";
    public const string PREFKEY_AUDIO_EFFECT = "Audio_Effect";
    public const string PREFKEY_SAVE_POWER = "Save_Power";

    public const string LAYER_BUILDING = "Building";
}