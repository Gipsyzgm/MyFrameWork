using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{


    // 游戏的总入口
    //注意先后顺序
    void Awake()
    {
        MyGameData.InitGameData();
        LanguageMgr.Init();
        gameObject.AddComponent<PanelMgr>();
        PanelMgr.instance.OpenPanel<MenuePl>();
        MyAudioMgr.Instance.Init();
       


    }

    // Update is called once per frame
    void Update()
    {

    }
}
