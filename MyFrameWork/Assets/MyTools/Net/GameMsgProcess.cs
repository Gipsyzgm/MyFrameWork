using Google.Protobuf;
using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XOProto;


public partial class GameMsgProcess : PacketMsgProcess
{
    public Action<int, RepeatedField<AppendItem>> onReveiveAppendMsg;
    private Dictionary<int, IProtobufReceiveMsg> msgRcvHandles = new Dictionary<int, IProtobufReceiveMsg>();
    private Dictionary<Type, IProtobufReceiveMsg> msgRcvHandleTypes = new Dictionary<Type, IProtobufReceiveMsg>();
    public int packetNumber { get; private set; }

    public override void Init()
    {
        base.Init();
        createMsgHandles();
    }

    public override void OnDestroy()
    {
        msgRcvHandles.Clear();
        msgRcvHandleTypes.Clear();
        onReveiveAppendMsg = null;
        base.OnDestroy();
    }

    public override void onconnect()
    {
        packetNumber = 0;
    }

    public override void sendpacket(int msgid, object msg)
    {
        if (msg is IMessage == false)
            throw new Exception($"GameMsgProcess.sendpacket(). msg type({msg.GetType().Name}) is not IMessage.");
        sendpacket(msgid, msg as IMessage);
    }

    public override void sendpacket(int msgid, IMessage msg)
    {
        Request rqst = new Request();
        rqst.ApiId = msgid;
        rqst.Key = packetNumber++;
        rqst.Data = ByteString.CopyFrom(serProtoMsg(msg));
        if (checkprintabled(rqst.ApiId))
            printMsg(rqst, msg, printLevel, "<color=#00C6FF>[Socket] >>> ({0}) {1}</color>{2}");
        sendmsg(serProtoMsg(rqst));
    }

    private byte[] serProtoMsg(IMessage msg)
    {
        using var ms = new MemoryStream();
        using var ims = new CodedOutputStream(ms, leaveOpen: false);
        msg.WriteTo(ims);
        ims.Flush();
        return ms.ToArray();
    }

    protected override void receivepacket(byte[] bytes, int offset, int count)
    {
        var resp = Response.Parser.ParseFrom(bytes, offset, count);
        if (resp.Code != 0)
        {
            Debug.Log($"<color=#00C6FF>[Socket] <<< ({resp.ApiId}) {resp.Code}</color>");
            onResponseError?.Invoke(resp);
            return;
        }

        if (msgRcvHandles.TryGetValue(resp.ApiId, out var msgHandle))
            msgHandle.doReceive(resp);
        onReveiveAppendMsg?.Invoke(resp.ApiId, resp.AppendItems);
    }

    public override void listenReceive(int msgid, Action<object> callback)
    {
        if (msgRcvHandles.TryGetValue(msgid, out var msgHandle))
            msgHandle.addListen(callback);
    }

    public override void listenReceive<T>(Action<T> callback)
    {
        if (msgRcvHandleTypes.TryGetValue(typeof(T), out var msgHandle))
            msgHandle.addListen(callback);
    }

    public void printMsg(IProtoMessageCommunicate commu, IMessage msg, PrintLevel printLevel, string format)
    {
        if (printLevel == PrintLevel.PacketHeadOnly)
            Debug.Log(string.Format(format, commu.ApiId, msg.GetType().Name, $" {commu.Data.Length}byte"));
        else if (printLevel == PrintLevel.PacketHead_And_Body)
            Debug.Log(string.Format(format, commu.ApiId, msg.GetType().Name, $" {commu.Data.Length}byte \n{msg}"));
        else if (printLevel == PrintLevel.PacketHead_And_EditorBody)
        {
#if UNITY_EDITOR
            printMsg(commu, msg, PrintLevel.PacketHead_And_Body, format);
#else
                printMsg(commu, msg, PrintLevel.PacketHeadOnly, format);
#endif
        }
    }
}

public interface IProtobufReceiveMsg
{
    public void addListen<T>(Action<T> callback);
    public void doReceive(Response response);
}

public sealed class ProtobufReceiveMsg<T> : IProtobufReceiveMsg where T : IMessage<T>, new()
{
    private GameMsgProcess owner;
    private Action<T> onReceive;
    private MessageParser<T> parser = new MessageParser<T>(() => new T());

    public ProtobufReceiveMsg(GameMsgProcess owner)
    {
        this.owner = owner;
    }

    public void addListen<U>(Action<U> callback)
    {
        var func = callback as Action<T>;
        onReceive += func;
    }

    public void doReceive(Response resp)
    {
        if (onReceive == null)
            return;
        var msg = parser.ParseFrom(resp.Data);
        if (owner.checkprintabled(resp.ApiId))
        {
            owner.printMsg(resp, msg, owner.printLevel, "<color=#00C6FF>[Socket] <<< ({0}) {1}</color>{2}");
            if (resp.AppendItems.Count > 0)
                foreach (var appendItem in resp.AppendItems)
                    Debug.Log(appendItem.ToString());
        }

        onReceive(msg);
    }
}