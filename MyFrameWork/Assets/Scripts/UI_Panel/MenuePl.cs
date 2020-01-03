using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuePl : PanelBase
{

    private Button startbt;
    private Button gameBt;
    private Button overBt;
    private Button settingBt;
    // Use this for initialization
    public override void Init(params object[] _args)
    {
        skinPath = "Panel/MenuePl";
        layer = PanelLayer.Tips;
    }

    // Update is called once per frame
    public override void OnBeforeShow()
    {
        Transform skinTf = skin.transform;
        startbt = skinTf.Find("StartBt").GetComponent<Button>();
        gameBt = skinTf.Find("GameBt").GetComponent<Button>();
        overBt = skinTf.Find("OverBt").GetComponent<Button>();
        settingBt = skinTf.Find("SettingBt").GetComponent<Button>();

        startbt.onClick.AddListener(OnStartBtClick);
		gameBt.onClick.AddListener(OnGameBtClick);
		overBt.onClick.AddListener(OnOverBtClick);
        settingBt.onClick.AddListener(OnSettingBtClick);
    }
    private void OnStartBtClick()
    {
        MyLog.LogWithColor("点击Start",Color.red);
        PanelMgr.instance.OpenPanel<StartPl>();
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }
    private void OnGameBtClick()
    {
        PanelMgr.instance.OpenPanel<GamePl>();
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }
    private void OnOverBtClick()
    {
        PanelMgr.instance.OpenPanel<OverPl>();
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.SettingPl);
    }
    private void OnSettingBtClick()
    {


        PanelMgr.instance.ClosePanel(PanelName.StartPl);
        //PanelMgr.instance.OpenPanel<SettingPl>();
        //PanelMgr.instance.HidePanel(PanelName.GamePl);
        //PanelMgr.instance.HidePanel(PanelName.StartPl);
        //PanelMgr.instance.HidePanel(PanelName.OverPl);
    }
}
