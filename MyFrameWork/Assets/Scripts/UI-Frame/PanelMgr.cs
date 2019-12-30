using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PanelLayer
{
    Start = 8,
    Panel = 10,
    Tips = 12,
}
public class PanelMgr : MonoBehaviour
{
    public static PanelMgr instance;
    public Transform _canvas;
    private Dictionary<string, PanelBase> dict;
    private Dictionary<PanelLayer, Transform> layer_dict;

    public Transform canvas
    {
        get { return _canvas; }
    }
    public void Awake()
    {
        instance = this;
        InitLayer();
        dict = new Dictionary<string, PanelBase>();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void InitLayer()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        if (_canvas == null)
            Debug.LogError("PanelMgr.InitLayerfail,Canvas is null");
        layer_dict = new Dictionary<PanelLayer, Transform>();
        foreach (PanelLayer pl in Enum.GetValues(typeof(PanelLayer)))
        {
            string name = pl.ToString();
            Transform transform = _canvas.Find(name);
            layer_dict.Add(pl, transform);
        }

    }
    private void SettingOrder()
    {
        if (layer_dict[PanelLayer.Tips].childCount > 0)
        {
            InitOrder(PanelLayer.Tips);
        }
        else
        {
            if (layer_dict[PanelLayer.Panel].childCount > 0)
            {
                InitOrder(PanelLayer.Panel);
            }
            else
            {
                InitOrder(PanelLayer.Start);
            }
        }
    }
    private void InitOrder(PanelLayer layer)
    {
        Canvas cvs = _canvas.GetComponent<Canvas>();
        cvs.overrideSorting = true;
        cvs.sortingOrder = 12;
    }

    public void OpenPanel<T>(string skinPath = "", params object[] _args) where T : PanelBase
    {
        string name = typeof(T).ToString();
        if (dict.ContainsKey(name))
        {
            GetPanel(name).skin.transform.SetAsLastSibling();
            if (GetPanel(name).skin.gameObject.activeInHierarchy)
            {
                return;
            }
            GetPanel(name).skin.SetActive(true);
            GetPanel(name).args = _args;
            GetPanel(name).OnOpen();
            return;
        }
        PanelBase panel = canvas.gameObject.AddComponent<T>();
        panel.Init(_args);
        dict.Add(name, panel);
        skinPath = (skinPath != "" ? skinPath : panel.skinPath);
        GameObject skin = Resources.Load<GameObject>(skinPath);
        if (skin == null)
            Debug.LogError("panelMgr.OpenPanelfail,skin is null,skinPath= " + skinPath);
        panel.skin = (GameObject)Instantiate(skin);
        panel.skin.transform.SetAsLastSibling();
        Transform skinTrans = panel.skin.transform;
        PanelLayer layer = panel.layer;
        Transform parent = layer_dict[layer];
        skinTrans.SetParent(parent, false);
        InitOrder(layer);
        panel.OnShowed();
        panel.OnOpen();
    }
    public void OpenPanel(PanelName panelName, params object[] _args)
    {
        string name = panelName.ToString();
        if (dict.ContainsKey(name))
        {
            GetPanel(name).skin.transform.SetAsLastSibling();
            if (GetPanel(name).skin.gameObject.activeInHierarchy)
            {
                return;
            }
            GetPanel(name).skin.SetActive(true);
            GetPanel(name).args = _args;
            GetPanel(name).OnOpen();
            return;
        }
    }
    public void CloseAllPanel(string except = "")
    {
        if (dict == null)
            return;
        string[] key_strs = new string[dict.Count];
        dict.Keys.CopyTo(key_strs, 0);
        for (int i = 0; i < key_strs.Length; i++)
        {
            string s = key_strs[i];
            if (s == except)
                continue;
            ClosePanel(s);
        }
    }

    public PanelBase GetPanel(PanelName _name)
    {
        return GetPanel(_name.ToString());
    }
    public PanelBase GetPanel(string name)
    {
        PanelBase panel;
        if (dict.TryGetValue(name, out panel))
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
    public void ClosePanel(string name)
    {
        PanelBase panel;
        dict.TryGetValue(name, out panel);
        //PanelBase panel = (PanelBase)dict [name];
        if (panel == null)
            return;
        panel.OnHide();
        dict.Remove(name);
        panel.OnClosed();
        panel.skin.transform.SetParent(canvas.transform);
        GameObject.Destroy(panel.skin);
        SettingOrder();
        Component.Destroy(panel);
    }
    public void HidePanel(PanelName panelName)
    {
        PanelBase panel = GetPanel(panelName);
        if (panel != null)
        {
            if (!panel.skin.activeInHierarchy)
                return;
            panel.skin.SetActive(false);
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

