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

    /// <summary>
    /// 用于记录当前页面显示顺序并实现关闭当前页面功能。
    /// </summary>
    private List<string> ExictPanel;


    public Transform canvas
    {
        get { return _canvas; }
    }
    public void Awake()
    {
        instance = this;
        InitLayer();
        Paneldict = new Dictionary<string, PanelBase>();
        ExictPanel = new List<string>();
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
                AddToListLast(name);
                return;
            }
            GetPanel(name).args = _args;
            AddToList(name);
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
        AddToList(name);
        panel.OnBeforeShow();        
        panel.OnShowed();
    }

    /// <summary>
    /// 用于打开已经打开过的页面隐藏显示或者靠后页面置顶
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
    /// 依次关闭最上层的panel
    /// </summary>
    public void CloseCurrentPanel()
    {
        if (ExictPanel== null)
        {
            MyLog.LogError("所有页面关闭或ExictPanelList异常");
        }
        if (ExictPanel.Count==1)
        {
            MyLog.LogWithColor("除主页面和隐藏页面外已经全部关闭", Color.green);
            return;
        }
        string name = ExictPanel[ExictPanel.Count - 1];
        ClosePanel(name);    
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
        RomoveToList(name);
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
            RomoveToList(panelName.ToString());
            panel.OnHide();      
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
    /// 每次未关闭页面置顶的时候放到最后面
    /// </summary>
    /// <param name="item"></param>
    private void AddToListLast(string item)
    {
        ExictPanel.Remove(item);
        ExictPanel.Add(item);
    }
    /// <summary>
    /// 每次打开页面或者隐藏页面显示时Add一次
    /// </summary>
    /// <param name="item"></param>
    private void AddToList(string item)
    {
        ExictPanel.Add(item);
    }
    /// <summary>
    /// 每次隐藏或者关闭Romove掉
    /// </summary>
    /// <param name="item"></param>
    private void RomoveToList(string item)
    {
        ExictPanel.Remove(item);     
    }

}

public enum PanelName
{
    StartPl,
    GamePl,
    OverPl,
    SettingPl
}

