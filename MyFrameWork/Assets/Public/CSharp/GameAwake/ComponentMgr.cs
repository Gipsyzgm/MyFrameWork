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
        uiRoot = uiCanvas.transform;
        //先矫正，再读数据
        gameObject.AddComponent<UpdateVersion>();
        gameObject.AddComponent<Data>();
        //UI
        gameObject.AddComponent<UIMgr>();
        //适配
        uiRoot.gameObject.AddComponent<UIFitter>();
        //IphoneX
        gameObject.AddComponent<SettingIphoneX>();
        //多语言
        gameObject.AddComponent<LanguageMgr>();
        //运行
        gameObject.AddComponent<IApplication>();
        gameObject.AddComponent<IUpdate>();
        gameObject.AddComponent<IResources>();
        //FPS
        gameObject.AddComponent<FPS>();
        //内购
        IAP.Initialize();
        
        IResources.UnloadByTick();

    }
    void Start()
    {
        //UIMgr.instance.Open(UIPath.MainView);
        //UIMgr.instance.Get<MainView>(UIPath.MainView).SureOnClick();

        //Instantiate(IResources.LoadRes<GameObject>("prefab/cube"));
    }

}
