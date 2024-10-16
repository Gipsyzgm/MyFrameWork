using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;


public class GameSocket : GameSocketBase
{
    private Socket socket = null;
    private EndPoint endpoint = null;
    private byte[] receiveBuffer = null;
    private Queue<byte[]> sendQueue = null;
    private bool isClosing = false;
    [SerializeField, Min(0f)] public float minConnectElapseTime = 3f;
    public override bool isConnected => socket != null && socket.Connected;

    public override void Init()
    {
        base.Init();
        sendQueue = new Queue<byte[]>();
        receiveBuffer = new byte[bufferSize.y];
        msgProcess = new GameMsgProcess();
        msgProcess.Init();
        msgProcess.socket = this;
    }

    public override void connect(string ip, int port)
    {
        if (Time.realtimeSinceStartup - lastConnectTime < minConnectElapseTime)
            return;
        lastConnectTime = Time.realtimeSinceStartup;
        connect(endpoint = new IPEndPoint(IPAddress.Parse(ip), port));
    }

    private void connect(EndPoint endPoint)
    {
        try
        {
            if (socket != null)
                close();
            socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.SendBufferSize = bufferSize.x;
            socket.ReceiveBufferSize = bufferSize.y;
            socket.Blocking = true;
            socket.BeginConnect(endPoint, endConnect, socket);
        }
        catch (Exception e)
        {
            exceptionClose(e);
        }
    }

    private void endConnect(IAsyncResult ar)
    {
        var socket = ar.AsyncState as Socket;
        try
        {
            if (socket.Connected)
            {
                Debug.Log($"{GetType().Name} connected!");
                beginReceive();
                msgProcess?.onconnect();
                onconnect?.Invoke();
                if (sendQueue.Count > 0)
                    send(null);
            }
        }
        catch (Exception e)
        {
            exceptionClose(e);
        }
    }

    public override void reconnect()
    {
        if (endpoint == null)
            return;
        Debug.Log($"{GetType().Name} reconnect()");
        close();
        connect(endpoint);
    }

    private void beginReceive()
    {
        try
        {
            if (isConnected)
                socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, endReceive, socket);
            else
                Debug.Log($"{GetType().Name}.beginReceive(). socket is closed!");
        }
        catch (Exception e)
        {
            exceptionClose(e);
        }
    }

    private void endReceive(IAsyncResult ar)
    {
        try
        {
            var socket = ar.AsyncState as Socket;
            var num = socket.EndReceive(ar);
            if (num > 0)
            {
                msgProcess?.receivemsg(receiveBuffer, 0, num);
                beginReceive();
            }
            else
            {
                Debug.Log($"{GetType().Name} Server({endpoint}) disconnect!");
                ondisconnect?.Invoke();
                msgProcess?.ondisconnect();
            }
        }
        catch (Exception e)
        {
            if (isClosing == false)
                exceptionClose(e);
        }
    }

    public override void send(byte[] bytes)
    {
        try
        {
            if (bytes != null && sendQueue.Contains(bytes) == false)
                sendQueue.Enqueue(bytes);
            if (sendQueue.Count > 0)
            {
                if (isConnected == false)
                    reconnect();
                else
                {
                    var sndmsg = sendQueue.Dequeue();
                    socket.BeginSend(sndmsg, 0, sndmsg.Length, SocketFlags.None, endSend, socket);
                }
            }
        }
        catch (Exception e)
        {
            exceptionClose(e);
        }
    }

    private void endSend(IAsyncResult ar)
    {
        try
        {
            var socket = ar.AsyncState as Socket;
            var sndlen = socket.EndSend(ar);
            if (sndlen > 0)
                send(null);
        }
        catch (Exception e)
        {
            exceptionClose(e);
        }
    }

    public override void disconnect()
    {
        try
        {
            if (isConnected == false)
                return;
            socket.BeginDisconnect(true, endDisconnect, socket);
        }
        catch (Exception e)
        {
            exceptionClose(e);
        }
    }

    private void endDisconnect(IAsyncResult ar)
    {
        try
        {
            var socket = ar.AsyncState as Socket;
            socket.EndDisconnect(ar);
        }
        catch (Exception e)
        {
            exceptionClose(e);
        }
    }

    public override void close()
    {
        try
        {
            if (isConnected)
            {
                socket.Shutdown(SocketShutdown.Both);
                isClosing = true;
            }
        }
        finally
        {
            Thread.Sleep(10);
            if (socket != null)
                socket.Close();
            isClosing = false;
        }
    }

    public void onDestroy()
    {
        close();
    }
}