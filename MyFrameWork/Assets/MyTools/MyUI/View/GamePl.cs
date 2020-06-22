using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GamePl : PanelBase {

    public Button GameBt;
    public Transform Content;
    public override void Init(params object[] _args)
    {
         CurViewPath="MyUI/View/GamePl";
         layer = PanelLayer.Panel;
    }
    public override void InitComponent()
    {
        GameBt = curView.transform.Find("GameBt_Button").GetComponent<Button>();
        GameBt.onClick.AddListener(GameBtOnClick);
        Content = curView.transform.Find("Scroll View/Viewport/Content_Transform").transform;
        CustomComponent();
    }
    //====================上面部分自动生成，每次生成都会替换掉,不要手写东西==================

    //====================以下为手写部分，初始化补充方法为CustomComponent()==================
    //@EndMark@
    public void GameBtOnClick()
    {
        Debug.Log("生成物体");
        GameObject item = Resources.Load<GameObject>(PathItem.Item);
        for (int i = 0; i < 30; i++)
        {
            GameObject gameObject =  Instantiate(item, Content);               
            Item Myitem = gameObject.AddComponent<Item>();
            Myitem.index = i;
            Myitem.InitComponent();
            Debug.LogError(Myitem.index);
        }

    }
        
    public void CustomComponent()
    {
        
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
