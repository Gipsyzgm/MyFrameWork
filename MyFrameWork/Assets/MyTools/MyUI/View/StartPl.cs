﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StartPl : PanelBase {

    public Button StartBt;
    public override void Init(params object[] _args)
    {
         CurViewPath= "Prefabs/MyUI/View/StartPl";
         layer = PanelLayer.Start;
    }
    public override void InitComponent()
    {
        StartBt = curView.transform.Find("StartBt_Button").GetComponent<Button>();
        StartBt.onClick.AddListener(StartBtOnClick);
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————
                                                                                                
    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public override void OnShow()
    {
        base.OnShow();
    }
        
    public void StartBtOnClick()
    {
        Debug.Log("开始游戏");
        ABMgr.Instance.LoadScene("Test_Env");
    }
       
    public void CustomComponent()
    {
        
    }


    public override void Update()
    {

    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
