using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 挂在UI根节点上,UI根节点挂在UI摄像机下
 * 相当于用高拉伸或者压缩
 */
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
