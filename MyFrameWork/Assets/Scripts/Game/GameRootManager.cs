using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using XOProto;
using Random = UnityEngine.Random;

public class GameRootManager : MonoSingleton<GameRootManager>
{
    public PushMessageData pushMsgSetting;
    public PushGiftData pushGiftSetting;

    [Header("数据")] public bool isPlay;
    public int countdown;
    public long totalScore;
    public long leftScore;
    public long rightScore;
    public int continueType;
    public int continueCount;
    public int leftWinCount;
    public int rightWinCount;
    public int leftGiftPrice;
    public int rightGiftPrice;
    public string token = "";
    public int countdown_2 = 300;

    [Header("缓存数据")] public int saveInterval = 5;
    public string roundNum;
    public CacheData cacheData;

    [Header("攻击倍数")] public float damageScale = 1;
    public int leftUserCount;
    public int rightUserCount;

    private int frameIndex;
    private float autoSoldierTime;
    public float leftAutoSoldierCount;
    public float rightAutoSoldierCount;

    private Dictionary<MarqueeType, bool> tipMarquee = new Dictionary<MarqueeType, bool>();

    public int autoGroupId = 0;

    public int damage = 4;

    public bool isFirstLogin = true;

    public GameAtkCamp gameAtkCamp;
    public GameDefCamp gameDefCamp;

    public int NEED_MIN_JIFEN;
    public int GIFT_JIFEN;
    public int DAMAGE_JIFEN;
    public int GIFT_JIFEN_POOL;
    public int DAMAGE_JIFEN_POOL;

    //进入暖场环节
    public bool IsEnterReady = false;
    public Transform GameObjectPar;

    bool isJingBao = true;
    bool isJingBao2 = true;

    public void InitScene()
    {
        LoaderMgr.Instance.InstantiatePrefabAsync("Prefabs/Game/GameAtkCamp", Vector3.zero, Quaternion.identity,
            transform, (gameObject) =>
            {
                gameObject.name = "GameAtkCamp";
                gameObject.transform.localPosition = new Vector3(0, 0, 38);
                gameAtkCamp = gameObject.GetComponent<GameAtkCamp>();
                gameAtkCamp.Init(0);
            });


        LoaderMgr.Instance.InstantiatePrefabAsync("Prefabs/Game/GameDefCamp", Vector3.zero, Quaternion.identity,
            transform, (gameObject) =>
            {
                gameObject.name = "GameDefCamp";
                gameObject.transform.localPosition = new Vector3(0, 0, -38);
                gameDefCamp = gameObject.GetComponent<GameDefCamp>();
                gameDefCamp.Init(1);
            });


        if (DataInfoMgr.Instance.gameOver == 0)
        {
            LocalToCache();
            if (IsUseCache())
            {
                Recovery();
                // CoroutineManager:GetInstance():StartNextTime(function()
                // GameMapManager:GetInstance():OnKeyDown(KeyId.CameraZoom, 3);
                // end, 0.2)
            }
        }

        InitCamera();
        PanelMgr.Instance.OpenPanel<PanelMain>();


        // gameBasicSetting.Startup();
        // pushMsgSetting.Startup();
        // pushGiftSetting.Startup();
        //
        //
        // NEED_MIN_JIFEN = int.Parse(ConfigurationConfigHelper.GetValueById("NEED_MIN_JIFEN"));
        // GIFT_JIFEN = int.Parse(ConfigurationConfigHelper.GetValueById("GIFT_JIFEN"));
        // DAMAGE_JIFEN = int.Parse(ConfigurationConfigHelper.GetValueById("DAMAGE_JIFEN"));
        // GIFT_JIFEN_POOL = int.Parse(ConfigurationConfigHelper.GetValueById("GIFT_JIFEN_POOL"));
        // DAMAGE_JIFEN_POOL = int.Parse(ConfigurationConfigHelper.GetValueById("DAMAGE_JIFEN_POOL"));

        //long.TryParse(UPlayerPrefs.GetString("scoreTotal"), out totalScore);
        InitStartupArgs();
        InitEvent();
    }

    public void InitCamera()
    {
        LoaderMgr.Instance.InstantiatePrefabAsync("Prefabs/Game/GameMap", Vector3.zero, Quaternion.identity,
            transform, (gameObject) => { });
    }


    private void InitEvent()
    {
        EventMgr.Instance.AddEventListener<string>(EventConst.GameStart, OnGameStart);
        EventMgr.Instance.AddEventListener(EventConst.GameCountdownOver, OnGameCountdownOver);
        EventMgr.Instance.AddEventListener<CampType>(EventConst.GameOver, OnGameOver);
        EventMgr.Instance.AddEventListener<long, long, long>(EventConst.GameRecord, OnGameRecord);
        EventMgr.Instance.AddEventListener<int>(EventConst.GameChooseCamp, OnGameChooseCamp);
        EventMgr.Instance.AddEventListener<int, int>(EventConst.GamePushComment, OnGamePushComment);
        EventMgr.Instance.AddEventListener<int, int>(EventConst.GamePushLike, OnGamePushLikeSoldier);
        EventMgr.Instance.AddEventListener<int, int, int, int, int>(EventConst.GamePushGift, OnGamePushGiftSoldier);
        EventMgr.Instance.AddEventListener<int, PushGiftData, int, PushGiftData, int, int>(EventConst.GamePushFirstGift,
            OnGameFirstGiftSoldier);
        EventMgr.Instance.AddEventListener<bool>(EventConst.DoRefreshRankUser, RefreshRankUser);
    }

    private void InitStartupArgs()
    {
        string[] args = Environment.GetCommandLineArgs();
        Debug.Log("Parse token start");
        foreach (string arg in args)
        {
            if (arg.StartsWith("-token="))
            {
                token = arg.Replace("-token=", string.Empty);
                Debug.Log(token);
                break;
            }
        }

        Debug.Log("Parse token end");
    }

    public void Init(int time)
    {
        isPlay = true;
        countdown = time;
        leftScore = 0;
        rightScore = 0;
        leftUserCount = 0;
        rightUserCount = 0;
        RefreshScore();
        InvokeRepeating(nameof(RefreshTime), 0, 1);
        //InvokeRepeating(nameof(CacheToLocal), 0, saveInterval);
    }

    public void Replay()
    {
        isPlay = true;
        RefreshScore();
        RefreshRankUser();
        InvokeRepeating(nameof(RefreshTime), 0, 1);
        //InvokeRepeating(nameof(CacheToLocal), 0, saveInterval);
    }

    public void Stop()
    {
        isPlay = false;
        CancelInvoke(nameof(RefreshTime));
        CancelInvoke(nameof(RefreshTime2));
        //CancelInvoke(nameof(CacheToLocal));
        ClearLocal(true);
    }

    public void Reset()
    {
        leftWinCount = 0;
        rightWinCount = 0;
        isJingBao = true;
        isJingBao2 = true;
        isPlay = false;
        IsEnterReady = false;
        leftUserCount = 0;
        rightUserCount = 0;
        autoGroupId = 0;
        gameAtkCamp.Reset();
        gameDefCamp.Reset();
        GameMap.Instance.Reset();
        ClearOtherGameobject();
        countdown_2 = 300;
        foreach (UserDataInfo userDataInfo in DataInfoMgr.Instance.UserDataDict.Values)
        {
            userDataInfo.Reset();
        }

        tipMarquee.Clear();
        RefreshRankUser();
        RefreshWinCount();
    }

    public void ClearOtherGameobject()
    {
        // foreach (Transform child in GameObjectPar)
        // {
        //     Destroy(child.gameObject);
        // }
    }

    public void UnInit()
    {
        CancelInvoke(nameof(RefreshTime));
        CancelInvoke(nameof(RefreshTime2));
        //CancelInvoke(nameof(CacheToLocal));
        isPlay = false;
        gameAtkCamp.AtkItemList.Clear();
        gameDefCamp.DefItemList.Clear();
    }


    private void Update()
    {
    }


    public void RemoveFromAtkItemList(BaseAtkItem item)
    {
        gameAtkCamp.RemoveSoldier(item);
    }

    public void RemoveFromDaShenShiList(DaFangKuaiItem item)
    {
        gameAtkCamp.RemoveDaShenShi(item);
    }

    public void RemoveFromMoGuanList(MoGuanItem item)
    {
        gameAtkCamp.RemoveMoGuan(item);
    }

    public void RemoveFromDefItemList(GameObject item)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="campType"></param>
    /// <param name="value"></param>
    /// <param name="type">类型</param>
    public void AddScore(CampType campType, int value, int type = 0)
    {
        int multiplier = 1;

        if (type == 0)
        {
            //送礼产生的
            multiplier = GIFT_JIFEN_POOL;
        }
        else if (type == 1)
        {
            //伤害产生的
            multiplier = DAMAGE_JIFEN_POOL;
        }

        totalScore += value * multiplier;
        if (campType == CampType.Atk)
        {
            rightScore += value * multiplier;
        }
        else
        {
            leftScore += value * multiplier;
        }

        RefreshScore();
    }

    public void RefreshTime3()
    {
        InvokeRepeating(nameof(RefreshTime2), 0, 1);
    }

    public void CloseRefreshTime()
    {
        CancelInvoke(nameof(RefreshTime2));
    }

    private void RefreshScore()
    {
        PanelMgr.Instance.GetPanel<PanelMain>().OnUpdateScorePool(totalScore);
    }


    private void RefreshTime()
    {
        if (countdown > 0)
        {
            countdown--;
            if (countdown <= 300)
            {
                DoTipMarquee(MarqueeType.TipCountdown5);
                if (isJingBao)
                {
                    AudioMgr.Instance.PlayEffect(MyAudioName.gs_jingbao, 0.3f);
                    isJingBao = false;
                }
            }
            else if (countdown <= 600)
            {
                DoTipMarquee(MarqueeType.TipKingDodgeLeft1);
                if (isJingBao2)
                {
                    AudioMgr.Instance.PlayEffect(MyAudioName.gs_jingbao, 0.3f);
                    isJingBao2 = false;
                }
            }
            else
            {
                damageScale = 1;
            }

            if (countdown <= 0)
            {
                OnGameOver(CampType.Atk);
            }

            RefreshRankUser(true);
        }

        EventMgr.Instance.InvokeEvent(EventConst.GameUpdateCountdown, countdown);
    }

    private void RefreshTime2()
    {
        if (countdown_2 > 0)
        {
            countdown_2--;

            if (countdown_2 <= 0)
            {
                EventMgr.Instance.InvokeEvent(EventConst.GameNuanchangOver);
                PanelMgr.Instance.GetPanel<PanelMain>().btnStartOnClick();

                CancelInvoke(nameof(RefreshTime2));
            }
        }

        EventMgr.Instance.InvokeEvent(EventConst.GameUpdateCountdown2, countdown_2);
    }

    private void RefreshWinCount()
    {
        PanelMgr.Instance.GetPanel<PanelMain>().OnUpdateWinCount(leftWinCount, rightWinCount);
    }

    private void RefreshRankUser(bool isAnim = false)
    {
        var users = DataInfoMgr.Instance.UserDataDict.Values.ToList();
        int campLeft = (int)CampType.Atk;
        int campRight = (int)CampType.Def;
        var leftUsers = users.FindAll(p => p.campType == campLeft).OrderByDescending(p => p.score).ToList();
        var rightUsers = users.FindAll(p => p.campType == campRight).OrderByDescending(p => p.score).ToList();
        GameMap.Instance.SetTopThreeInfo(rightUsers);
        PanelMgr.Instance.GetPanel<PanelMain>().OnGameUpdateRankUser(leftUsers, rightUsers, isAnim);
    }

    private void OnGameStart(string roundNum)
    {
        roundNum = roundNum;
        IsEnterReady = true;
        PanelMgr.Instance.GetPanel<PanelMain>().OnGameStart(roundNum);
    }

    private void OnGameRecord(long scoreTotal, long scorePoolShow, long pointPoolShow)
    {
        PanelMgr.Instance.GetPanel<PanelMain>().OnGameRecordRsp(scoreTotal, scorePoolShow, pointPoolShow);
        totalScore = scoreTotal;
        PlayerPrefs.SetString("scoreTotal", scoreTotal.ToString());
        RefreshScore();
    }

    public void OnGameCountdownOver()
    {
        Debug.Log("Game countdown over");
        EventMgr.Instance.InvokeEvent(EventConst.GameOver, CampType.Def);
    }

    public void OnGameOver(CampType loserCampType)
    {
        isPlay = false;
        Debug.LogFormat("Gameover, loser: {0}", loserCampType);

        CampType winnerType = loserCampType == CampType.Atk ? CampType.Def : CampType.Atk;
        long remainScore = 0;
        //MsgSend.SendGameRecord(totalScore, remainScore, GameBasicSetting.Instance.scoreThrethold, winnerType);

        RefreshWinCount();
        continueType = loserCampType == CampType.Atk ? 2 : 1;
        PanelMgr.Instance.GetPanel<PanelMain>().OnGameContinueWin(loserCampType, continueCount);
        Stop();
    }

    private void OnGameCrazy(params object[] arg)
    {
        // if (!isPlay) return;
        // Debug.LogFormat("Game crazy, user: {0}, time: {1}", userId, time);
        // CacheToLocal();
    }


    /// <summary>
    /// 加入阵营
    /// </summary>
    /// <param name="userId"></param>
    private void OnGameChooseCamp(int userId)
    {
        PanelMgr.Instance.GetPanel<PanelMain>().OnChooseCamp(userId);

        if (IsEnterReady == false && !isPlay) return;
        UserDataInfo info = DataInfoMgr.Instance.UserDataDict.GetById(userId);

        if (info.campType == (int)CampType.Atk)
        {
            leftUserCount++;
            leftWinCount += info.costPoint;
            gameAtkCamp.AddNewSoldier(info);
            //Debug.Log(leftWinCount + "上方胜点池" + "------------" + info.costPoint + "玩家投入胜点");
        }
        else if (info.campType == (int)CampType.Def)
        {
            rightUserCount++;
            rightWinCount += info.costPoint;
            gameDefCamp.GenerateSoldierById(info);
            //Debug.Log(leftWinCount + "下方胜点池" + "------------" + info.costPoint + "玩家投入胜点");
        }

        RefreshWinCount();
        CacheToLocal();
    }


    /// <summary>
    /// 评论出兵/加子弹
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="num"></param>
    private void OnGamePushComment(int userId, int num)
    {
        if (IsEnterReady == false && !isPlay) return;
        UserDataInfo info = DataInfoMgr.Instance.UserDataDict.GetById(userId);
        //评论666
        if (num == 6)
        {
            int solidernum = 2;
            int bulluetnum = 30;
            if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 400)
            {
                solidernum = 6;
            }
            else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 800)
            {
                solidernum = 3;
            }
            else
            {
                solidernum = 0;
            }

            DoPushCommentSoldier(info, bulluetnum, solidernum);
        }
    }


    private void DoPushCommentSoldier(UserDataInfo info, int bulletnum = 30, int soldiernum = 6)
    {
        if (info.campType == (int)CampType.Atk)
        {
            foreach (var variableAtkItem in gameAtkCamp.AtkPlayerList)
            {
                if (variableAtkItem.info.id == info.id)
                {
                    //生成玩家对应的方块
                    for (int i = 0; i < soldiernum; i++)
                    {
                        gameAtkCamp.GenerateSoldierByPlayer(variableAtkItem, 0, 1);
                    }
                }
            }
        }
        else
        {
            foreach (var variableDefItem in gameDefCamp.DefItemList)
            {
                if (variableDefItem.Info.id == info.id)
                {
                    variableDefItem.AddBullet(bulletnum);
                }
            }
        }
    }

    /// <summary>
    /// 点赞处理
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="likeCount"></param>
    private void OnGamePushLikeSoldier(int userId, int likeCount)
    {
        if (IsEnterReady == false && !isPlay) return;
        UserDataInfo info = DataInfoMgr.Instance.UserDataDict.GetById(userId);
        info.AddLike(likeCount);
        int solidernum = 2;
        int bulluetnum = 2;
        if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 400)
        {
            solidernum = 2;
        }
        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 800)
        {
            solidernum = 1;
        }
        else
        {
            solidernum = 0;
        }

        DoPushLikeSoldier(info, bulluetnum, solidernum, likeCount);
    }

    /// <summary>
    /// 不同阵营评论效果
    /// </summary>
    /// <param name="info"></param>
    /// <param name="num"></param>
    private void DoPushLikeSoldier(UserDataInfo info, int bulletnum = 8, int soldiernum = 2, int likeCount = 1)
    {
        if (info.campType == (int)CampType.Atk)
        {
            int lastnum = soldiernum * likeCount;

            if (lastnum > 9)
            {
                lastnum = 9;
            }

            if (GameRootManager.Instance.gameAtkCamp.CurActiveNum + lastnum > 220)
            {
                lastnum = 0;
            }
            else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum + lastnum > 150)
            {
                lastnum = 3;
            }
            else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum + lastnum > 100)
            {
                lastnum = 6;
            }

            if (lastnum > 0)
            {
                foreach (var variableAtkItem in gameAtkCamp.AtkPlayerList)
                {
                    if (variableAtkItem.info.id == info.id)
                    {
                        //生成玩家对应的方块
                        for (int i = 0; i < lastnum; i++)
                        {
                            gameAtkCamp.GenerateSoldierByPlayer(variableAtkItem, 0, 1);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (var variableDefItem in gameDefCamp.DefItemList)
            {
                if (variableDefItem.Info.id == info.id)
                {
                    variableDefItem.AddBullet(bulletnum * likeCount);
                }
            }
        }
    }

    /// <summary>
    /// 送礼效果
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="superSoldierId"></param>
    /// <param name="giftCount"></param>
    /// <param name="price"></param>
    /// <param name="soldierCount"></param>
    private void OnGamePushGiftSoldier(int userId, int superSoldierId, int giftCount, int price, int soldierCount)
    {
        if (IsEnterReady == false && !isPlay) return;
        UserDataInfo info = DataInfoMgr.Instance.UserDataDict.GetById(userId);
        AddScore((CampType)info.campType, price * giftCount);
        info.AddPrice(price * giftCount);
        info.AddScore(price * giftCount);
        if (info.campType == (int)CampType.Atk)
        {
            StartCoroutine(gameAtkCamp.GetGiftInfo(info, superSoldierId, giftCount));
        }
        else if (info.campType == (int)CampType.Def)
        {
            StartCoroutine(gameDefCamp.GetGiftInfo(info, superSoldierId, giftCount));
        }
    }


    private void OnGameFirstGiftSoldier(int userId, PushGiftData giftData1, int firstGiftID, PushGiftData giftData,
        int count, int price)
    {
        if (IsEnterReady == false && !isPlay) return;
        UserDataInfo info = DataInfoMgr.Instance.UserDataDict.GetById(userId);
        if (info.campType == (int)CampType.Atk)
        {
            StartCoroutine(gameAtkCamp.GetGiftInfo(info, firstGiftID, 1));
        }
        else if (info.campType == (int)CampType.Def)
        {
            StartCoroutine(gameDefCamp.GetGiftInfo(info, firstGiftID, 1));
        }

        PanelMgr.Instance.GetPanel<PanelMain>()
            .OnGamePushFirstGift(userId, giftData1, firstGiftID, giftData, count, price);
    }


    public void DoTipMarquee(MarqueeType marqueeType)
    {
        if (!tipMarquee.ContainsKey(marqueeType) || !tipMarquee[marqueeType])
        {
            tipMarquee[marqueeType] = true;
            PanelMgr.Instance.GetPanel<PanelMain>().OnGameTipMarquee((int)marqueeType);
        }
    }

    public void ClearTipMarquee(MarqueeType marqueeType)
    {
        tipMarquee.Remove(marqueeType);
    }


    #region 本地缓存

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("保存数据")]
#endif
    public void CacheToLocal()
    {
        Debug.Log("Game cache to local");
        cacheData = new CacheData();
        cacheData.roundNum = roundNum;
        cacheData.countdown = countdown;
        cacheData.totalScore = totalScore;
        cacheData.leftScore = leftScore;
        cacheData.rightScore = rightScore;
        cacheData.continueType = continueType;
        cacheData.continueCount = continueCount;
        cacheData.leftWinCount = leftWinCount;
        cacheData.rightWinCount = rightWinCount;
        cacheData.leftGiftPrice = leftGiftPrice;
        cacheData.rightGiftPrice = rightGiftPrice;
        cacheData.maplength = GameMap.Instance.maxlengh;
        cacheData.atkitemnum = gameAtkCamp.CurActiveNum;
        cacheData.isEnable = true;

        cacheData.userList = new List<UserData>();
        if (DataInfoMgr.Instance.UserDataDict != null)
        {
            foreach (var userDataInfo in DataInfoMgr.Instance.UserDataDict.Values)
            {
                cacheData.userList.Add(new UserData(userDataInfo));
            }
        }

        // cacheData.soldierList = new List<SoldierData>();
        //aliveCount = 0;
        // foreach (GameCamp gameCamp in dictCamp.Values)
        // {
        //     foreach (GameSoldier soldier in gameCamp.soldiers)
        //     {
        //         if (soldier.IsAlive())
        //         {
        //             aliveCount++;
        //             cacheData.soldierList.Add(new SoldierData(soldier));
        //         }
        //     }
        // }
        // attackIntervalScale = GameBasicSetting.Instance.GetAttackInterval(aliveCount);
        // soldierRadiusScale = GameBasicSetting.Instance.GetSoldierRadiusScale(aliveCount);
        // checkViewRangeInterval = GameBasicSetting.Instance.GetCheckViewRangeInterval(aliveCount);
        // checkAttackRangeInterval = GameBasicSetting.Instance.GetCheckAttackRangeInterval(aliveCount);

        // cacheData.pushSoldiers = new List<List<PushSoldierData>>();
        // foreach (var soldierList in pushSoldiers.Values)
        // {
        //     var list = new List<PushSoldierData>();
        //     foreach (PushSoldier soldier in soldierList)
        //     {
        //         list.Add(new PushSoldierData(soldier));
        //     }
        //     cacheData.pushSoldiers.Add(list);
        // }
        //
        // cacheData.pushSuperSoldiers = new List<List<PushSoldierData>>();
        // foreach (var soldierList in pushSuperSoldiers.Values)
        // {
        //     var list = new List<PushSoldierData>();
        //     foreach (PushSoldier soldier in soldierList)
        //     {
        //         list.Add(new PushSoldierData(soldier));
        //     }
        //     cacheData.pushSuperSoldiers.Add(list);
        // }

        // Loom.RunAsync(() =>
        // {
        //     byte[] bytes = SerializeUtils.SerializeBinary(cacheData);
        //     if (bytes != null)
        //     {
        //         string file = Path.Combine(FilePathMgr.persistentDataPath, "save.dat");
        //         File.WriteAllBytes(file, bytes);
        //     }
        // });
    }

    public void LocalToCache()
    {
        Debug.Log("Came local to cache");
        string file = Path.Combine(FilePathMgr.persistentDataPath, "save.dat");
        if (File.Exists(file))
        {
            cacheData = (CacheData)SerializeUtils.DeSerializeBinaryFile(file);
            if (cacheData != null && !string.IsNullOrEmpty(cacheData.roundNum) &&
                cacheData.roundNum.Equals(DataInfoMgr.Instance.roundNum))
            {
                Debug.Log("Came local cache is enabled");
                cacheData.isEnable = true;
            }
            else
            {
                if (cacheData != null)
                {
                    Debug.LogFormat("Came local cache, invalid round: \n{0}\n{1}", cacheData.roundNum,
                        DataInfoMgr.Instance.roundNum);
                }

                ClearLocal();
            }
        }
        else
        {
            Debug.Log("Came local cache not exists");
            ClearLocal();
        }
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("恢复数据")]
#endif
    public void Recovery()
    {
        Debug.Log("Came cache recovery");
        roundNum = cacheData.roundNum;
        countdown = cacheData.countdown;
        totalScore = cacheData.totalScore;
        leftScore = cacheData.leftScore;
        rightScore = cacheData.rightScore;
        continueType = cacheData.continueType;
        continueCount = cacheData.continueCount;
        leftWinCount = cacheData.leftWinCount;
        rightWinCount = cacheData.rightWinCount;
        leftGiftPrice = cacheData.leftGiftPrice;
        rightGiftPrice = cacheData.rightGiftPrice;

        if (DataInfoMgr.Instance.UserDataDict != null)
        {
            DataInfoMgr.Instance.UserDataDict.Clear();
            leftUserCount = 0;
            rightUserCount = 0;
            foreach (UserData userData in cacheData.userList)
            {
                var userDataInfo = new UserDataInfo(userData);
                DataInfoMgr.Instance.UserDataDict.Add(userData.id, userDataInfo);

                if (userDataInfo.campType == (int)CampType.Atk)
                {
                    leftScore += userDataInfo.score;
                    leftUserCount++;
                }
                else if (userDataInfo.campType == (int)CampType.Def)
                {
                    rightScore += userDataInfo.score;
                    rightUserCount++;
                }

                totalScore += userDataInfo.score;
            }
        }


        StartCoroutine(RecoverSoldier());
    }

    private IEnumerator RecoverSoldier()
    {
        // int total = 100;
        // if (cacheData != null && cacheData.soldierList != null)
        // {
        //     total = cacheData.soldierList.Count;
        //     Messenger.Broadcast(EventConst.UPDATE_PROGRESS_EVENT, 0.99, Math.Clamp(cacheData.soldierList.Count * 0.002f, 0.5f, 30));
        // }
        // List<int> groupIds = new List<int>();
        // int index = 0;
        // foreach (var soldier in cacheData.soldierList)
        // {
        //     if (soldier.campType == (int)CampType.Left)
        //     {
        //         leftCampList[soldier.campIndex - 1].GenerateSoldierById(soldier.soldierId, soldier.level, 1, soldier.userId, soldier.groupId, soldier.groupId == 0 || !groupIds.Contains(soldier.groupId), (gameSoldier) =>
        //         {
        //             gameSoldier.Recover(soldier);
        //         }, true);
        //     }
        //     else
        //     {
        //         rightCampList[soldier.campIndex - 1].GenerateSoldierById(soldier.soldierId, soldier.level, 1, soldier.userId, soldier.groupId, soldier.groupId == 0 || !groupIds.Contains(soldier.groupId), (gameSoldier) =>
        //         {
        //             gameSoldier.Recover(soldier);
        //         }, true);
        //     }
        //
        //     if (!groupIds.Contains(soldier.groupId))
        //     {
        //         autoGroupId = Math.Max(autoGroupId, soldier.groupId);
        //         groupIds.Add(soldier.groupId);
        //     }
        //     index++;
        //     if (index % 10 == 0)
        //     {
        //         Messenger.Broadcast(EventConst.UPDATE_PROGRESS_EVENT, 1.0f * index / total - 0.01f);
        //         yield return new WaitForEndOfFrame();
        //     }
        // }

        // for (int i = 0; i < 6; i++)
        // {
        //     var source = cacheData.pushSoldiers[i];
        //     var dest = pushSoldiers[i + 1];
        //     foreach (PushSoldierData soldier in source)
        //     {
        //         dest.Add(new PushSoldier(soldier));
        //     }
        // }
        //
        // for (int i = 0; i < 6; i++)
        // {
        //     var source = cacheData.pushSuperSoldiers[i];
        //     var dest = pushSuperSoldiers[i + 1];
        //     foreach (PushSoldierData soldier in source)
        //     {
        //         dest.Add(new PushSoldier(soldier));
        //     }
        // }
        // CheckIgnoreCollide();
        //
        // Messenger.Broadcast(EventConst.GENERATE_SUPER_SOLDIER, (int)CampType.Left);
        // Messenger.Broadcast(EventConst.GENERATE_SUPER_SOLDIER, (int)CampType.Right);
        // autoGroupId++;
        // Messenger.Broadcast(EventConst.FINISH_PROGRESS_EVENT, 0.2f, true);
        //
        // if (pushSoldiers.Count > 0 && !isGenSoldier)
        // {
        //     isGenSoldier = true;
        //     StartCoroutine(OnGenerateSoldier());
        // }
        // if (pushSuperSoldiers.Count > 0 && !isGenSuperSoldier)
        // {
        //     isGenSuperSoldier = true;
        //     StartCoroutine(OnGenerateSuperSoldier());
        // }
        yield return new WaitForSeconds(0.2f);

        EventMgr.Instance.InvokeEvent(EventConst.GameStart, roundNum);
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("清除数据")]
#endif
    public void ClearLocal(bool isSave = false)
    {
        string file = Path.Combine(FilePathMgr.persistentDataPath, "save.dat");
        if (File.Exists(file))
        {
            if (isSave)
            {
                string backFolder = Path.Combine(FilePathMgr.persistentDataPath, "History");
                if (!Directory.Exists(backFolder)) Directory.CreateDirectory(backFolder);
                string backFile = Path.Combine(backFolder,
                    "save_" + Guid.NewGuid().ToString().Replace("-", string.Empty) + ".dat");
                Debug.Log("Came record history " + backFile);
                File.Copy(file, backFile);
            }

            Debug.Log("Came cache clear");
            File.Delete(file);
        }

        if (cacheData != null)
        {
            cacheData.isEnable = false;
        }

        cacheData = null;
    }

    public bool HasLocalCache()
    {
        Debug.Log("Came check has local cache");
        bool hasLocalCache = false;
        string file = Path.Combine(FilePathMgr.persistentDataPath, "save.dat");
        if (File.Exists(file))
        {
            var tempCacheData = (CacheData)SerializeUtils.DeSerializeBinaryFile(file);
            if (tempCacheData != null && !string.IsNullOrEmpty(tempCacheData.roundNum) &&
                tempCacheData.roundNum.Equals(DataInfoMgr.Instance.roundNum))
            {
                hasLocalCache = true;
            }
            else
            {
                if (tempCacheData != null)
                {
                    Debug.LogFormat("Came local cache, invalid round: \n{0}\n{1}", tempCacheData.roundNum,
                        DataInfoMgr.Instance.roundNum);
                }

                ClearLocal();
            }
        }
        else
        {
            Debug.Log("Came local cache not exists");
            ClearLocal();
        }

        Debug.Log("Came has local cache " + hasLocalCache);
        return hasLocalCache;
    }

    public bool IsUseCache()
    {
        return cacheData != null && cacheData.isEnable;
    }

    #endregion 本地缓存
}

#region 本地缓存结构

[Serializable]
public class CacheData
{
    public bool isEnable;
    public string roundNum;
    public int countdown;
    public long totalScore;
    public long leftScore;
    public long rightScore;
    public int continueType;
    public int continueCount;
    public int leftWinCount;
    public int rightWinCount;
    public int leftGiftPrice;
    public int rightGiftPrice;
    public float maplength;
    public int atkitemnum;

    public List<UserData> userList;
    public List<AtkItem> atkItemList;
    public List<DefItem> defItemList;
}

[Serializable]
public struct UserData
{
    public int id; // ID
    public string openId; // 第三方唯一标识
    public string nickname; // 玩家昵称
    public string avatarUrl; // 用户头像
    public int titleLevel; // 称谓等级
    public int winPoint; // 胜点
    public int accrueWinPoint; //累计胜点
    public long monthRank;
    public int campType;
    public int winCount;
    public int score;
    public int continueKill;
    public int totalKill;
    public int giftCount;
    public int totalLikeCount;
    public int soldierLevel;
    public float autoSoldierTime;
    public int curprice;
    public int bulletCount;


    public List<SoldierTypeCount> dictGiftCount;
    public List<SoldierTypeCount> dictSuperCount;
    public List<GroupTypeCount> dictGroupCount;

    public UserData(UserDataInfo userData)
    {
        id = userData.id;
        openId = userData.openId;
        nickname = userData.nickname;
        avatarUrl = userData.avatarUrl;
        titleLevel = userData.titleLevel;
        winPoint = userData.winPoint;
        accrueWinPoint = userData.accrueWinPoint;
        monthRank = userData.monthRank;
        campType = userData.campType;
        winCount = userData.winCount;
        score = userData.score;
        continueKill = userData.continueKill;
        totalKill = userData.totalKill;
        giftCount = userData.giftCount;
        autoSoldierTime = userData.autoSoldierTime;
        totalLikeCount = userData.totalLikeCount;
        soldierLevel = userData.soldierLevel;
        curprice = userData.curprice;
        bulletCount = userData.bulletCount;

        dictGiftCount = new List<SoldierTypeCount>();
        dictSuperCount = new List<SoldierTypeCount>();
        dictGroupCount = new List<GroupTypeCount>();
        foreach (var kvp in userData.dictGiftCount)
        {
            dictGiftCount.Add(new SoldierTypeCount(kvp.Key, kvp.Value));
        }

        foreach (var kvp in userData.dictSuperCount)
        {
            dictSuperCount.Add(new SoldierTypeCount(kvp.Key, kvp.Value));
        }

        foreach (var kvp in userData.dictGroupCount)
        {
            dictGroupCount.Add(new GroupTypeCount(kvp.Key, kvp.Value));
        }
    }
}

// [Serializable]
// public struct SoldierData
// {
//     public int soldierId;       //兵种ID
//     public int userId;       //用户ID
//     public int groupId;       //组ID
//     public int level;
//     public float hp;
//     public int campType;
//     public int campIndex;
//     public float x;
//     public float z;
//     public float vx;
//     public float vz;
//     public int animationIndex;
//     public int direction;
//     public bool isCollide;
//     public bool isWall;
//     public bool isInWall;
//     public bool isAttack;
//
//
//     public SoldierData(GameSoldier soldier)
//     {
//         soldierId = soldier.soldierId;
//         userId = soldier.userId;
//         groupId = soldier.groupId;
//         level = soldier.level;
//         hp = soldier.hp;
//         campType = (int)soldier.GetCampType();
//         campIndex = (int)soldier.GetCampIndex();
//         x = soldier.transform.position.x;
//         z = soldier.transform.position.z;
//         vx = soldier.moveVector.x;
//         vz = soldier.moveVector.z;
//         animationIndex = soldier.animationIndex;
//         direction = soldier.direction;
//         isCollide = soldier.isCollide;
//         isWall = soldier.isWall;
//         isInWall = soldier.isInWall;
//         isAttack = soldier.isAttack;
//     }
// }
//
[Serializable]
public struct SoldierTypeCount
{
    public int soldierId;
    public int count;

    public SoldierTypeCount(int soldierId, int count)
    {
        this.soldierId = soldierId;
        this.count = count;
    }
}


[Serializable]
public struct GroupTypeCount
{
    public long groupId;
    public int count;

    public GroupTypeCount(long groupId, int count)
    {
        this.groupId = groupId;
        this.count = count;
    }
}

#endregion 本地缓存

public enum MarqueeType
{
    None = 0,
    TipKingHealthLeft1 = 1,
    TipKingHealthLeft2 = 2,
    TipKingHealthLeft3 = 3,
    TipKingHealthRight1 = 4,
    TipKingHealthRight2 = 5,
    TipKingHealthRight3 = 6,
    TipCampHealthLeftUp = 7,
    TipCampHealthLeftMid = 8,
    TipCampHealthLeftDown = 9,
    TipCampHealthRightUp = 10,
    TipCampHealthRightMid = 11,
    TipCampHealthRightDown = 12,
    TipCampRecoverLeftUp = 13,
    TipCampRecoverLeftMid = 14,
    TipCampRecoverLeftDown = 15,
    TipCampRecoverRightUp = 16,
    TipCampRecoverRightMid = 17,
    TipCampRecoverRightDown = 18,
    TipCountdown5 = 19,
    TipKingDodgeLeft1 = 20,
    TipKingDodgeLeft2 = 21,
    TipKingDodgeRight1 = 22,
    TipKingDodgeRight2 = 23,
}