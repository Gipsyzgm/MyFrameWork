using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {

    public Image Image;
    public Text Text;
    public int index = 0;
    public void InitComponent()
    {
        Image = transform.Find("Image_Image").GetComponent<Image>();
        Text = transform.Find("Text_Text").GetComponent<Text>();
        CustomComponent();
    }
    //====================上面部分自动生成，每次生成都会替换掉,不要手写东西==================

    //====================以下为手写部分，初始化补充方法为CustomComponent()==================
    //@EndMark@
    public void UpdateItem()
    {
        
    }
    public void CustomComponent()
    {
        Text.text = index.ToString();
    }
        
   
}
