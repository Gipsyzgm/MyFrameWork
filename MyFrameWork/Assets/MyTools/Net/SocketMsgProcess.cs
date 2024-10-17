using System;
using System.Collections;
using System.IO;
using Google.Protobuf;
using UnityEngine;
using XOProto;

public abstract class PacketMsgProcess
{
    public GameSocketBase socket { get; internal set; }

    public Action<Response> onResponseError;

    [SerializeField] protected PacketHeadSize packetHeadSize = PacketHeadSize.Len_4;

    [SerializeField] protected bool littleEndianReverse = true;

    [SerializeField] public PrintLevel printLevel = PrintLevel.PacketHead_And_EditorBody;

    //设置屏蔽的msgID,不可为空,可以屏蔽高频不重要的信息。
    [SerializeField] private int[] printExceptMsgIds = new[] { 1006 };

    [NonSerialized] protected MemoryStream sndstream;

    [NonSerialized] protected MemoryStream rcvstream;

    [NonSerialized] private int packetLength;

    [NonSerialized] private object rcvLocker = new object();

    [NonSerialized] private bool isDestroy = false;

    public enum PacketHeadSize
    {
        Len_0 = 0,
        Len_2 = 2,
        Len_4 = 4
    }

    public enum PrintLevel
    {
        NoPrint = 0,
        PacketHeadOnly,
        PacketHead_And_Body,
        PacketHead_And_EditorBody
    }

    public virtual void Init()
    {
        sndstream = new MemoryStream();
        rcvstream = new MemoryStream();
        Array.Sort(printExceptMsgIds);
        isDestroy = false;
    }

    public virtual void OnDestroy()
    {
        sndstream.Dispose();
        rcvstream.Dispose();
        sndstream = null;
        rcvstream = null;
        isDestroy = true;
    }

    public virtual void sendmsg(byte[] msg)
    {
        if (isDestroy)
            return;
        var lenBytes = BitConverter.GetBytes(msg.Length);
        if (littleEndianReverse && BitConverter.IsLittleEndian)
            Array.Reverse(lenBytes);
        sndstream.Seek(0, SeekOrigin.Begin);
        sndstream.Write(lenBytes);
        sndstream.Write(msg);
        sndstream.Flush();
        var buf = new byte[sndstream.Position];
        Buffer.BlockCopy(sndstream.GetBuffer(), 0, buf, 0, buf.Length);
        socket.send(buf);
    }

    public virtual void sendpacket(int msgid, object msg)
    {
    }

    public virtual void sendpacket(int msgid, IMessage msg)
    {
    }

    protected virtual int readPacketLength(byte[] bytes)
    {
        var span = new Span<byte>(bytes, 0, 4);
        if (littleEndianReverse && BitConverter.IsLittleEndian)
            span.Reverse();
        switch (packetHeadSize)
        {
            default:
                throw new Exception($"GameMsgProcess.readPacketLength(). packetHeadSize = {packetHeadSize}");
            case PacketHeadSize.Len_4: return BitConverter.ToInt32(span);
            case PacketHeadSize.Len_2: return BitConverter.ToUInt16(span);
        }
    }

    public virtual void receivemsg(byte[] bytes, int offset, int length)
    {
        if (isDestroy)
            return;
        lock (rcvLocker)
        {
            rcvstream.Write(bytes, offset, length);
        }
    }


    public virtual void onconnect()
    {
    }

    public virtual void ondisconnect()
    {
    }

    public void update()
    {
        if (rcvstream == null)
            return;
        lock (rcvLocker)
        {
            receivemsg();
        }
    }

    private void receivemsg()
    {
        if (isDestroy)
            return;
        var buf = rcvstream.GetBuffer();
        var headSize = (int)packetHeadSize;
        if (packetLength == 0)
        {
            if (rcvstream.Position >= headSize)
            {
                packetLength = readPacketLength(buf);
                receivemsg();
            }
        }
        else if (rcvstream.Position - headSize >= packetLength)
        {
            try
            {
                var offst = headSize + packetLength;
                var subLen = (int)rcvstream.Position - offst;
                receivepacket(buf, headSize, packetLength);
                Buffer.BlockCopy(buf, offst, buf, 0, subLen);
                rcvstream.Seek(subLen, SeekOrigin.Begin);
                packetLength = 0;
            }
            catch (Exception e)
            {
                rcvstream.Seek(packetLength = 0, SeekOrigin.Begin);
                Debug.LogError(e.ToString());
            }
        }
    }

    protected virtual void receivepacket(byte[] bytes, int offset, int count)
    {
    }


    protected virtual void createMsgHandles()
    {
    }

    public virtual void listenReceive(int msgid, Action<object> callback)
    {
    }

    public virtual void listenReceive<T>(Action<T> callback) where T : IMessage<T>, new()
    {
    }

    public bool checkprintabled(int msgid)
    {
        return printLevel > PrintLevel.NoPrint && Array.BinarySearch(printExceptMsgIds, msgid) < 0;
    }
}