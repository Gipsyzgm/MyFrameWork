using System.Collections;
using Google.Protobuf;
using UnityEngine;
using XOProto;

public class NetMgr : MonoSingleton<NetMgr>
{
    static float lastTime = 0;
    static float pingInterval = 1;
    static bool isReconnect = false;
    static bool inited = false;
    public GameSocket gameSocket;
    public static int sendTimes;
    public static float beginSilentTime;
    public static bool enablePing = false;
    public void Init()
    {
        gameSocket = new GameSocket();
        gameSocket.Init();
        InitNetwork();
        DataInfoMgr.Instance.RegistEvent();
    }

    public void Update()
    {
        gameSocket.msgProcess.update();
        if (enablePing)
        {
            lastTime += Time.deltaTime;
            if (lastTime > pingInterval)
            {
                MsgSend.SendUserHeart();
                lastTime = 0;
                sendTimes++;
                if (sendTimes >= 15)
                {
                    sendTimes = 0;
                    OnNetworkError();
                }
            }
        }

        // if (CommonData.isInGame)
        // {
        //     ViewMgr.Instance.Update();
        //     PoolManager.Instance.Update();
        //     Performance.Instance.Update();
        // }
    }

    private void InitNetwork()
    {
        gameSocket.onconnect += OnNetworkConnected;
        gameSocket.onreconnect += Reconnect;
        gameSocket.onerror += OnNetworkError;
        gameSocket.msgProcess.onResponseError += OnResponseError;
    }

    static void OnNetworkConnected()
    {
        Debug.LogError("OnNetworkConnected");
        isReconnect = false;
        enablePing = true;
        lastTime = 0;
        //Loom.QueueOnMainThread(() => Messenger.Broadcast(EventConst.NETWORK_CONNECTED));
    }

    void Reconnect()
    {
        Debug.Log("Reconnect isReconnect ===========> " + isReconnect);
        if (isReconnect == false)
        {
            isReconnect = true;
            Debug.Log("Broadcast NETWORK_RECONNECT");
            //Loom.QueueOnMainThread(() => Messenger.Broadcast(EventConst.NETWORK_RECONNECT));
        }

        if (IsNetworkConnected() == false)
            gameSocket.reconnect();
    }

    public void ConnectServer(string server, string port)
    {
        gameSocket.connect(server, int.Parse(port));
        inited = true;
    }

    static void OnResponseError(Response response)
    {
        //Loom.QueueOnMainThread(() => Messenger.Broadcast(EventConst.RESPONSE_ERROR, response));
    }

    static void OnNetworkError()
    {
        enablePing = false;
        Debug.LogError("OnNetworkError");
        //Loom.QueueOnMainThread(() => Messenger.Broadcast(EventConst.NETWORK_ERROR));
    }

    public void SetPause(bool pause)
    {
        if (!inited) return;
        if (pause)
            beginSilentTime = Time.realtimeSinceStartup;
        else
        {
            if ((Time.realtimeSinceStartup - beginSilentTime) > 3600)
            {
                Debug.LogError("BgTimeout OnApplicationFocus true");
                OnBackgroundTimeout();
                beginSilentTime = float.MaxValue;
            }
            else
            {
                if (gameSocket.isConnected == false)
                    gameSocket.reconnect();
            }
        }
    }

    public void OnBackgroundTimeout()
    {
        //Loom.QueueOnMainThread(() => Messenger.Broadcast(EventConst.BG_TIMEOUT));
    }

    public void Disconnect()
    {
        enablePing = false;
        gameSocket.disconnect();
        //UPlayerPrefs.SetPlayerId(0);
    }

    public bool IsNetworkConnected()
    {
        return gameSocket.isConnected;
    }

    public void ClearSendTimes()
    {
        sendTimes = 0;
    }
    
    public void SendMsg(int msgid, object msg)
    {
        gameSocket.msgProcess.sendpacket((int)msgid, msg);
    }
    
}