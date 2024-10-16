using System;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;


public abstract class GameSocketBase
{
    [SerializeField] protected Vector2Int bufferSize = new Vector2Int(1024, 1024);
    [SerializeField] public PacketMsgProcess msgProcess;
    [NonSerialized] protected float lastConnectTime = float.MinValue;
    [NonSerialized] public Action onconnect = null;
    [NonSerialized] public Action ondisconnect = null;
    [NonSerialized] public Action onreconnect = null;
    [NonSerialized] public Action onerror = null;
    public virtual bool isConnected => false;
    public string errorString { get; protected set; }

    public virtual void Init()
    {
        if (msgProcess != null)
        {
            msgProcess.socket = this;
        }
    }

    public virtual void connect(string ip, int port)
    {
    }

    public virtual void disconnect()
    {
    }

    public virtual void reconnect()
    {
    }

    public virtual void send(byte[] bytes)
    {
    }

    public virtual void close()
    {
    }

    public void exceptionClose(Exception e)
    {
        errorString = e.Message;
        if (e is SocketException)
            onerror?.Invoke();
        close();
    }
}