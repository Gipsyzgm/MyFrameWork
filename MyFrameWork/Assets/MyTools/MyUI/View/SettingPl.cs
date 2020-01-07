using UnityEngine;
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
    public override void Init(params object[] _args)
    {
         CurViewPath="MyUI/View/SettingPl";
         layer = PanelLayer.Panel;
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
        CustomComponent();
    }
    //====================上面部分自动生成，每次生成都会替换掉,不要手写东西==================

    //====================以下为手写部分，初始化补充方法为CustomComponent()==================
    //@EndMark@
    int count = 0;


    public void PlayMusicBtOnClick()
    {
        count++;
        if (count % 2 == 0)
        {
            QAudioSingleton.Instance.PlayeMusicAudio(AudioName.BackMusic);
        }
        else
        {
            QAudioSingleton.Instance.PlayeMusicAudio(AudioName.BackMusic2);
        }
    }
        
    public void PlayWinBtOnClick()
    {
        QAudioSingleton.Instance.PlayerEffectAudio(AudioName.Win);
    }
        
    public void PlayFailBtOnClick()
    {
        QAudioSingleton.Instance.PlayerEffectAudio(AudioName.Fail);
    }
        
    public void StopMusicBtOnClick()
    {
        count++;
        if (count % 2 == 0)
        {
            QAudioSingleton.Instance.OpenMusic();
        }
        else
        {
            QAudioSingleton.Instance.CloseMusic();
        }
    }
        
    public void StopEffectOnClick()
    {
        count++;
        if (count % 2 == 0)
        {
            QAudioSingleton.Instance.OpenEffect();
        }
        else
        {
            QAudioSingleton.Instance.CloseEffect();
        }

    }
        
    public void CustomComponent()
    {
        SoundValueBt.onValueChanged.AddListener((float Value) =>
        {
            QAudioSingleton.Instance._musicSource.volume = Value;
            QAudioSingleton.Instance._effectSource.volume = Value;

        });
        MusicCtrlBt.onValueChanged.AddListener((bool isOn) =>
        {
            if (isOn)
            {
                QAudioSingleton.Instance.OpenMusic();

            }
            else
            {
                QAudioSingleton.Instance.CloseMusic();
            }
        });
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
}
