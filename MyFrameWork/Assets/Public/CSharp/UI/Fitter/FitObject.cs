using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 这个可以适配一些Sprite场景或者单个UI等
 */ 
public class FitObject: MonoBehaviour {

    void Awake()
    {
        //UI主节点的适配
        CanvasScaler scaler = FindObjectOfType<CanvasScaler>();
        Vector2 size = scaler.referenceResolution;
        //Vector2 size = new Vector2(1080, 1920);
        //标准宽高比
        float standard = size.x / size.y;
        //实际
        float real = Screen.width / (Screen.height + 0.0f);

        if(real > standard)
        {
            //宽更多适配宽
            transform.localScale = new Vector3(real / standard, 1, 1);
        }
        else if (real < standard)
        {
            //高更多适配高
            transform.localScale = new Vector3(1, standard / real, 1);
        }
    }

}
