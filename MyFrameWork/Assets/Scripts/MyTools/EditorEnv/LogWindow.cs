/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.12.30
 *  描述信息：Log信息窗口 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Gui生成的一个可以在App上显示log信息的Window
/// </summary>
public class LogWindow : MonoBehaviour
{
    private bool IsDevelopment;
    private Vector2 m_scroll = new Vector2(0, 0);
    List<string> mLogs = new List<string>();
    //滑动条的大小
    public float ScrollbarSize = 50f;
    public int LogFontSize = 25;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string str = logString + "\r\n" + stackTrace;
        string logMsg = string.Format("[{0:u}][{1}]\r\n{2}", System.DateTime.Now, "tag", str);
        mLogs.Add(logMsg);
    }
    void ClearLog()
    {
        mLogs.Clear();
    }

    void OnGUI()
    {

        // Create style for a button
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 30;   
        myButtonStyle.active.textColor = Color.green;
        GUILayout.BeginArea(new Rect(0, 0, 180, 100));
        if (GUILayout.Button("Show Log", myButtonStyle, GUILayout.Width(180f), GUILayout.Height(80f)))
        {
            IsDevelopment = IsDevelopment ? false : true;
        } 
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(200, 0, 180, 100));
        if (GUILayout.Button("Clear Log", myButtonStyle, GUILayout.Width(180f), GUILayout.Height(80f)))
        {
            ClearLog();
        }
        GUILayout.EndArea();
        if (!IsDevelopment)
            return;
       
        GUIStyle horizontalScrollbarThumb = new GUIStyle(GUI.skin.horizontalScrollbarThumb);
        horizontalScrollbarThumb.fixedHeight = ScrollbarSize-2f;
        GUI.skin.horizontalScrollbarThumb = horizontalScrollbarThumb;

        GUIStyle verticalScrollbarThumb = new GUIStyle(GUI.skin.verticalScrollbarThumb);
        verticalScrollbarThumb.fixedWidth = ScrollbarSize-2f;
        GUI.skin.verticalScrollbarThumb = verticalScrollbarThumb;

        GUIStyle MyhorizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
        MyhorizontalScrollbar.fixedHeight = ScrollbarSize;

        GUIStyle MyverticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
        MyverticalScrollbar.fixedWidth = ScrollbarSize;
       
        GUILayout.BeginArea(new Rect(0, 100, Screen.width, Screen.height - 100));
        m_scroll = GUILayout.BeginScrollView(m_scroll,true,true, MyhorizontalScrollbar, MyverticalScrollbar, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height-100));


        GUIStyle style = new GUIStyle();
        style.fontSize = LogFontSize;
        GUIStyleState ss = new GUIStyleState();
        ss.textColor = Color.red;
        style.normal = ss;
        foreach (var v in mLogs)
        {
            GUILayout.Label(v, style);
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
