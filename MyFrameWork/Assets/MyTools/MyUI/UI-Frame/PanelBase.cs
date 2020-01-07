using UnityEngine;
using System.Collections;

public class PanelBase : MonoBehaviour {
    /// <summary>
    /// 预制体路径
    /// </summary>
	public string CurViewPath;
    /// <summary>
    /// 代表panel面板在场景中的物体
    /// </summary>
	public GameObject curView;
    public PanelLayer layer = PanelLayer.Null;
	public object[] args;

	public virtual void Init(params object[] _args)
	{
		this.args = _args;
	}
    /// <summary>
    /// 必须重写，页面获取组件,首次打开页面必须调用
    /// </summary>
    public virtual void InitComponent(){

   
	}
    /// <summary>
    /// 可重写，页面显示的逻辑。动画等
    /// </summary>
    public virtual void OnShow()
    {
        curView.SetActive(true);

    }

    public virtual void Update(){


	}
    /// <summary>
    /// 可重写，页面隐藏的逻辑。动画等
    /// </summary>
    public virtual void OnHide()
    {
        curView.SetActive(false);

    }
    /// <summary>
    /// 可重写，页面关闭的逻辑。动画等
    /// </summary>
    public virtual void OnClose(){

        Destroy(curView);
        Destroy(this);
    }
    /// <summary>
    /// 备用的自己关闭自己的方法
    /// </summary>
	protected virtual void Close()
	{
		string name = this.GetType().ToString ();
		PanelMgr.instance.ClosePanel (name);
	}   
}
