using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class PanelLogin : PanelBase {

    public Button btnTimeSetting;
    public Button btnLogin;
    public InputField goInput;
    public Text txtVersion;
    public Button btnSetting;
    public Button btnNotice;
    public override void Init(params object[] _args)
    {
         args = _args;
         CurViewPath="MyUI/View/PanelLogin";
         layer = PanelLayer.Normal;
    }
    public override void InitComponent()
    {
        btnTimeSetting = curView.transform.Find("btnTimeSetting_Button").GetComponent<Button>();
        btnTimeSetting.onClick.AddListener(btnTimeSettingOnClick);
        btnLogin = curView.transform.Find("btnLogin_Button").GetComponent<Button>();
        btnLogin.onClick.AddListener(btnLoginOnClick);
        goInput = curView.transform.Find("goInput_InputField").GetComponent<InputField>();
        txtVersion = curView.transform.Find("txtVersion_Text").GetComponent<Text>();
        btnSetting = curView.transform.Find("btnSetting_Button").GetComponent<Button>();
        btnSetting.onClick.AddListener(btnSettingOnClick);
        btnNotice = curView.transform.Find("btnNotice_Button").GetComponent<Button>();
        btnNotice.onClick.AddListener(btnNoticeOnClick);
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————
                                                                                                
    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void btnTimeSettingOnClick()
    {
        
        
        
    }
        
    public void btnLoginOnClick()
    {
        
    }
        
    public void btnSettingOnClick()
    {
        PanelMgr.Instance.OpenPanel<PanelSetting>();
    }
        
    public void btnNoticeOnClick()
    {
        PanelMgr.Instance.OpenPanel<PanelNotice>();
    }
        
    public void btncloseOnClick()
    {
        
    }
        
    public void CustomComponent()
    {
        
    }
        
    public override void OnShow()
    {
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
}
