using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MenuePl : PanelBase {

    public Button StartBt;
    public Button GameBt;
    public Button OverBt;
    public Button SettingBt;
    public Button ClosePl;
    public Button TestParameter;
    public override void Init(params object[] _args)
    {
         args = _args;
         CurViewPath="MyUI/View/MenuePl";
         layer = PanelLayer.Normal;
    }
    public override void InitComponent()
    {
        StartBt = curView.transform.Find("StartBt_Button").GetComponent<Button>();
        StartBt.onClick.AddListener(StartBtOnClick);
        GameBt = curView.transform.Find("GameBt_Button").GetComponent<Button>();
        GameBt.onClick.AddListener(GameBtOnClick);
        OverBt = curView.transform.Find("OverBt_Button").GetComponent<Button>();
        OverBt.onClick.AddListener(OverBtOnClick);
        SettingBt = curView.transform.Find("SettingBt_Button").GetComponent<Button>();
        SettingBt.onClick.AddListener(SettingBtOnClick);
        ClosePl = curView.transform.Find("ClosePl_Button").GetComponent<Button>();
        ClosePl.onClick.AddListener(ClosePlOnClick);
        TestParameter = curView.transform.Find("TestParameter_Button").GetComponent<Button>();
        TestParameter.onClick.AddListener(TestParameterOnClick);
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————
                                                                                                
    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void TestParameterOnClick()
    {
        Debug.Log("测试传递参数按钮");
        object[] args = new object[6];
        args[0] = "Title_New";
        args[1] = "描述_New";
        args[2] = "确认";
        args[3] = "取消";
        args[4] = new UnityAction(TestAction1);
        args[5] = new UnityAction(() =>
        {

            Debug.LogError("点击取消");
        });
        PanelMgr.Instance.OpenPanel<CommonConfirmPl>("", args);


    }
    public void TestAction1()
    {
        Debug.LogError("点击确认");

    }    

    public void CustomComponent()
    {
       
           
    }

    public override void OnShow()
    {
        Debug.LogError("打开游戏首页");
        base.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public void StartBtOnClick()
    {



        Debug.LogError("点击Start");
        PanelMgr.Instance.OpenPanel<StartPl>();
        
       
        MsgSend.SendLogin("token", "1.0.1");
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }

    public void GameBtOnClick()
    {
       
        PanelMgr.Instance.OpenPanel<GamePl>();
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }

    public void OverBtOnClick()
    {
        
       
        PanelMgr.Instance.OpenPanel<OverPl>();
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }

    public void SettingBtOnClick()
    {
       
        PanelMgr.Instance.OpenPanel<SettingPl>();
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
    }


    public void ClosePlOnClick()
    {
        PanelMgr.Instance.CloseCurrentPanel();
    }



}
