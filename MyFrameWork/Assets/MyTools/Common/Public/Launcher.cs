using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Launcher : MonoBehaviour
{


    // 游戏的总入口
    //注意先后顺序
    async void Awake()
    {
        DataMgr.Instance.InitAllConfig();
        LanguageMgr.Init();
        PanelMgr.Instance.OpenPanel<MenuePl>();
         
    }

    // Update is called once per frame
    void Update()
    {

    }
}
