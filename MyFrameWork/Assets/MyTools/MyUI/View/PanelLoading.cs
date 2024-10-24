using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class PanelLoading : PanelBase
{
    public Slider goProgress;
    public Text txtProgress;
    public Text txtTip;

    public override void Init(params object[] _args)
    {
        args = _args;
        CurViewPath = "MyUI/View/PanelLoading";
        layer = PanelLayer.Info;
    }

    public override void InitComponent()
    {
        goProgress = curView.transform.Find("goProgress_Slider").GetComponent<Slider>();
        txtProgress = curView.transform.Find("goProgress_Slider/txtProgress_Text").GetComponent<Text>();
        txtTip = curView.transform.Find("goProgress_Slider/txtTip_Text").GetComponent<Text>();
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————

    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void CustomComponent()
    {
        goProgress.value = 0f;
        goProgress.onValueChanged.AddListener((value) => { txtProgress.text = string.Format("{0:F1}%", value * 100); });
    }

    public override void OnShow()
    {
        base.OnShow();
        EventMgr.Instance.AddEventListener<float, float, bool>(EventConst.UpdateProgressEvent, UpdateProgress);
    }


    public void UpdateProgress(float value, float duration, bool autoclose)
    {
        if (duration == 0)
        {
            goProgress.value = value;
        }
        else
        {
            goProgress.DOValue(value, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (autoclose)
                {
                    Close();
                }
            });
        }
    }


    public override void OnHide()
    {
        base.OnHide();
        EventMgr.Instance.RemoveEventListener<float, float, bool>(EventConst.UpdateProgressEvent, UpdateProgress);
    }

    public override void OnClose()
    {
        base.OnClose();
        EventMgr.Instance.RemoveEventListener<float, float, bool>(EventConst.UpdateProgressEvent, UpdateProgress);
    }
}