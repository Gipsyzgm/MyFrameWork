using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour {

    public static UIMgr instance;

    public Dictionary<string, UIBase> uiList = new Dictionary<string, UIBase>();

    void Awake()
    {
        instance = this;
    }
    public void Open(string ui)
    {
        if (uiList.ContainsKey(ui)) return;
        //GameObject obj = Resources.Load(ui) as GameObject;

        //uiObj.name = obj.name;
        //Type type = System.Reflection.Assembly.Load("Assembly-CSharp").GetType(uiObj.name);
        //UIBase uiBase = uiObj.AddComponent(type) as UIBase;
        //uiList[ui] = uiBase;
        //uiBase.gameObject.SetActive(true);
        //uiBase.Open();
    }
    public void Close(string ui)
    {
        if (!uiList.ContainsKey(ui)) return;
        UIBase uiBase = uiList[ui];
        uiBase.Close();
        uiList.Remove(ui);
        Destroy(uiBase.gameObject);
    }
    public T Get<T>(string ui) where T : UIBase
    {
        T son = uiList[ui] as T;
        return son;
    }
}
