using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class PanelMain : PanelBase {

    public Text txtFirstGiftCount;
    public Text txtFirstGiftCount1;
    public Text txtPool;
    public GameObject goTimeBG;
    public Text txtCountdown;
    public GameObject tipDownJoin;
    public GameObject tipUpJoin;
    public Text txtUpWinPoint;
    public Text txtDownWinPoint;
    public Slider sliderHP;
    public Text txtsliderHP;
    public GameObject goTipJoin1;
    public GameObject goTipJoin11;
    public GameObject LVup;
    public GameObject LVdown;
    public GameObject Kuangbao;
    public GameObject KeZhiGuanXi;
    public Button btnStart;
    public Text txttime3min;
    public Button btnRankWeek;
    public Button btnSetting;
    public GameObject goWarning;
    public Text txtWaring;
    public override void Init(params object[] _args)
    {
         args = _args;
         CurViewPath="MyUI/View/PanelMain";
         layer = PanelLayer.Normal;
    }
    public override void InitComponent()
    {
        txtFirstGiftCount = curView.transform.Find("goFirstCall/txtFirstGiftCount_Text").GetComponent<Text>();
        txtFirstGiftCount1 = curView.transform.Find("goFirstCall/txtFirstGiftCount1_Text").GetComponent<Text>();
        txtPool = curView.transform.Find("ScoreView/ScorePool/txtPool_Text").GetComponent<Text>();
        goTimeBG = curView.transform.Find("ScoreView/goTimeBG_GameObject").gameObject;
        txtCountdown = curView.transform.Find("ScoreView/goTimeBG_GameObject/txtCountdown_Text").GetComponent<Text>();
        tipDownJoin = curView.transform.Find("ScoreView/tipDownJoin_GameObject").gameObject;
        tipUpJoin = curView.transform.Find("ScoreView/tipUpJoin_GameObject").gameObject;
        txtUpWinPoint = curView.transform.Find("uplianshengbg/txtUpWinPoint_Text").GetComponent<Text>();
        txtDownWinPoint = curView.transform.Find("downlianshengbg/txtDownWinPoint_Text").GetComponent<Text>();
        sliderHP = curView.transform.Find("sliderHP_Slider").GetComponent<Slider>();
        txtsliderHP = curView.transform.Find("sliderHP_Slider/Handle Slide Area/Handle/txtsliderHP_Text").GetComponent<Text>();
        goTipJoin1 = curView.transform.Find("goHelpVertical/goTipJoin1_GameObject").gameObject;
        goTipJoin11 = curView.transform.Find("goHelpVertical/goTipJoin11_GameObject").gameObject;
        LVup = curView.transform.Find("LVup_GameObject").gameObject;
        LVdown = curView.transform.Find("LVdown_GameObject").gameObject;
        Kuangbao = curView.transform.Find("Kuangbao_GameObject").gameObject;
        KeZhiGuanXi = curView.transform.Find("KeZhiGuanXi_GameObject").gameObject;
        btnStart = curView.transform.Find("btnStart_Button").GetComponent<Button>();
        btnStart.onClick.AddListener(btnStartOnClick);
        txttime3min = curView.transform.Find("btnStart_Button/txttime3min_Text").GetComponent<Text>();
        btnRankWeek = curView.transform.Find("btnRankWeek_Button").GetComponent<Button>();
        btnRankWeek.onClick.AddListener(btnRankWeekOnClick);
        btnSetting = curView.transform.Find("btnSetting_Button").GetComponent<Button>();
        btnSetting.onClick.AddListener(btnSettingOnClick);
        goWarning = curView.transform.Find("goWarning_GameObject").gameObject;
        txtWaring = curView.transform.Find("goWarning_GameObject/txtWaring_Text").GetComponent<Text>();
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————
                                                                                                
    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void btnStartOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        GameRootManager.Instance.isPlay = true;
        GameRootManager.Instance.IsEnterReady = false;
        ShowHelpText();
        btnStart.gameObject.SetActive(false);
        GameRootManager.Instance.CloseRefreshTime();
        int minutes = PlayerPrefs.GetInt(PlayerPrefKey.TimeIndex, 20);
        GameRootManager.Instance.Init(minutes * 60 + 1);
        txtCountdown.text = minutes.ToString() + ":00";
    }


    public void ShowHelpText()
    {
        PlayerPrefs.GetInt("isTipHorizontal", 1);
    }


    public void btnRankWeekOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        //PanelMgr.Instance.OpenPanel<PanelRank>();
    }

    public void btnSettingOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        PanelMgr.Instance.OpenPanel<PanelSetting>();
    }

    public bool isTimeWarning = false;
    public int tipIndex = 0;

    public void CustomComponent()
    {
        sliderHP.onValueChanged.AddListener((value) => { txtsliderHP.text = Mathf.Round(value) + "米"; });
        EventMgr.Instance.AddEventListener<int>(EventConst.GameUpdateCountdown, OnUpdateCountdown);
        EventMgr.Instance.AddEventListener<int>(EventConst.GameUpdateCountdown2, OnUpdateCountdown2);
        
        EventMgr.Instance.AddEventListener<int, PushGiftData, int, int, bool>(EventConst.GamePushGiftShow,
            OnGamePushGiftShow);
        EventMgr.Instance.AddEventListener<int, int, int>(EventConst.GameUserLevelup, OnGameUserLevelUp);
        EventMgr.Instance.AddEventListener<int, int, int>(EventConst.GameUserShanghai, OnGameUserShangHai);
        EventMgr.Instance.AddEventListener<int>(EventConst.GameMaxLengh, OnGameMaxLengh);
    }

    public override void OnShow()
    {
        base.OnShow();
        AudioMgr.Instance.PlayMusic(MyAudioName.bgm_1);
        OnUpdateScorePool(GameRootManager.Instance.totalScore);
        goTipJoin1.SetActive(PlayerPrefs.GetInt("isTipJoin", 1) == 1);
        int minutes = PlayerPrefs.GetInt(PlayerPrefKey.TimeIndex, 20);
        txtCountdown.text = minutes + ":00";
        isTimeWarning = false;
        tipIndex = 0;
        sliderHP.maxValue = 2000;
        InvokeRepeating(nameof(SetDifTip), 10, 10);
    }


    public void OnGameStart(string roundNum)
    {
        txtFirstGiftCount.text = "0/5";
        txtFirstGiftCount1.text = "0/5";
        PanelMgr.Instance.HidePanel(PanelName.PanelSetting);
        //PanelMgr.Instance.HidePanel(PanelName.Rank);
        sliderHP.value = 2000;
        GameRootManager.Instance.Reset();
        GameRootManager.Instance.IsEnterReady = true;
        Debug.Log("暖场环节开始Lua");
    }


    public void OnUpdateScorePool(long totalScore)
    {
        if (totalScore < 10000)
        {
            txtPool.text = totalScore.ToString();
        }
        else
        {
            txtPool.text = string.Format("{0:0.0}万", totalScore / 10000);
        }
    }

    public void OnUpdateWinCount(int leftWinCount, int rightWinCount)
    {
        txtUpWinPoint.text = leftWinCount.ToString();
        txtDownWinPoint.text = rightWinCount.ToString();
    }


    public void OnGameContinueWin(CampType campType, int count)
    {
        //PanelNetLoading.Start(3, nil, "结算中");
        GameRootManager.Instance.isPlay = false;
        txtCountdown.color = ToolsHelper.ToColor("FFFFFF");
        goWarning.SetActive(false);
        sliderHP.maxValue = 2000;
        sliderHP.value = 2000;
        txtCountdown.text = GameRootManager.Instance.countdown.ToString();
    }

    public void OnGameRecordRsp(long scorePool, long scorePoolShow, long pointPoolShow)
    {
        //PanelNetLoading.Stop();
        //如果是本地模式不需要發
        if (true)
        {
            MsgSend.SendGameRankQuery(0);
        }

        //PanelMgr.Instance.OpenPanel<PanelResult>(self.winnerCampType,scorePoolShow,PointPoolShow);
        Debug.Log("积分池数据" + scorePoolShow + "争夺连胜数据" + pointPoolShow);
    }

    public void OnChooseCamp(int userId)
    {
        // local userInfo = Data.UserDataDict:GetById(userId);
        // local weekRankDataInfo = Data.WeekRankDict:GetById(userId);
        // local monthRankDataInfo = Data.MonthRankDict:GetById(userId);
        // local weekRank = 100;
        // local monthRank = 100;
        // if weekRankDataInfo ~= nil then
        //     weekRank = math.min(weekRank, weekRankDataInfo.rankingWeek + 1);
        // print("该玩家目前周榜排名："..weekRankDataInfo.rankingWeek + 1)
        // end
        // if monthRankDataInfo ~= nil then
        //     monthRank = math.min(monthRank, monthRankDataInfo.rankingMonth + 1);
        // print("该玩家目前周榜排名："..monthRankDataInfo.rankingMonth + 1)
        // end
        //
        // if weekRank < 100 or monthRank < 100 then
        // local name = userInfo.campType == CampType.Left and "加入魔界阵营" or "加入仙界阵营";
        // self:PlayJoinItem(userInfo.campType, userInfo.nickname, userInfo.avatarUrl, name,userInfo.accrueWinPoint);
        // local data = {};
        // data.nickname = userInfo.nickname;
        // data.avatarUrl = userInfo.avatarUrl;
        // data.campType = userInfo.campType;
        // data.weekRank = weekRank;
        // data.monthRank = monthRank;
        // data.titleLevel = userInfo.titleLevel;
        // data.accrueWinPoint = userInfo.accrueWinPoint
        // PanelEnter.CreateUser(data);
        // else
        // local name = userInfo.campType == CampType.Left and "加入魔界阵营" or "加入仙界阵营";
        // self:PlayJoinItem(userInfo.campType, userInfo.nickname, userInfo.avatarUrl, name,userInfo.accrueWinPoint);
        // --self:PlayJoinText(userInfo.campType, userInfo.nickname);
        // end
    }

    public void SetDifTip()
    {
        tipIndex = tipIndex + 1;
        if (tipIndex > 4)
        {
            tipIndex = 1;
        }

        LVup.SetActive(tipIndex == 1);
        LVdown.SetActive(tipIndex == 2);
        Kuangbao.SetActive(tipIndex == 3);
        KeZhiGuanXi.SetActive(tipIndex == 4);
    }


    public void OnGameUpdateRankUser(List<UserDataInfo> leftUsers, List<UserDataInfo> rightUsers, bool isAnim)
    {
        // leftUsers, rightUsers, isAnim
        //   local leftList = {};
        // local rightList = {};
        // for i = 1, math.min(leftUsers.Count, 3) do
        //     table.insert(leftList, leftUsers[i - 1]);
        // end
        // for i = 1, math.min(rightUsers.Count, 3) do
        //     table.insert(rightList, rightUsers[i - 1]);
        // end
        //
        // if isAnim then
        //     local rankUserIdOld = {};
        //     local rankUserIdNew = {};
        //     local dataList, userData, userList;
        //     --统计原先和新的用户ID
        //     for j = 1, 2 do
        //         local pool;
        //         if j == 1 then
        //             dataList = leftList;
        //             pool = UserItemLeftPool
        //             userList = self.userListLeft
        //         else
        //             dataList = rightList;
        //             pool = UserItemRightPool
        //             userList = self.userListRight
        //         end
        //         for k, v in pairs(userList) do
        //             userData = v:GetData();
        //             rankUserIdOld[userData.id] = v:GetIndex();
        //         end
        //         for i = 1, 3 do
        //             userData = dataList[i];
        //             if userData ~= nil then
        //                 rankUserIdNew[userData.id] = i;
        //
        //                 --生成新增的用户
        //                 if not rankUserIdOld[userData.id] then
        //                     local userItem = self:NewRankUserItem(pool, userData, i, j);
        //                     table.insert(userList, userItem);
        //                 end
        //             end
        //         end
        //     end
        //     --排名变化
        //     for j = 1, 2 do
        //         local pool;
        //         if j == 1 then
        //             pool = UserItemLeftPool
        //             userList = self.userListLeft
        //         else
        //             pool = UserItemRightPool
        //             userList = self.userListRight
        //         end
        //         for _, userItem in pairs(userList) do
        //             userData = userItem:GetData();
        //             local oldRank = rankUserIdOld[userData.id];
        //             local newRank = rankUserIdNew[userData.id];
        //             --当前排名有用户
        //             if newRank ~= nil then
        //                 userItem:SetIndex(newRank);
        //                 --排名有变化
        //                 if newRank ~= oldRank then
        //                     --原先已上榜
        //                     if oldRank ~= nil then
        //                         --上升
        //                         if oldRank > newRank then
        //                             userItem:PlayUp()
        //                         --下降
        //                         else
        //                             userItem:PlayDown(oldRank)
        //                         end
        //                     --原先未上榜 上升
        //                     else
        //                         userItem:PlayUp()
        //                     end
        //                 else
        //                     userItem:Refresh(); 
        //                 end
        //             --用户移除
        //             else
        //                 local list = userList;
        //                 local item = userItem;
        //                 userItem:PlayDisappear(function()
        //                     item:UnInit();
        //                     table.removebyvalue(list, item);
        //                     pool:RecyclePoolObj(item.gameObject);
        //                 end)
        //             end
        //         end
        //     end
        // else
        //     self:ReleaseRankUser();
        //     for i = 1, 3 do
        //         if leftList[i] ~= nil then
        //             self.userListLeft[i] = self:NewRankUserItem(UserItemLeftPool, leftList[i], i, 1);
        //         end
        //         if rightList[i] ~= nil then
        //             self.userListRight[i] = self:NewRankUserItem(UserItemRightPool, rightList[i], i, 2);
        //         end
        //     end
        // end
    }


    public void OnGamePushGiftShow(int userId, PushGiftData giftSetting, int count, int curprice, bool FirstGift)
    {
        // local soldierId = giftSetting.soldierId;
        // local giftName = giftSetting.name;
        // local userDataInfo = Data.UserDataDict:GetById(userId);
        // local soldierData = LuaCommonData.GetSuperSoldierBySoldierId(soldierId)
        // local userDataInfo = Data.UserDataDict:GetById(userId);
        // self:PlayGiftItem(userDataInfo.campType, userDataInfo.nickname, userDataInfo.avatarUrl, giftName, curprice,count)
    }


    public void OnGamePushFirstGift(int userId, PushGiftData giftData1, int firstGiftID, PushGiftData giftData,
        int count, int price)
    {
        // if LuaCommonData.IsLocalMode then return end;
        // local userDataInfo = Data.UserDataDict:GetById(userId);
        // if userDataInfo.campType == CampType.Left then
        // self.leftCount = self.leftCount + 1 
        // XObject.SetText(self.txtFirstGiftCount, self.leftCount.."/5")
        // else
        // self.rightCount = self.rightCount + 1
        // XObject.SetText(self.txtFirstGiftCount1, self.rightCount.."/5")
        // end
        // local giftName = giftSetting.name;
        // local giftName2 = giftSetting2.name;
        // self:PlayGiftItem(userDataInfo.campType, userDataInfo.nickname, userDataInfo.avatarUrl, giftName, curprice,count,true,giftName2)
    }

    public void OnGameTipMarquee(int type)
    {
        // local msg = MarqueeMsg[marqueeType];
        // if not string.IsNullOrEmpty(msg) then
        // PanelMarquee.Create(msg);
        // end
    }

    public void OnGameUserLevelUp(int userId, int level, int toNextLevel)
    {
        // local userDataInfo = Data.UserDataDict:GetById(userId);
        // self:PlayLevelUpItem(userDataInfo.avatarUrl,userDataInfo.campType, LuaCommonData.GetLeaveData(Level).name,NeedJiFen,Level )
    }

    public void OnGameUserShangHai(int id, int needHit, int curprice)
    {
        // local userDataInfo = Data.UserDataDict:GetById(userId);
        // if needHit < 10000 then
        // self:PlayShangHaiItem(userDataInfo.campType,userDataInfo.nickname,userDataInfo.avatarUrl,needHit,curprice)
        // else 
        // if self.oldHit == nil then
        // self.oldHit = 0
        // end
        // self:PlayTotalKillItem(userDataInfo.campType,userDataInfo.nickname,userDataInfo.avatarUrl,self.oldHit,needHit)
        // self.oldHit = needHit
        // end
    }

    public void OnGameMaxLengh(int maxLengh)
    {
        if (maxLengh <= 100)
        {
            goWarning.SetActive(true);
            txtWaring.text = maxLengh.ToString();
        }

        sliderHP.value = maxLengh;
    }

    
    public void OnUpdateCountdown2(int value)
    {
        txttime3min.text = TimeHelper.FormatTimeToString(value);
    }
    

    public void OnUpdateCountdown(int value)
    {

        if (value <= 300)
        {
            txtCountdown.color = ToolsHelper.ToColor("FF4747");
            if (value <= 30)
            {
                isTimeWarning = true;
            }
        }
        else
        {
            txtCountdown.color = ToolsHelper.ToColor("FFFFFF");
            isTimeWarning = false;
        }

        txtCountdown.text = TimeHelper.FormatTimeToString(value);

        if (isTimeWarning)
        {
            goTimeBG.transform.DOScale(0.9f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                goTimeBG.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutQuad);
            });
        }

        tipUpJoin.transform.DOScale(0.9f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            tipUpJoin.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutQuad);
        });
        tipDownJoin.transform.DOScale(0.9f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            tipDownJoin.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutQuad);
        });

        goWarning.transform.DOScale(0.7f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            goWarning.transform.DOScale(0.9f, 0.5f).SetEase(Ease.OutQuad);
        });
    }


    public override void OnHide()
    {
        CancelInvoke(nameof(SetDifTip));
        base.OnHide();
    }

    public override void OnClose()
    {
        CancelInvoke(nameof(SetDifTip));
        base.OnClose();
    }
}