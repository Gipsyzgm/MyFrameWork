using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuePl : PanelBase {

    public Button StartBt;
    public Button GameBt;
    public Button OverBt;
    public Button SettingBt;
    public Button ClosePl;
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
        CustomComponent();
    }
    //====================上面部分自动生成，每次生成都会替换掉,不要手写东西==================

    //====================以下为手写部分，初始化补充方法为CustomComponent()==================
    //@EndMark@
 
        
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
