/*
 *  项目名字：Solitaire
 *  创建时间：2019.12.28
 *  描述信息：显示帧率
 *  
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowFPS : MonoBehaviour 
{
    private float _UpdateInterval = 0.2f;//更新周期 
    private float _Accum;
    private int _Frames;
    private float _Timeleft;
    private string _Fps; //帧率 
    public void Init()
    {
        Reset();
    }
    private void Reset()
    {
        _Timeleft = _UpdateInterval;
        _Accum = 0.0f;
        _Frames = 0;
    }
    public void Update()
    {
        _Timeleft -= Time.deltaTime;
        _Accum += Time.timeScale / Time.deltaTime;
        ++_Frames;

        if (_Timeleft <= 0)
        {
            _Fps = (_Accum / _Frames).ToString("F2");//保留两位小数，四舍五入
            Reset();
        }
    }
    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 200,0, 200, 100));
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;    //这是设置背景填充的
        bb.normal.textColor = new Color(1, 0, 0);   //设置字体颜色的
        bb.fontSize = 40;       //当然，这是字体颜色
        GUILayout.Label("FPS: " + _Fps,bb);
        GUILayout.EndArea();
    }
}
