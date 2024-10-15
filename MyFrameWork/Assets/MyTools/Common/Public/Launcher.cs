using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    
    [SerializeField]
    public GameObject startUpCfgReference;
    
    //游戏的总入口
    //注意先后顺序
    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DataMgr.Instance.InitAllConfig();
        LanguageMgr.Init();
        MyAudioMgr.Instance.Init();
        PanelMgr.Instance.OpenPanel<MenuePl>();
         
    }

    // Update is called once per frame
    void Update()
    {

    }
}
