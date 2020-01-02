using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPl :PanelBase {
    
    private Button PlayMusicBt;
    private Button PlayWinBt;
    private Button PlayFailBt;
    private Button StopMusicBt;
    private Slider SoundValueBt;
    private Toggle MusicCtrlBt;
    private Button StopEffect;
    int count = 0;
    public override void Init(params object[] _args)
    {
        skinPath = "Panel/SettingPl";
        layer = PanelLayer.Start;
   
    }

    public override void OnShowed()
    {
        PlayMusicBt = skin.transform.Find("PlayMusicBt").GetComponent<Button>();
        PlayWinBt = skin.transform.Find("PlayWinBt").GetComponent<Button>();
        PlayFailBt = skin.transform.Find("PlayFailBt").GetComponent<Button>();
        StopMusicBt = skin.transform.Find("StopMusicBt").GetComponent<Button>();
        SoundValueBt = skin.transform.Find("SoundValueBt").GetComponent<Slider>();
        MusicCtrlBt = skin.transform.Find("MusicCtrlBt").GetComponent<Toggle>();
        StopEffect = skin.transform.Find("StopEffect").GetComponent<Button>();
        PlayMusicBt.onClick.AddListener(OnMusicPlay);
        PlayWinBt.onClick.AddListener(OnMusicWinPlay);
        PlayFailBt.onClick.AddListener(OnMusicFailPlay);
        StopMusicBt.onClick.AddListener(OnMusicStop);
        StopEffect.onClick.AddListener(OnEffectStop);

        SoundValueBt.onValueChanged.AddListener((float Value)=> { QAudioSingleton.Instance._musicSource.volume = Value; });
        MusicCtrlBt.onValueChanged.AddListener((bool isOn) =>
        {
            if (isOn)
            {
                QAudioSingleton.Instance.PlayeMusicAudio(AudioName.BackMusic);

            }
            else
            {
                QAudioSingleton.Instance.PlayeMusicAudio(AudioName.BackMusic,0);
            } });
    }

    public void OnMusicPlay()
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
    public void OnMusicWinPlay()
    {
        QAudioSingleton.Instance.PlayerEffectAudio (AudioName.Win);
    }
    public void OnMusicFailPlay()
    {
        QAudioSingleton.Instance.PlayerEffectAudio(AudioName.Fail);
    }
    public void OnMusicStop()
    {
        count++;
        if (count % 2 == 0)
        {
            QAudioSingleton.Instance.IsOpenMusic(false);
        }
        else
        {
            QAudioSingleton.Instance.IsOpenMusic(true);
        }
       
    }
    public void OnEffectStop()
    {
        count++;
        if (count % 2 == 0)
        {
            QAudioSingleton.Instance.IsOpenEffect(false);
        }
        else
        {
            QAudioSingleton.Instance.IsOpenEffect(true);
        }
       
    }
    public void SoundValueChange()
    {
        QAudioSingleton.Instance.PlayeMusicAudio(AudioName.BackMusic, 0);
    }
}
