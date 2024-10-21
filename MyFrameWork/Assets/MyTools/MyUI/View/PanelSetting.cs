using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class PanelSetting : PanelBase
{
    public Toggle tab1;
    public Toggle tab2;
    public Button btnClose;
    public Toggle tab3close;
    public Toggle tab3open;
    public Slider goSoundSlider;
    public Toggle tab2close;
    public Toggle tab2open;
    public Slider goMusicSlider;
    public Dropdown btnResolution;

    public override void Init(params object[] _args)
    {
        args = _args;
        CurViewPath = "MyUI/View/PanelSetting";
        layer = PanelLayer.Normal;
    }

    public override void InitComponent()
    {
        tab1 = curView.transform.Find("tabswich/tab1_Toggle").GetComponent<Toggle>();
        tab2 = curView.transform.Find("tabswich/tab2_Toggle").GetComponent<Toggle>();
        btnClose = curView.transform.Find("btnClose_Button").GetComponent<Button>();
        btnClose.onClick.AddListener(btnCloseOnClick);
        tab3close = curView.transform.Find("goShortcut/goShortcutView/Sound/goSound/tab3close_Toggle")
            .GetComponent<Toggle>();
        tab3open = curView.transform.Find("goShortcut/goShortcutView/Sound/goSound/tab3open_Toggle")
            .GetComponent<Toggle>();
        goSoundSlider = curView.transform.Find("goShortcut/goShortcutView/Sound/goSoundSlider_Slider")
            .GetComponent<Slider>();
        tab2close = curView.transform.Find("goShortcut/goShortcutView/Music/goMusic/tab2close_Toggle")
            .GetComponent<Toggle>();
        tab2open = curView.transform.Find("goShortcut/goShortcutView/Music/goMusic/tab2open_Toggle")
            .GetComponent<Toggle>();
        goMusicSlider = curView.transform.Find("goShortcut/goShortcutView/Music/goMusicSlider_Slider")
            .GetComponent<Slider>();
        btnResolution = curView.transform.Find("goShortcut/goShortcutView/goResolution/btnResolution_DropDown")
            .GetComponent<Dropdown>();
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————

    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void btnCloseOnClick()
    {
        Close();
    }


    public void CustomComponent()
    {
        tab2open.onValueChanged.AddListener((bool active) =>
        {
            tab2open.isOn = active;
            tab2close.isOn = !active;
            MyAudioMgr.Instance.OpenMusic(active);
        });
        tab2close.onValueChanged.AddListener((bool active) =>
        {
            tab2open.isOn = !active;
            tab2close.isOn = active;
            MyAudioMgr.Instance.OpenMusic(!active);
        });

        tab3open.onValueChanged.AddListener((bool active) =>
        {
            tab3open.isOn = active;
            tab3close.isOn = !active;
            MyAudioMgr.Instance.OpenEffect(active);
        });
        tab3close.onValueChanged.AddListener((bool active) =>
        {
            tab3open.isOn = !active;
            tab3close.isOn = active;
            MyAudioMgr.Instance.OpenEffect(!active);
        });

        goMusicSlider.onValueChanged.AddListener((float Value) => { MyAudioMgr.Instance.SetMusicVolume(Value); });

        goSoundSlider.onValueChanged.AddListener((float Value) => { MyAudioMgr.Instance.SetEffectVolume(Value); });
        btnResolution.AddOptions(ResolutionConfig);
        btnResolution.onValueChanged.AddListener((int value) =>
        {
            string[] split = ResolutionConfig[value].Split('x');
            PlayerPrefs.SetInt(PlayerPrefKey.ScreenWidth, int.Parse(split[0]));
            PlayerPrefs.SetInt(PlayerPrefKey.ScreenHeight, int.Parse(split[1]));
        });
    }

    List<string> ResolutionConfig = new List<string>
    {
        "2160x3840",
        "1440x2560",
        "1080x1920",
        "900x1600",
        "720x1280",
        "576x1024",
        "450x800",
    };

    public override void OnShow()
    {
        base.OnShow();
        tab1.isOn = true;

        tab2open.isOn = PlayerPrefs.GetInt(PlayerPrefKey.MusicOn) == 1;
        tab3open.isOn = PlayerPrefs.GetInt(PlayerPrefKey.SoundOn) == 1;
        goMusicSlider.value = PlayerPrefs.GetFloat(PlayerPrefKey.MusicVoulume, 1.0f);
        goSoundSlider.value = PlayerPrefs.GetFloat(PlayerPrefKey.SoundVoulume, 1.0f);

        for (int i = 0; i < ResolutionConfig.Count; i++)
        {
            string[] split = ResolutionConfig[i].Split('x');
            if (int.Parse(split[0]) == PlayerPrefs.GetInt("ScreenWidth", 1080))
            {
                btnResolution.value = i;
                break;
            }
        }
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