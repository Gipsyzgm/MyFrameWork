using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverPl : PanelBase {
    private Button overBt;
	public override void Init(params object[] _args)
	{
		skinPath= "Panel/OverPl";
		layer=PanelLayer.Panel;
	}

	public override void OnBeforeShow()
    {
		overBt=skin.transform.Find("overBt").GetComponent<Button>();
	    overBt.onClick.AddListener(OnOverBtClick);
	}

	private void OnOverBtClick()
	{
		Debug.Log("游戏结束");
	}

}
