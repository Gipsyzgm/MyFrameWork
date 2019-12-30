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

        PlayMusicBt.onClick.AddListener(OnMusicPlay);
        PlayWinBt.onClick.AddListener(OnMusicWinPlay);
        PlayFailBt.onClick.AddListener(OnMusicFailPlay);
        StopMusicBt.onClick.AddListener(OnMusicStop);
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
        QAudioSingleton.Instance.PlayeMusicAudio(AudioName.BackMusic);
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
        QAudioSingleton.Instance.PlayeMusicAudio(AudioName.BackMusic,0);
    }
    public void SoundValueChange()
    {
        QAudioSingleton.Instance.PlayeMusicAudio(AudioName.BackMusic, 0);
    }
}
