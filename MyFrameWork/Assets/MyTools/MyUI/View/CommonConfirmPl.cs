using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class CommonConfirmPl : PanelBase {

    public Text Title;
    public Text Des;
    public Button AgreeBtn;
    public Button CancelBtn;
    public override void Init(params object[] _args)
    {
         args = _args;
         CurViewPath="MyUI/View/CommonConfirmPl";
         layer = PanelLayer.Tips;
    }
    public override void InitComponent()
    {
        Title = curView.transform.Find("BG/Title_Text").GetComponent<Text>();
        Des = curView.transform.Find("BG/Des_Text").GetComponent<Text>();
        AgreeBtn = curView.transform.Find("BG/AgreeBtn_Button").GetComponent<Button>();
        AgreeBtn.onClick.AddListener(AgreeBtnOnClick);
        CancelBtn = curView.transform.Find("BG/CancelBtn_Button").GetComponent<Button>();
        CancelBtn.onClick.AddListener(CancelBtnOnClick);
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————
                                                                                                
    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void AgreeBtnOnClick()
    {
        if (args != null)
        {
            UnityAction action  =(UnityAction)args[2];
            action();
        }
        Close();
    }
        
    public void CancelBtnOnClick()
    {
        if (args != null)
        {
            UnityAction action = (UnityAction)args[3];
            action();
        }
    }

    public void CustomComponent()
    {
        if (args!= null)
        {
            Title.text = args[0].ToString();
            Des.text = args[0].ToString();
        }
        
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
