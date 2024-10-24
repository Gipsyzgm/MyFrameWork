using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

public class PanelLoading : PanelBase
{
    public Text txtTip;
    public Image imgProgress;
    public Text txtProgress;

    public override void Init(params object[] _args)
    {
        args = _args;
        CurViewPath = "MyUI/View/PanelLoading";
        layer = PanelLayer.Top;
    }

    public override void InitComponent()
    {
        txtTip = curView.transform.Find("goProgress/txtTip_Text").GetComponent<Text>();
        imgProgress = curView.transform.Find("goProgress/imgProgress_Image").GetComponent<Image>();
        txtProgress = curView.transform.Find("goProgress/txtProgress_Text").GetComponent<Text>();
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————

    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void CustomComponent()
    {
        
    }

    public override void OnShow()
    {
        base.OnShow();
        EventMgr.Instance.AddEventListener< float,float,bool>(EventConst.UpdateProgressEvent, UpdateProgress);
    }


    public void StartLoading(Action callback)
    {
        callback?.Invoke();
    }


    public void UpdateProgress( float value,float duration,bool autoclose)
    {
        float currentValue;
        if (duration == 0)
        {
            imgProgress.fillAmount = value;
            txtProgress.text = string.Format("{0:F1}%", value * 100);
        }
        else
        {
            currentValue = imgProgress.fillAmount * 100;
            txtProgress.text = string.Format("{0:F1}%", currentValue);
            imgProgress.DOFillAmount(value, duration).SetEase(Ease.OutBounce).OnComplete(() =>
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
        EventMgr.Instance.RemoveEventListener< float,float,bool>(EventConst.UpdateProgressEvent, UpdateProgress);
    }

    public override void OnClose()
    {
        base.OnClose();
        EventMgr.Instance.RemoveEventListener< float,float,bool>(EventConst.UpdateProgressEvent, UpdateProgress);
    }
}