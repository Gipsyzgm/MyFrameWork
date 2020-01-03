using UnityEngine;
using System.Collections;

public class PanelBase : MonoBehaviour {

    /// <summary>
    /// 预制体路径
    /// </summary>
	public string skinPath;
    /// <summary>
    /// 代表panel面板在场景中的物体
    /// </summary>
	public GameObject skin;
    public PanelLayer layer = PanelLayer.Null;
	public object[] args;

	public virtual void Init(params object[] _args)
	{
		this.args = _args;
	}
    /// <summary>
    /// 必须重写，页面获取组件,首次打开页面必须调用
    /// </summary>
    public virtual void OnBeforeShow(){


	}
    /// <summary>
    /// 可重写，页面显示的逻辑。动画等
    /// </summary>
    public virtual void OnShowed()
    {
        skin.SetActive(true);

    }

    public virtual void Update(){


	}

    /// <summary>
    /// 可重写，页面隐藏的逻辑。动画等
    /// </summary>
    public virtual void OnHide()
    {
        skin.SetActive(false);

    }

    /// <summary>
    /// 可重写，页面关闭的逻辑。动画等
    /// </summary>
    public virtual void OnClosed(){

        Destroy(skin);
        Destroy(this);
    }

	protected virtual void Close()
	{
		string name = this.GetType().ToString ();
		PanelMgr.instance.ClosePanel (name);
	}
   
}
