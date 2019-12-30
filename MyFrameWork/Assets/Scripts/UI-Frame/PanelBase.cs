using UnityEngine;
using System.Collections;

public class PanelBase : MonoBehaviour {

	public string skinPath;
	public GameObject skin;//代表panel面板在场景中的物体
	public PanelLayer layer;
	public object[] args;
    private  GameObject currentPanelBk;//标记当前显示面板背景
   
	public virtual void Init(params object[] _args)
	{
		this.args = _args;
	}

    public virtual void OnShowed(){

	}
    public virtual void OnOpen()
    {

    }
    public virtual void Update(){

	}
    public virtual void OnHide()
    {

    }
    public virtual void OnClosed(){

	}
	protected virtual void Close()
	{
		string name = this.GetType ().ToString ();
		PanelMgr.instance.ClosePanel (name);
	}
   
}
