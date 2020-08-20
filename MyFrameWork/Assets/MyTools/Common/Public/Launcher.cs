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
        MyGameData.InitGameData();
        LanguageMgr.Init();
        await  VersionCheckMgr.Instance.Check();
        while (!VersionCheckMgr.Instance.isUpdateCheckComplete)
        {
            await new WaitForEndOfFrame();
        }
      
        if (VersionCheckMgr.Instance.isUpdateCheckComplete)
        {

            await AssetbundleMgr.Instance.Initialize();
          
        }
        PanelMgr.Instance.ClosePanel(PanelName.VersionCheckPl);
        PanelMgr.Instance.OpenPanel<MenuePl>();
        Debug.LogError("111111111111111");
        MyAudioMgr.Instance.Init();
    
    }

    // Update is called once per frame
    void Update()
    {

    }
}
