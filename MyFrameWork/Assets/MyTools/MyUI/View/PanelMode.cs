using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class PanelMode : PanelBase
{
    public Button btnClose;
    public Toggle timetab;
    public Text timetabtxt;
    public Toggle timetab2;
    public Text timetab2txt;
    public Button btnConfirm;

    public override void Init(params object[] _args)
    {
        args = _args;
        CurViewPath = "MyUI/View/PanelMode";
        layer = PanelLayer.Info;
    }

    public override void InitComponent()
    {
        btnClose = curView.transform.Find("bgPopup/btnClose_Button").GetComponent<Button>();
        btnClose.onClick.AddListener(btnCloseOnClick);
        timetab = curView.transform.Find("bgPopup/goTimeList/timetab_Toggle").GetComponent<Toggle>();
        timetabtxt = curView.transform.Find("bgPopup/goTimeList/timetab_Toggle/timetabtxt_Text").GetComponent<Text>();
        timetab2 = curView.transform.Find("bgPopup/goTimeList/timetab2_Toggle").GetComponent<Toggle>();
        timetab2txt = curView.transform.Find("bgPopup/goTimeList/timetab2_Toggle/timetab2txt_Text")
            .GetComponent<Text>();
        btnConfirm = curView.transform.Find("bgPopup/btnConfirm_Button").GetComponent<Button>();
        btnConfirm.onClick.AddListener(btnConfirmOnClick);
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————

    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void btnCloseOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        Close();
    }

    public void btnConfirmOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        Close();
    }

    int[] timelist = new int[] { 20, 30 };

    public void CustomComponent()
    {
        timetabtxt.text = timelist[0].ToString() + "分钟";
        timetab2txt.text = timelist[1].ToString() + "分钟";
        timetab.onValueChanged.AddListener((bool ison) =>
        {
            AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
            if (ison)
            {
                PlayerPrefs.SetInt(PlayerPrefKey.TimeIndex, timelist[0]);
            }
        });
        timetab2.onValueChanged.AddListener((bool ison) =>
        {
            AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
            if (ison)
            {
                PlayerPrefs.SetInt(PlayerPrefKey.TimeIndex, timelist[1]);
            }
        });
    }

    public override void OnShow()
    {
        base.OnShow();

        if (PlayerPrefs.GetInt(PlayerPrefKey.TimeIndex, timelist[0]) == timelist[0])
        {
            timetab.isOn = true;
        }
        else
        {
            timetab2.isOn = true;
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