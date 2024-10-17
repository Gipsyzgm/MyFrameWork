using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OverPl : PanelBase {

    public Button OverBt;
    public override void Init(params object[] _args)
    {
         CurViewPath= "MyUI/View/OverPl";
         layer = PanelLayer.Info;
    }
    public override void InitComponent()
    {
        OverBt = curView.transform.Find("OverBt_Button").GetComponent<Button>();
        OverBt.onClick.AddListener(OverBtOnClick);
        CustomComponent();
    }
    //====================上面部分自动生成，每次生成都会替换掉,不要手写东西==================

    //====================以下为手写部分，初始化补充方法为CustomComponent()==================
    //@EndMark@
    public void OverBtOnClick()
    {
        Debug.Log("游戏结束");
    }
        
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
