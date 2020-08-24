using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class CommonConfirmPl : PanelBase {

    public Text Title;
    public Text Des;
    public Button AgreeBtn;
    public Text AgreeText;
    public Button CancelBtn;
    public Text CancelText;
    public override void Init(params object[] _args)
    {
         args = _args;
         CurViewPath= "Prefabs/MyUI/View/CommonConfirmPl";
         layer = PanelLayer.Tips;
    }
    public override void InitComponent()
    {
        Title = curView.transform.Find("BG/Title_Text").GetComponent<Text>();
        Des = curView.transform.Find("BG/Des_Text").GetComponent<Text>();
        AgreeBtn = curView.transform.Find("BG/AgreeBtn_Button").GetComponent<Button>();
        AgreeBtn.onClick.AddListener(AgreeBtnOnClick);
        AgreeText = curView.transform.Find("BG/AgreeBtn_Button/AgreeText_Text").GetComponent<Text>();
        CancelBtn = curView.transform.Find("BG/CancelBtn_Button").GetComponent<Button>();
        CancelBtn.onClick.AddListener(CancelBtnOnClick);
        CancelText = curView.transform.Find("BG/CancelBtn_Button/CancelText_Text").GetComponent<Text>();
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————
                                                                                                
    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void AgreeBtnOnClick()
    {
        if (args != null)
        {
            UnityAction action  =(UnityAction)args[4];
            action();
        }
        Close();
    }
        
    public void CancelBtnOnClick()
    {
        if (args != null)
        {
            UnityAction action = (UnityAction)args[5];
            action();
        }
        Close();
    }

    public void CustomComponent()
    {
        if (args!= null)
        {
            Title.text = args[0].ToString();
            Des.text = args[1].ToString();
            AgreeText.text = args[2].ToString();
            CancelText.text = args[3].ToString();
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
