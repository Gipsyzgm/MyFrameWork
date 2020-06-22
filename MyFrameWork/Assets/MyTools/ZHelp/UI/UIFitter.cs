/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.1.16
 *  描述信息：通过获取真实的缩放值来达到UI适配的目的，适用于与目标宽高比相差不大或者允许UI元素存在一定屏幕适配。
 *  使用方法：
 *      1：挂在UI根节点上,即Canvas,选择ScreenSpace-Camera模式并指定相机。
 *      2：选择ScaleWithScreenSize并指定目标大小
 *      3：所有子节点锚点必须是中心点 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFitter : MonoBehaviour {

    public static float realStandard;
    void Awake () {
        Camera uiCamera = GetComponent<Canvas>().worldCamera;
        if(uiCamera == null)
        {
            Debug.Log("先设置UI摄像机");
            return;
        }
        transform.SetParent(uiCamera.transform);
        CanvasScaler scaler = GetComponent<CanvasScaler>();
        scaler.matchWidthOrHeight = 0;
        Vector2 size = scaler.referenceResolution;
        float standard = size.y / size.x;
        float real = Screen.height / (Screen.width + 0.0f);
        realStandard = real / standard;
        uiCamera.transform.localScale = new Vector3(1, realStandard, 1);

    }
}
