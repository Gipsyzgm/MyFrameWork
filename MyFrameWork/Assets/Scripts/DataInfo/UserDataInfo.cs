using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XOProto;
public class UserDataInfo : BaseDataInfo
{
    public int id { get; private set; }     // ID
    public string openId { get; private set; }      // 第三方唯一标识
    public string nickname { get; private set; }        // 玩家昵称
    public string avatarUrl { get; private set; }       // 用户头像
    public int titleLevel { get; private set; }       // 称谓等级
    public int winPoint { get; private set; }       // 连胜
    public int accrueWinPoint { get; private set; }       // 累计连胜

    public int costPoint;    //玩家加入房间注入胜点池的胜点


    public long monthRank;
    /// <summary>
    /// 阵营
    /// </summary>
    public int campType;
    
    /// <summary>
    /// 胜场
    /// </summary>
    public int winCount;
    /// <summary>
    /// 上次评论出兵时间
    /// </summary>
    public float lastCommentTime;
    /// <summary>
    /// 上次点赞出兵时间
    /// </summary>
    public float lastLikeTime;
    /// <summary>
    /// 自动出兵时间
    /// </summary>
    public float autoSoldierTime;
    /// <summary>
    /// 杀敌积分
    /// </summary>
    public int score;
    public int continueKill;
    public int totalKill;
    public int giftCount;
    public int hit;
    public int needHit = 200;
    public int needHitCount = 0;
    public int needHitCount2 = 0;
    /// <summary>
    /// 送礼价格
    /// </summary>
    public int curprice;
    
    /// <summary>
    /// 子弹数量
    /// </summary>
    public int bulletCount;
    public int totalLikeCount { get; private set; }
    public int soldierLevel { get; private set; }

    /// <summary>
    /// 礼物数量
    /// </summary>
    public Dictionary<int, int> dictGiftCount;
    public Dictionary<int, int> dictSuperCount;
    public Dictionary<long, int> dictGroupCount;

    public override void Destroy()
    {
    }

    public UserDataInfo()
    {
        dictGiftCount = new Dictionary<int, int>();
        dictSuperCount = new Dictionary<int, int>();
        dictGroupCount = new Dictionary<long, int>();
        soldierLevel = 1;
        monthRank = 100;
    }

    public UserDataInfo(UserData userData)
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
        curprice =  userData.curprice;
        bulletCount = userData.bulletCount;
        dictGiftCount = new Dictionary<int, int>();
        foreach (var kvp in userData.dictGiftCount)
        {
            dictGiftCount.Add(kvp.soldierId, kvp.count);
        }
        dictSuperCount = new Dictionary<int, int>();
        foreach (var kvp in userData.dictSuperCount)
        {
            dictSuperCount.Add(kvp.soldierId, kvp.count);
        }
        dictGroupCount = new Dictionary<long, int>();
        foreach (var kvp in userData.dictGroupCount)
        {
            dictGroupCount.Add(kvp.groupId, kvp.count);
        }
    }

    public void SetDto(UserDto dto)
    {
        if (dto != null)
        {
            if (dto.HasId) id = dto.Id;
            if (dto.HasOpenId) openId = dto.OpenId;
            if (dto.HasNickname) nickname = dto.Nickname;
            if (dto.HasAvatarUrl) avatarUrl = dto.AvatarUrl;
            if (dto.HasWinPoint) winCount = dto.WinPoint;
            if (dto.HasTitleLevel) titleLevel = dto.TitleLevel;
            if (dto.HasAccrueWinPoint) accrueWinPoint = dto.AccrueWinPoint;
            if (dto.HasWinPoint) winPoint = dto.WinPoint;
            
        }
    }

    public void SetCampType(CampType value)
    {
        campType = (int)value;
    }
    

    public int AddGiftSoldier(int superSoldierId, int count = 1)
    {
        if (!dictGiftCount.ContainsKey(superSoldierId)) dictGiftCount[superSoldierId] = 0;
        dictGiftCount[superSoldierId] = dictGiftCount[superSoldierId] + count;
        return dictGiftCount[superSoldierId];
    }

    public int GetGiftSoldier(int superSoldierId)
    {
        if (!dictGiftCount.ContainsKey(superSoldierId)) dictGiftCount[superSoldierId] = 0;
        return dictGiftCount[superSoldierId];
    }

    public int AddSuperCount(int superSoldierId)
    {
        if (!dictSuperCount.ContainsKey(superSoldierId)) dictSuperCount[superSoldierId] = 0;
        dictSuperCount[superSoldierId] = dictSuperCount[superSoldierId] + 1;
        return dictSuperCount[superSoldierId];
    }

    public void ReduceSuperCount(int superSoldierId)
    {
        if (dictSuperCount.ContainsKey(superSoldierId))
        {
            dictSuperCount[superSoldierId] = Mathf.Max(dictSuperCount[superSoldierId] - 1, 0);
        }
    }

    public int GetSuperCount(int superSoldierId)
    {
        if (!dictSuperCount.ContainsKey(superSoldierId)) dictSuperCount[superSoldierId] = 0;
        return dictSuperCount[superSoldierId];
    }


    public int AddGroupCount(int groupId)
    {
        if (!dictGroupCount.ContainsKey(groupId)) dictGroupCount[groupId] = 0;
        dictGroupCount[groupId] = dictGroupCount[groupId] + 1;
        return dictGroupCount[groupId];
    }

    public void ReduceGroupCount(int groupId)
    {
        if (dictGroupCount.ContainsKey(groupId))
        {
            dictGroupCount[groupId] = Mathf.Max(dictGroupCount[groupId] - 1, 0);
        }
    }

    public int GetGroupCount(int groupId)
    {
        if (!dictGroupCount.ContainsKey(groupId)) dictGroupCount[groupId] = 0;
        return dictGroupCount[groupId];
    }

    public void AddScore(int value,int type = 0)
    {
        int multiplier = 1;
        if (type == 0)
        {
            //送礼产生的
            multiplier = GameRootManager.Instance.GIFT_JIFEN;
        }
        else if (type == 1)
        {
            //伤害产生的
            multiplier =  GameRootManager.Instance.DAMAGE_JIFEN;
            hit += value;
        }
        
        score += value*multiplier;
        if (needHit > 0)
        {
            if (hit >= needHit && needHit <10000)
            {
                needHitCount++;
                EventMgr.Instance.InvokeEvent(EventConst.GameUserShanghai, id, needHit, curprice);
                switch (needHitCount)
                {
                    case 1:needHit += 400;break;
                    case 2:needHit += 400;break;
                    default:needHit += 1000;break;
                }

            }
            else if(hit >= needHit && needHit >= 10000)
            {
                needHitCount2++;
                EventMgr.Instance.InvokeEvent(EventConst.GameUserShanghai, id, needHit, curprice);
                switch (needHitCount)
                {
                    case 1: needHit += 20000; break;
                    default: needHit += 20000; break;
                }
            }
        }
        
    }

    public void AddPrice(int value)
    {
        curprice += value;
    }
    
    public void AddBullet(int value)
    {
        bulletCount += value;
    }
    
    public void AddKill()
    {
        continueKill++;
        totalKill++;
    }

    public void KillEnd()
    {
        continueKill = 0;
    }

    public void SetWinCount(int value)
    {
        winCount = value;
    }

    public void AddLike(int value = 1)
    {
        totalLikeCount += value;

        if (soldierLevel == 1 && totalLikeCount >= 50)
        {
            soldierLevel = 2;
        }
        if (soldierLevel == 2 && totalLikeCount >= 200)
        {
            soldierLevel = 3;
        }
    }

    public void Reset()
    {
        hit = 0;
        needHit = 200;
        needHitCount = 0;
        campType = 0;
        curprice = 0;
        score = 0;
        giftCount = 0;
        continueKill = 0;
        totalKill = 0;
        lastLikeTime = 0;
        lastCommentTime = 0;
        autoSoldierTime = 0;
        totalLikeCount = 0;
        soldierLevel = 1;
        dictGiftCount.Clear();
        dictSuperCount.Clear();
        dictGroupCount.Clear();
    }

    public void SetMonthRank(long value)
    {
        monthRank = value;
    }
}


public static class UserDataInfoHelper
{
    public static UserDataInfo GetById(this Dictionary<int, UserDataInfo> dict, int id)
    {
        if (dict != null && id > 0)
        {
            if (dict.ContainsKey(id))
            {
                return dict[id];
            }
        }
        return null;
    }

    public static UserDataInfo AddOrUpdate(this Dictionary<int, UserDataInfo> dict, UserDto dto)
    {
        UserDataInfo info = null;
        if (dict != null && dto != null)
        {
            info = dict.GetById(dto.Id);
            if (info == null)
            {
                info = new UserDataInfo();
            }
            info.SetDto(dto);
            dict[dto.Id] = info;
        }
        return info;
    }

    public static void AddOrUpdateRange(this Dictionary<int, UserDataInfo> dict, IList<UserDto> dtos)
    {
        if (dict != null && dtos != null)
        {
            foreach (var dto in dtos)
            {
                dict.AddOrUpdate(dto);
            }
        }
    }
}