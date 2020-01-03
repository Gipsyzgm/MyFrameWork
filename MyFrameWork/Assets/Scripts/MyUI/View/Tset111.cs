using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tset111 : PanelBase {

    public Text Text;
    void Awake()
    {
        Text = transform.Find("Text_Text").GetComponent<Text>();
        CustomComponent();
    }
    //=======================上面部分自动生成，每次生成都会替换掉,不要手写东西==================

    //=====================================手写部分=============================================
    //@EndMark@
    public void CustomComponent()
    {
        
    }

}
