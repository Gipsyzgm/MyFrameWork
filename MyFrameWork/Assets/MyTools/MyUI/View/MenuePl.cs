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
         CurViewPath="MyUI/View/MenuePl";
         layer = PanelLayer.Tips;
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
        object[] args = new object[4];
        args[0] = "Title_New";
        args[1] = "描述_New";
        args[2] = new UnityAction(TestAction1);
        args[3] = new UnityAction(() =>
        {

            Debug.LogError("点击取消");
        });
        PanelMgr.instance.OpenPanel<CommonConfirmPl>("", args);


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
        curView.SetActive(true); 
    }
        
    public override void Update()
    {
        
    }
        
    public override void OnHide()
    {
         curView.SetActive(false);    
    }
        
    public override void OnClose()
    {
         Destroy(curView);   
         Destroy(this);   
    }
        
    public void StartBtOnClick()
    {
       

       
        MyLog.LogWithColor("点击Start", Color.red);
        PanelMgr.instance.OpenPanel<StartPl>();
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }

    public void GameBtOnClick()
    {
       
        PanelMgr.instance.OpenPanel<GamePl>();
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }

    public void OverBtOnClick()
    {
        
       
        PanelMgr.instance.OpenPanel<OverPl>();
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }

    public void SettingBtOnClick()
    {
       
        PanelMgr.instance.OpenPanel<SettingPl>();
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
    }


    public void ClosePlOnClick()
    {
        PanelMgr.instance.CloseCurrentPanel();
    }



}
