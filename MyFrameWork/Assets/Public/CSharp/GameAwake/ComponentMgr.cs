using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 游戏入口
 */
public class ComponentMgr : MonoBehaviour {

    public static ComponentMgr Instance;

    //ui画布
    public Canvas uiCanvas;
    public Transform uiRoot;

	void Awake () {
        Instance = this;

        //ui
        uiCanvas = FindObjectOfType<CanvasScaler>().gameObject.GetComponent<Canvas>();

        //适配
        uiRoot.gameObject.AddComponent<UIFitter>();
        //IphoneX
        gameObject.AddComponent<SettingIphoneX>();
        //多语言
        gameObject.AddComponent<LanguageMgr>();


        gameObject.AddComponent<IResources>();

        
       

    }
    void Start()
    {
        //UIMgr.instance.Open(UIPath.MainView);
        //UIMgr.instance.Get<MainView>(UIPath.MainView).SureOnClick();

        //Instantiate(IResources.LoadRes<GameObject>("prefab/cube"));
    }

}
