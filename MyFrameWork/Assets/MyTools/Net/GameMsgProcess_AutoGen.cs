using Google.Protobuf;


public partial class GameMsgProcess : PacketMsgProcess
{
    private void addHandle<T>(int msgid) where T : IMessage<T>, new()
    {
        msgRcvHandles[msgid] = msgRcvHandleTypes[typeof(T)] = new ProtobufReceiveMsg<T>(this);
    }

    protected override void createMsgHandles()
    {
        addHandle<S2C_Game_Start>(1201);
        addHandle<S2C_Game_Record>(1203);
        addHandle<S2C_Game_Rank_Query>(1202);
        addHandle<S2C_Game_PayloadBro>(1281);
        addHandle<S2C_User_Create>(1001);
        addHandle<S2C_User_Login>(1002);
        addHandle<S2C_User_Auth>(1003);
        addHandle<S2C_User_LogoutBro>(1005);
        addHandle<S2C_User_Heart>(1006);
        addHandle<S2C_User_Room_Create>(1020);
        addHandle<S2C_User_Room_Add>(1021);
        addHandle<S2C_User_Room_ChangeBro>(1081);
        addHandle<S2C_User_Room_Quit>(1022);
        addHandle<S2C_User_Room_Query>(1023);
        addHandle<S2C_User_Ready_Change>(1026);
        addHandle<S2C_User_Invite_Send>(1024);
        addHandle<S2C_User_Invite_Query>(1025);
    }
}