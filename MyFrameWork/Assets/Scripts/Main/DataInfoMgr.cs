using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XOProto;
using pb = global::Google.Protobuf;
using Random = UnityEngine.Random;

public class DataInfoMgr : Singleton<DataInfoMgr>
{
    public string roomId { get; private set; }
    public int firstGiftCount { get; private set; }
    public int gameOver { get; private set; }
    public string roundNum { get; private set; }

    public Dictionary<int, UserDataInfo> UserDataDict { get; private set; }

    public void RegistEvent()
    {
        var msgpro = NetMgr.Instance.gameSocket.msgProcess;

        #region 用户

        msgpro.listenReceive<S2C_User_Heart>(OnUserHeart);
        msgpro.listenReceive<S2C_User_Login>(OnUserLogin);
        msgpro.listenReceive<S2C_Game_Start>(OnGameStart);
        msgpro.listenReceive<S2C_Game_Record>(OnGameRecord);
        msgpro.listenReceive<S2C_Game_Rank_Query>(OnGameRankQuery);
        msgpro.listenReceive<S2C_Game_PayloadBro>(OnGamePlayerMsgBro);

        #endregion 用户
    }

    public void InitData()
    {
    }

    public void UnInitData()
    {
    }


    #region 消息处理

    //命令附加数据列表解析
    void OnReveiveAppendMsg(int msgId, pb.Collections.RepeatedField<AppendItem> appendItems)
    {
        if (appendItems != null && appendItems.Count > 0)
        {
        }
    }

    private void OnUserHeart(S2C_User_Heart message)
    {
        if (message.HasCurTime)
        {
            long curTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }

        NetMgr.Instance.ClearSendTimes();
    }

    private void OnUserLogin(S2C_User_Login message)
    {
        gameOver = 1;
        if (message.HasRoomId) roomId = message.RoomId;
        if (message.HasFirstGiftCount) firstGiftCount = message.FirstGiftCount;
        if (message.HasGameOver) gameOver = message.GameOver;
        if (message.HasRoundNum) roundNum = message.RoundNum;


        Debug.Log(
            $"Login success, RoomId: {roomId}, firstGiftCount: {firstGiftCount}, gameOver: {gameOver}, roundNum： {roundNum} ");

        EventMgr.Instance.InvokeEvent(EventConst.UserLogin);
        MsgSend.SendGameStart(0);
    }

    private void OnGameStart(S2C_Game_Start message)
    {
        Debug.Log($"Game start, roundNum： {message.RoundNum} ");
        EventMgr.Instance.InvokeEvent(EventConst.GameStart, message.RoundNum);
       
    }


    private void OnGameRecord(S2C_Game_Record message)
    {
        Debug.Log($"Game start, remain score: {message.ScorePool} ");

        // Loom.QueueOnMainThread(() =>
        // {
        //     Messenger.Broadcast(EventConst.GAME_RECORD, message.ScorePool, message.ScorePoolShow,
        //         message.PointPoolShow);
        // });
    }

    private void OnGameRankQuery(S2C_Game_Rank_Query message)
    {
        //Loom.QueueOnMainThread(() => { Messenger.Broadcast(EventConst.GAME_RANK, message); });
    }

    public void OnGamePlayerMsgBro(S2C_Game_PayloadBro message)
    {
        // if (message.Player != null)
        // {
        //     bool updateRank = false;
        //
        //     UserDataInfo info;
        //     foreach (var dto in message.Player)
        //     {
        //         if (dto.User != null)
        //         {
        //             if (dto.Type == 0)
        //             {
        //                 PushMessageData msgSetting = PushMsgSetting.GetByKey(dto.Content);
        //                 //新增用户
        //                 info = UserDataDict.AddOrUpdate(dto.User);
        //                 UserDataDict[dto.User.Id].costPoint = dto.CostPoint;
        //                 //Debug.Log(dto.User.Nickname + "-----------------" + dto.User.WinPoint);
        //                 info.AddPrice(dto.GiftExp);
        //                 if (info != null)
        //                 {
        //                     GameLog.LogFormat("DataInfo new user, user: {0}, content: {1}", dto.User.Id, dto.Content);
        //                     info.autoSoldierTime = 0;
        //                     foreach (var rank in MonthRankDict.Values)
        //                     {
        //                         if (rank.user != null && rank.user.id == info.id)
        //                         {
        //                             info.SetMonthRank(rank.rankingMonth);
        //                             break;
        //                         }
        //                     }
        //                 }
        //
        //                 if (msgSetting.value.Equals("left"))
        //                 {
        //                     info.SetCampType(CampType.Atk);
        //                     updateRank = true;
        //                     //出兵
        //                     Loom.QueueOnMainThread(() =>
        //                     {
        //                         Messenger.Broadcast(EventConst.GAME_CHOOSE_CAMP, dto.User.Id);
        //                         GameRootManager.Instance.gameAtkCamp.atkPointPool +=
        //                             (long)Math.Ceiling(dto.User.WinPoint * 0.3f);
        //                     });
        //                 }
        //                 else if (msgSetting.value.Equals("right"))
        //                 {
        //                     info.SetCampType(CampType.Def);
        //                     updateRank = true;
        //                     //出兵
        //                     Loom.QueueOnMainThread(() =>
        //                     {
        //                         Messenger.Broadcast(EventConst.GAME_CHOOSE_CAMP, dto.User.Id);
        //                         GameRootManager.Instance.gameDefCamp.defPointPool +=
        //                             (long)Math.Ceiling(dto.User.WinPoint * 0.3f);
        //                     });
        //                 }
        //             }
        //             //评论
        //             else if (dto.Type == 1)
        //             {
        //                 GameLog.LogFormat("DataInfo comment, user: {0}, content: {1}", dto.User.Id, dto.Content);
        //                 if (!string.IsNullOrEmpty(dto.Content) &&
        //                     dto.Content.Equals("525557a0f07f4944bd78f7d85244c129"))
        //                 {
        //                     Loom.QueueOnMainThread(() => { Messenger.Broadcast(EventConst.GAME_COUNTDOWN_OVER); });
        //                     return;
        //                 }
        //
        //                 PushMessageData msgSetting = PushMsgSetting.GetByKey(dto.Content);
        //                 //生成
        //                 if (msgSetting.useType == PushUseType.GenerateSoldier)
        //                 {
        //                     Loom.QueueOnMainThread(() =>
        //                     {
        //                         Messenger.Broadcast(EventConst.GAME_PUSH_COMMENT, dto.User.Id,
        //                             int.Parse(msgSetting.value));
        //                     });
        //                 }
        //             }
        //             //礼物
        //             else if (dto.Type == 2)
        //             {
        //                 GameLog.LogFormat("DataInfo gift, user: {0}, giftId: {1}", dto.User.Id, dto.GiftId);
        //                 PushGiftData giftSetting = PushGiftSetting.GetByKey(dto.GiftId);
        //                 if (giftSetting != null)
        //                 {
        //                     info = UserDataDict.GetById(dto.User.Id);
        //                     //出超级兵
        //                     Loom.QueueOnMainThread((Action)(() =>
        //                     {
        //                         Messenger.Broadcast(EventConst.GAME_PUSH_GIFT, dto.User.Id, giftSetting.soldierId,
        //                             dto.Count, giftSetting.price, giftSetting.count);
        //
        //                         if (info != null && dto.FirstGift == false)
        //                         {
        //                             int oldCount = info.GetGiftSoldier(giftSetting.soldierId);
        //                             int oldLevel = GameBasicSetting.Instance.GetLevelByCount(oldCount);
        //                             int newLevel = GameBasicSetting.Instance.GetLevelByCount(oldCount + dto.Count);
        //                             Messenger.Broadcast(EventConst.GAME_PUSH_GIFT_SHOW, dto.User.Id, giftSetting,
        //                                 dto.Count, info.curprice, dto.FirstGift);
        //                         }
        //                     }));
        //
        //                     if (info != null && dto.FirstGift == true)
        //                     {
        //                         int id = 10;
        //                         if (info.campType == (int)CampType.Atk)
        //                         {
        //                             id = Random.Range(0, 100);
        //                             if (id < 60)
        //                             {
        //                                 id = 10;
        //                             }
        //                             else if (id < 90)
        //                             {
        //                                 id = 25;
        //                             }
        //                             else
        //                             {
        //                                 id = 11;
        //                             }
        //                         }
        //                         else if (info.campType == (int)CampType.Def)
        //                         {
        //                             id = Random.Range(0, 100);
        //                             if (id < 60)
        //                             {
        //                                 id = 10;
        //                             }
        //                             else if (id < 95)
        //                             {
        //                                 id = 11;
        //                             }
        //                             else
        //                             {
        //                                 id = 23;
        //                             }
        //                         }
        //
        //                         PushGiftData giftSetting2 = PushGiftSetting.GetBySoldierId(id);
        //
        //                         Messenger.Broadcast(EventConst.GAME_PUSH_FIRST_GIFT, dto.User.Id, giftSetting, id,
        //                             giftSetting2, dto.Count, info.curprice);
        //                     }
        //                 }
        //             }
        //             //点赞
        //             else if (dto.Type == 3)
        //             {
        //                 //出兵
        //                 Loom.QueueOnMainThread(() =>
        //                 {
        //                     Messenger.Broadcast(EventConst.GAME_PUSH_LIKE, dto.User.Id, dto.Count);
        //                 });
        //             }
        //         }
        //     }
        //
        //     //更新座位排名
        //     if (updateRank)
        //     {
        //         Loom.QueueOnMainThread(() => { Messenger.Broadcast(EventConst.DO_REFRESH_RANK_USER, true); });
        //     }
        // }
    }

    #endregion 消息处理

    #region 输出日志

    private void LogAppendData<T>(int id, T obj)
    {
#if UNITY_EDITOR
        if (obj != null)
        {
            Debug.Log(string.Format("AppendItem Id: {0}\n{1}", id, obj.ToString()));
        }
#endif
    }

    #endregion 输出日志
}