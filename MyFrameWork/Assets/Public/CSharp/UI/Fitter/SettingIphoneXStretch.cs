using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingIphoneXStretch : MonoBehaviour {

    public Color color = Color.white;

    void Start()
    {
        SettingIphoneX.SetValueStretch(transform);
    }
    void OnEnable()
    {
        SettingIphoneX.SetColor(color);
    }
}
