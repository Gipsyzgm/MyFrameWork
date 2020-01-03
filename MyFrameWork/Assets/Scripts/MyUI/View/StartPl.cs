using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StartPl : PanelBase {

    public Button startBt;
    public Text Text;
    public override void Init(params object[] _args)
    {
         skinPath="MyUI/View/StartPl";
        
    }
    public override void OnBeforeShow()
    {
        startBt = transform.Find("startBt_Button").GetComponent<Button>();
        startBt.onClick.AddListener(startBtOnClick);
        Text = transform.Find("startBt_Button/Text_Text").GetComponent<Text>();
        CustomComponent();
    }
    //====================上面部分自动生成，每次生成都会替换掉,不要手写东西==================

    //====================以下为手写部分，初始化补充方法为CustomComponent()==================
    //@EndMark@
    public override void OnHide()
    {
         skin.SetActive(false);    
    }
    public override void Update()
    {
        
    }
    public void startBtOnClick()
    {
        
    }
    public void CustomComponent()
    {
        
    }
    public override void OnShowed()
    {
        skin.SetActive(true);
        MyLog.LogError("ggg");
    }
    public override void OnClosed()
    {
         Destroy(skin);   
         Destroy(this);   
    }
}
