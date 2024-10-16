using Google.Protobuf;
using System.Collections.Generic;
using UnityEngine;
using XOProto;
using System.Linq;

public class MsgSend
{
    private static void sendmsg<T>(E_MSGID msgid, T data) where T : IMessage<T>, new()
    {
         NetMgr.Instance.SendMsg((int)msgid, data);
    }

    #region 用户

    /// <summary>
    /// 请求心跳
    /// </summary>
    public static void SendUserHeart()
    {
        C2S_User_QHeart data = new C2S_User_QHeart();
        sendmsg(E_MSGID.UserHeart, data);
    }

    public static void SendLogin(string token, string version)
    {
        C2S_User_Login data = new C2S_User_Login();
        data.DyToken = token;
        data.Version = version;
        sendmsg(E_MSGID.UserLogin, data);
    }

    /// <summary>
    /// 重连
    /// </summary>
    public static void SendUserAuth(string session)
    {
        C2S_User_Auth data = new C2S_User_Auth();
        data.Session = session;
        sendmsg(E_MSGID.UserAuth, data);
    }

    #endregion 用户


    public static void SendGameStart(int timeIndex)
    {
        C2S_Game_Start data = new C2S_Game_Start();
        data.Time = 1;
        sendmsg(E_MSGID.GameStart, data);
    }

    public static void SendGameRecord(long scoreTotal, long scoreRemain, long scoreThrethold)
    {
        C2S_Game_Record data = new C2S_Game_Record();
        data.ScorePool = scoreTotal;
        data.ScoreRemain = scoreRemain;
        
        sendmsg(E_MSGID.GameRecord, data);
    }

    public static void SendGameRankQuery(int type)
    {
        C2S_Game_Rank_Query data = new C2S_Game_Rank_Query();
        data.Type = type;
        sendmsg(E_MSGID.GameRankQuery, data);
    }
}
