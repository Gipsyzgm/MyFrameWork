using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingIphoneXCenter : MonoBehaviour {

    public Color color = Color.white;

	void Start () {
        SettingIphoneX.SetValueCenter(transform);
	}
    void OnEnable()
    {
        SettingIphoneX.SetColor(color);
    }
}
