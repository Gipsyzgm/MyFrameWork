using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPll : PanelBase {
    private Button startBt;
	public override void Init(params object[] _args)
	{
		skinPath= "Panel/StartPl";
		layer=PanelLayer.Start;
	}

	public override void OnBeforeShow()
    {
		startBt=skin.transform.Find("startBt").GetComponent<Button>();
	    startBt.onClick.AddListener(OnStartBtClick);
	}

    public override void OnShowed()
    {
        skin.SetActive(true);
    }
    public override void Update()
    {


    }
    public override void OnHide()
    {
        skin.SetActive(false);
    }

 
    public override void OnClosed()
    {

        Destroy(skin);
        Destroy(this);
    }

    private void OnStartBtClick()
	{
		Debug.Log("开始游戏");
        SceneManager.LoadScene("Test_Env");
	}

}
