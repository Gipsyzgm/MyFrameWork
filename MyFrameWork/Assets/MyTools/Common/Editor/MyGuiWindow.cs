using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using System.IO;

//继承自EditorWindow类
public class MyGuiWindow : EditorWindow
{
    string description = "没什么有效信息。就是试试Unity自定义的窗口怎么搞。";

    public int LogFontSize = 25;
    /// <summary>
    /// 按钮高度
    /// </summary>
    public int BtnHigh = 25;
    /// <summary>
    /// 通用间隔
    /// </summary>
    public int Space = 3;

    //利用构造函数来设置窗口名称
    MyGuiWindow()
    {
        this.titleContent = new GUIContent("使用说明");
    }
    //添加菜单栏用于打开窗口
    [MenuItem("我的工具/使用说明", false, 0)]
    static void showWindow()
    {
        MyGuiWindow myGuiWindow = EditorWindow.GetWindow<MyGuiWindow>();

        myGuiWindow.minSize = new Vector2(600, 500);
    }
    void OnGUI()
    {
        ////绘制当前时间
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Space(10);
        GUILayout.Label("MyFrameWork使用说明");
        //添加名为按钮，用于调用AutoUi()函数
        GUILayout.Space(Space);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("框架使用说明", GUILayout.Height(BtnHigh)))
        {
            FrameWorkInfo();
        }

        if (GUILayout.Button("关于我的工具", GUILayout.Height(BtnHigh)))
        {
            AboutMyTools();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(Space);
        if (GUILayout.Button("自动生成UI", GUILayout.Height(BtnHigh)))
        {
            AutoUi();
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("声音控制系统", GUILayout.Height(BtnHigh)))
        {
            AutoAudio();
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("对象池", GUILayout.Height(BtnHigh)))
        {
            AutoPool();
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("事件监听系统",GUILayout.Height(BtnHigh)))
        {
            AutoListener();
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("多语言快速构建", GUILayout.Height(BtnHigh)))
        {
            AutoLanguage();
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("Excel读写数据及使用", GUILayout.Height(BtnHigh)))
        {
            AutoExcel();
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("快捷打包支持", GUILayout.Height(BtnHigh)))
        {
            AutoPackPro();
        }
      

        GUILayout.Space(30);
        EditorGUI.BeginDisabledGroup(true);  //如果nextPath == null 为真，在Inspector面板上显示，承灰色（即不可操作）  
        GUILayout.BeginHorizontal();
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        description = EditorGUILayout.TextArea(description, GUI.skin.label, GUILayout.MaxHeight(400));
        GUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
        GUILayout.EndVertical();
    }

    //用于保存当前信息
    void FrameWorkInfo()
    {
        description =
            "使用说明1：Common文件夹为通用资源。谨慎操作，其他脚本可能引用。\n" +
            "使用说明2：Zhelp文件内为一些辅助功能脚本。\n" +
            "使用说明3：功能遵循独立原则，通过删除对应功能文件夹以删除其功能。\n" +
            "使用说明4：功能遵循极简原则，通过简单说明快速构建。\n" +
            "使用说明5：功能说明遵循使用说明4。\n" +
            "使用说明6：由划水爱好者Gipsy整理。";
    }
    void AutoUi()
    {
        description = "自己去搜ExportUI脚本自己看,里面有详细介绍。";
    }
    void AutoAudio()
    {
        description = "自己去搜MyAudioMgr脚本自己看,里面有详细介绍。";
    }
    void AutoPool()
    {
        description = "自己去搜MyPoolSingleton脚本自己看,里面有详细介绍。";
    }
    void AutoListener()
    {
        description = "自己去搜MyEventMgr脚本自己看,里面有详细介绍。";

    }
    void AutoLanguage()
    {
        description = "自己去LocalLanguage文件夹自己看,里面有详细介绍。";

    }
    void AutoExcel()
    {
        description = "自己去搜CreateExcel脚本自己看,里面有详细介绍。";

    }
    void AutoPackPro()
    {
        description = "自己去搜ExportedSetting脚本自己看,里面有详细介绍。";
    }
    void AboutMyTools()
    {
        description = "1：游戏，即游戏内容的扩展。需要自行在GameEditor实现。\n" +
            "2：环境，快速添加一些对应环境的通用功能。\n" +
            "3：快捷键，增加开始和停止的对应快捷键。\n" +
            "4：截图，截取当前Game视图放在桌面。";
    }
    void QuickStart()
    {
        description = "其实其他框架都是载入一个预制体就ok了。\n" +
            "但我也觉得还需要花时间去看";
    }
}

