using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if Do_Tween
using DG.Tweening;

public class ViewAnimator : MonoBehaviour {

    public Transform[] ani;
    public float[] intervalArray = new float[0];   //这两个要长度一样
    public float interval = 0.05f;
    public float scaleTime = 0.2f;
    float tick = 0;


    void OnEnable()
    {
        tick = 0;
        for(int i=0;i<ani.Length;i++)
        {
            ani[i].localScale = Vector3.zero;
            ani[i].DOScale(1, scaleTime).SetDelay(tick).SetEase(Ease.Linear).SetAutoKill(true);
            //TweenClass.TweenBoard(ani[i],tick);

            if (intervalArray.Length > 0)
                tick = intervalArray[i];
            else
                tick += interval;
        }
    }

    public void Insert(Transform[] _ani,float _interval = 0.05f,float _scaleTime = 0.2f)
    {
        ani = _ani;
        interval = _interval;
        scaleTime = _scaleTime;
    }
}
#endif
