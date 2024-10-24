﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SettingPl : PanelBase {

    public Button PlayMusicBt;
    public Button PlayWinBt;
    public Button PlayFailBt;
    public Button StopMusicBt;
    public Button StopEffect;
    public Slider SoundValueBt;
    public Toggle MusicCtrlBt;
    public Button ChangeLanguage;
    public override void Init(params object[] _args)
    {
         CurViewPath= "MyUI/View/SettingPl";
         layer = PanelLayer.Info;
    }
    public override void InitComponent()
    {
        PlayMusicBt = curView.transform.Find("PlayMusicBt_Button").GetComponent<Button>();
        PlayMusicBt.onClick.AddListener(PlayMusicBtOnClick);
        PlayWinBt = curView.transform.Find("PlayWinBt_Button").GetComponent<Button>();
        PlayWinBt.onClick.AddListener(PlayWinBtOnClick);
        PlayFailBt = curView.transform.Find("PlayFailBt_Button").GetComponent<Button>();
        PlayFailBt.onClick.AddListener(PlayFailBtOnClick);
        StopMusicBt = curView.transform.Find("StopMusicBt_Button").GetComponent<Button>();
        StopMusicBt.onClick.AddListener(StopMusicBtOnClick);
        StopEffect = curView.transform.Find("StopEffect_Button").GetComponent<Button>();
        StopEffect.onClick.AddListener(StopEffectOnClick);
        SoundValueBt = curView.transform.Find("SoundValueBt_Slider").GetComponent<Slider>();
        MusicCtrlBt = curView.transform.Find("MusicCtrlBt_Toggle").GetComponent<Toggle>();
        ChangeLanguage = curView.transform.Find("ChangeLanguage_Button").GetComponent<Button>();
        ChangeLanguage.onClick.AddListener(ChangeLanguageOnClick);
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————
                                                                                                
    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void ChangeLanguageOnClick()
    {
        Debug.Log("切换语言");
        if (LanguageMgr.type == LanguageMgr.LanguageType.cn)
        {
            LanguageMgr.ChangeLanguage(LanguageMgr.LanguageType.en);
        }
        else
        {
            LanguageMgr.ChangeLanguage(LanguageMgr.LanguageType.cn);
        }
       
    }
        
    int count = 0;


    public void PlayMusicBtOnClick()
    {
        count++;
        if (count % 2 == 0)
        {
            AudioMgr.Instance.PlayMusic(MyAudioName.BackMusic);
        }
        else
        {
            AudioMgr.Instance.PlayMusic(MyAudioName.QuickBg);
        }
    }
        
    public void PlayWinBtOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.Win);
    }
        
    public void PlayFailBtOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.Fail);
    }
        
    public void StopMusicBtOnClick()
    {
        count++;
    }
        
    public void StopEffectOnClick()
    {
        count++;

    }
        
    public void CustomComponent()
    {
        SoundValueBt.onValueChanged.AddListener((float Value) =>
        {
            AudioMgr.Instance.musicSource.volume = Value;
            AudioMgr.Instance.effectSource.volume = Value;

        });
        MusicCtrlBt.onValueChanged.AddListener((bool isOn) =>
        {
         
        });
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
