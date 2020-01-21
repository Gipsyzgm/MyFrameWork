using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{


    // 游戏的总入口

    void Start()
    {
        MyGameData.InitGameData();
        gameObject.AddComponent<PanelMgr>();
        PanelMgr.instance.OpenPanel<MenuePl>();
        MyAudioMgr.Instance.Init();
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
