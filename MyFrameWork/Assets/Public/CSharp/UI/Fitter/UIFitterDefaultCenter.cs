using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * If it's just cooperation, you're not the core, use this
 * 父节点撑开,四周的按钮用锚点,中间的UI节点选中间挂这个脚本
 */
public class UIFitterDefaultCenter : MonoBehaviour {

    public float width = 1;

	void Awake () {
        CanvasScaler scaler = FindObjectOfType<CanvasScaler>();
        Vector2 size = scaler.referenceResolution;
        float standard = size.y / size.x;
        float real = Screen.height / (Screen.width + 0.0f);
        float realStandard = real / standard;
        transform.localScale = new Vector3(realStandard * width, realStandard * width, 1);
    }
	
}
