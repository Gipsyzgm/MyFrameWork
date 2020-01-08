using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIEffect))]
public class UIEffectTween : MonoBehaviour {

    UIEffect ui;
    Vector2 uv = Vector2.zero;
    bool trip;
    float dis;
    public float outline = 5;
    public float speed = 5;

	void Start () {
        dis = outline;
        ui = GetComponent<UIEffect>();
    }
	

	void Update () {

        if(trip)
        {
            dis += Time.deltaTime * speed;
            if(dis > outline)
            {
                dis = outline;
                trip = false;
            }
        }
        else
        {
            dis -= Time.deltaTime * speed;
            if(dis < 0)
            {
                dis = 0;
                trip = true;
            }
        }

        ui.effectDistance = Vector2.one * dis;

    }




    [ContextMenu("Reset")]
    public void EffectInit()
    {
        ui = GetComponent<UIEffect>();
        ui.colorMode = UIEffect.ColorMode.Set;
        ui.color = new Color(1, 1, 1, 0);
        ui.useGraphicAlpha = true;
        ui.shadowMode = UIEffect.ShadowMode.Outline8;
        ui.blurMode = UIEffect.BlurMode.Fast;
        ui.blur = 0;
        ui.shadowBlur = 1;
    }
}
