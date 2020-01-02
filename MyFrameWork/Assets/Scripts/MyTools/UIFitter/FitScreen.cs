using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class FitScreen : MonoBehaviour {
    [SerializeField]
    private float Size_with = 9.0f;
    [SerializeField]
    private float Size_High = 16f;

    void Awake()
    {
        Fit_Screen();
    }
    #region 简单的适配屏幕
    /// <summary>
    /// 根据屏幕宽度来匹配屏幕
    /// </summary>
    public void Fit_Screen()
    {
        float specificValue = Screen.width * 1.0f / Screen.height;//屏幕宽高比
        CanvasScaler[] canvas = GameObject.FindObjectsOfType<CanvasScaler>();

        if (specificValue < Size_with / Size_High)
        {
            foreach (CanvasScaler can in canvas)
            {
                can.matchWidthOrHeight = 0f;
            }
        }
        else
        {
            foreach (CanvasScaler can in canvas)
            {
                can.matchWidthOrHeight = 1f;
            }
        }
    }
    #endregion
}