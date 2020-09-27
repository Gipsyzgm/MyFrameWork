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
        await VersionCheckMgr.Instance.Check();
        while (!VersionCheckMgr.Instance.isUpdateCheckComplete)
        {
            await new WaitForEndOfFrame();
        }
        ABMgr.Instance.Initialize();
        DataMgr.Instance.InitAllHotConfig();
        LanguageMgr.Init();
        PanelMgr.Instance.ClosePanel(PanelName.VersionCheckPl);
        PanelMgr.Instance.OpenPanel<MenuePl>();
        MyAudioMgr.Instance.Init();
        GameObject gameObject = ABMgr.Instance.LoadPrefab("prefabs/scenemodel/tree_red_01");
        GameObject obj = Instantiate(gameObject);
        obj.transform.position = Vector3.zero;

        string[] any = TestDicExcel.Get(123).testDic3;
        for (int i = 0; i < any.Length; i++)
        {
          
            Debug.LogError(any[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
