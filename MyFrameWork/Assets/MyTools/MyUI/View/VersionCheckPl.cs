using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class VersionCheckPl : PanelBase {

    public Image ImageSlider;
    public Text VersionInfo;
    public Text SpeedText;
    public override void Init(params object[] _args)
    {
         args = _args;
         CurViewPath= "MyUI/View/VersionCheckPl";
         layer = PanelLayer.Info;
    }
    public override void InitComponent()
    {
        ImageSlider = curView.transform.Find("ImageSlider_Image").GetComponent<Image>();
        VersionInfo = curView.transform.Find("VersionInfo_Text").GetComponent<Text>();
        SpeedText = curView.transform.Find("SpeedText_Text").GetComponent<Text>();
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
