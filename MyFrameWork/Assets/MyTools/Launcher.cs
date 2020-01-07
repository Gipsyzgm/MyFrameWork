using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<PanelMgr>();
        PanelMgr.instance.OpenPanel<MenuePl>();
        QAudioSingleton.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
