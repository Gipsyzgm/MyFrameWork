using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] public GameBasicInfoEditor GameBasicInfo;

    //游戏的总入口
    //注意先后顺序
    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DataMgr.Instance.InitAllConfig();
        LanguageMgr.Init();
        MyAudioMgr.Instance.Init();
        NetMgr.Instance.Init();
        PanelMgr.Instance.OpenPanel<MenuePl>();
        
        
        string server = GameBasicInfo.LoginServer;
        string str = server.Substring(7);
        string[] array = str.Split(':');
        string serverIp = array[0];
        string serverPort = array[1];
        Debug.Log("serverIp:" + serverIp + "  serverPort:" + serverPort);
        if (NetMgr.Instance.IsNetworkConnected())
        {
            
        }
        else
        {
            NetMgr.Instance.ConnectServer(serverIp, serverPort);
        }
    }


    void OnApplicationPause(bool pauseStatus)
    {
        NetMgr.Instance.SetPause(pauseStatus);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        NetMgr.Instance.SetPause(!hasFocus);
    }
}