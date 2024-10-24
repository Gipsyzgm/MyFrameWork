using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class PanelLogin : PanelBase
{
    public Button btnTimeSetting;
    public Button btnLogin;
    public InputField goInput;
    public Text txtVersion;
    public Button btnSetting;
    public Button btnNotice;

    public override void Init(params object[] _args)
    {
        args = _args;
        CurViewPath = "MyUI/View/PanelLogin";
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
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        PanelMgr.Instance.OpenPanel<PanelMode>();
        
    }
    
    private bool  isWaitConfirm = false;
    public void btnLoginOnClick()
    {
        
        Debug.Log("点击启动");
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        if (isWaitConfirm)
        {
            OnUserLoginRsp();
        }
        else
        {

            if (GameRootManager.Instance.isFirstLogin)
            {
                GameRootManager.Instance.isFirstLogin = false;
                //PanelNetLoading.Start(3);
                MsgSend.SendLogin( GameRootManager.Instance.token,"1.0.1" );
            }
            else
            {
                //PanelNetLoading.Start(3);
                OnUserLoginRsp();
                MsgSend.SendGameStart(1);
            }
            GameRootManager.Instance.RefreshTime3();
        }
    }

    public void OnUserLoginRsp()
    {
        
        Debug.Log("进入Login环节");
        
        // --  if (dataInfoMgr.gameOver == 0 or LuaCommonData.IsLocalMode) and SceneManager:GetInstance():GetGameRootMgr():HasLocalCache() then
        //     --     UIPromptHelper.ShowConfirm2("发现上局比赛还未结束，是否继续上一局的比赛?", function()
        //     --         self:LoginFinish()
        //     --     end, function()
        //     --         SceneManager:GetInstance():GetGameRootMgr():ClearLocal();
        // --         self:LoginFinish()
        //     --     end, function()
        //     --         self.isWaitConfirm = true;
        // --     end, {
        //     --         confirmText = "继续上局",
        //     --         cancelText = "新开比赛",
        //     --     })
        // --  else
        // --     self:LoginFinish()
        //     --  end
        
        //PanelNetLoading.Stop();
        //SceneManager:GetInstance():GetGameRootMgr():ClearLocal();
        LoginFinish();

    }

    public void LoginFinish()
    {
        isWaitConfirm = false;
       
        //进入战斗场景
        PanelMgr.Instance.OpenPanel<PanelLoading>();
        PanelMgr.Instance.GetPanel<PanelLoading>().UpdateProgress( 0.9f, 6f,false);
        GameRootManager.Instance.InitScene();
        // GameManager:GetInstance():Init();
        // GameMapManager:GetInstance():Init();
        PanelMgr.Instance.HidePanel(PanelName.PanelLogin);
        PanelMgr.Instance.GetPanel<PanelLoading>().UpdateProgress( 1f, 0.3f,true);
    }


    public void btnSettingOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        PanelMgr.Instance.OpenPanel<PanelSetting>();
    }

    public void btnNoticeOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        PanelMgr.Instance.OpenPanel<PanelNotice>();
    }


    public void CustomComponent()
    {
        EventMgr.Instance.AddEventListener(EventConst.UserLogin, OnUserLoginRsp);
        EventMgr.Instance.AddEventListener(EventConst.PanelEnterDestroy, OnPanelEnterDestroy);
        
    }
    public void OnPanelEnterDestroy()
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