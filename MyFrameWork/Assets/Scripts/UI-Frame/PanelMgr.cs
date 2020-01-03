using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PanelLayer
{
    Null = 0,
    Start = 8,
    Panel = 10,
    Tips = 12   
}
public class PanelMgr : MonoBehaviour
{
    public static PanelMgr instance;
    public Transform _canvas;
    private Dictionary<string, PanelBase> Paneldict;
    private Dictionary<PanelLayer, Transform> layer_dict;


    public Transform canvas
    {
        get { return _canvas; }
    }
    public void Awake()
    {
        instance = this;
        InitLayer();
        Paneldict = new Dictionary<string, PanelBase>();
      
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void InitLayer()
    {
        Debug.LogError("初始化layer");
        _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        if (_canvas == null)
            Debug.LogError("PanelMgr.InitLayerfail,Canvas is null");
        layer_dict = new Dictionary<PanelLayer, Transform>();
        //存layer的位置
        foreach (PanelLayer pl in Enum.GetValues(typeof(PanelLayer)))
        {
            string name = pl.ToString();
            Transform transform = _canvas.Find(name);
            layer_dict.Add(pl, transform);
        }
    }
    /// <summary>
    /// 首次打开必须根据类型打开页面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="skinPath"></param>
    /// <param name="_args"></param>
    public void OpenPanel<T>(string skinPath = "", params object[] _args) where T : PanelBase
    {
        string name = typeof(T).ToString();
        if (Paneldict.ContainsKey(name))
        {
            //设置页面在最后面（摄像机最先渲染位置）
            GetPanel(name).skin.transform.SetAsLastSibling();
            if (GetPanel(name).skin.gameObject.activeInHierarchy)
            {
                return;
            }
            GetPanel(name).args = _args;
            GetPanel(name).OnShowed();
            return;
        }
        PanelBase panel = canvas.gameObject.AddComponent<T>();
        panel.Init(_args);
        Paneldict.Add(name, panel);
        skinPath = (skinPath != "" ? skinPath : panel.skinPath);
        GameObject skin = Resources.Load<GameObject>(skinPath);
        if (skin == null)
            Debug.LogError("panelMgr.OpenPanelfail,skin is null,skinPath= " + skinPath);
        panel.skin = (GameObject)Instantiate(skin);
        panel.skin.transform.SetAsLastSibling();
        Transform skinTrans = panel.skin.transform;
        PanelLayer layer;
        if (panel.layer == PanelLayer.Null)
        {
            MyLog.LogError(panel.skin.name + "未设置PanelLayer,默认放在最后");
            layer = PanelLayer.Tips;
        }
        else
        {
            layer = panel.layer;
        }       
        Transform parent = layer_dict[layer];
        skinTrans.SetParent(parent, false);
        //每个脚本第一次打开必须执行OnBeforeShow
        panel.OnBeforeShow();
        panel.OnShowed();
    }

    /// <summary>
    /// 用于打开已经打开过的页面
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="_args"></param>
    public void OpenHidePanel(PanelName panelName, params object[] _args)
    {
        string name = panelName.ToString();
        if (Paneldict.ContainsKey(name))
        {
            GetPanel(name).skin.transform.SetAsLastSibling();
            if (GetPanel(name).skin.gameObject.activeInHierarchy)
            {
                return;
            }
            GetPanel(name).args = _args;
            GetPanel(name).OnShowed();
            return;
        }
        else
        {
            MyLog.LogError(panelName.ToString()+ ":页面打开失败，页面不存在。尝试使用OpenPanel<T>打开页面");
        }
    }

    /// <summary>
    /// 谨慎使用，会关闭所有UI
    /// </summary>
    /// <param name="except"></param>
    public void CloseAllPanel(string except = "")
    {
        if (Paneldict == null)
            return;
        string[] key_strs = new string[Paneldict.Count];
        Paneldict.Keys.CopyTo(key_strs, 0);
        for (int i = 0; i < key_strs.Length; i++)
        {
            string s = key_strs[i];
            if (s == except)
                continue;
            ClosePanel(s);
        }
    }

    /// <summary>
    /// 通过枚举获得panel
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public PanelBase GetPanel(PanelName _name)
    {
        return GetPanel(_name.ToString());
    }
    /// <summary>
    /// 通过名字获得panel
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PanelBase GetPanel(string name)
    {
        PanelBase panel;
        if (Paneldict.TryGetValue(name, out panel))
        {
            return panel;
        }
        return null;
    }
    public T GetPanel<T>(PanelName _name) where T : PanelBase
    {
        return GetPanel(_name) as T;
    }

    public void ClosePanel(PanelName _name)
    {
        ClosePanel(_name.ToString());
    }
    /// <summary>
    /// 关闭页面
    /// </summary>
    /// <param name="name"></param>
    public void ClosePanel(string name)
    {
        PanelBase panel;
        Paneldict.TryGetValue(name, out panel);
        if (panel == null)
            return;
        Paneldict.Remove(name);
        panel.OnClosed();
       
    }
    /// <summary>
    /// 隐藏页面
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(PanelName panelName)
    {
        PanelBase panel = GetPanel(panelName);
        if (panel != null)
        {
            if (!panel.skin.activeInHierarchy)
                return;
            Debug.LogError("隐藏物体名?"+panelName.ToString());
            panel.OnHide();      
        }
    }


}

public enum PanelName
{
    StartPl,
    GamePl,
    OverPl,
    SettingPl
}

