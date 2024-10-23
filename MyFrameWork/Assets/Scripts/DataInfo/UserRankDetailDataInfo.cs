using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XOProto;

public class UserRankDetailDataInfo : BaseDataInfo
{
    public UserDataInfo user { get; private set; }      //玩家信息 1
    public long score { get; private set; }         //当前局总积分
    public long scoreWeek { get; private set; }      //主播间周积分 每局结束后更新 1
    public long scoreMonth { get; private set; }      //世界月积分 每局结束后更新 1
    public long rankChangeWeek { get; private set; }      //上次游戏结束后名次变化 周 1
    public long rankChangeMonth { get; private set; }      //上次游戏结束后名次变化 月 1
    public long rankingWeek { get; private set; }      //周榜 每局结束后更新 1
    public long rankingMonth { get; private set; }      //月榜排名 每局结束后更新 1
    public long scoreAdd { get; private set; }      //上次结算积分变化数量 1
    public long pointAdd { get; private set; }      //胜点变化
    public long pointShare { get; private set; }     //胜点池瓜分
    public long scoreShare { get; private set; }     //积分池瓜分
    public int camp { get; private set; }       //阵营  0 上    1 下

    public UserRankDetailDataInfo()
    {
        user = new UserDataInfo();
    }

    public void SetDto(UserRankDetailDto dto)
    {
        if (dto != null)
        {
            if (dto.User != null) user.SetDto(dto.User);
            if (dto.HasScoreWeek) scoreWeek = dto.ScoreWeek;
            if (dto.HasScoreMonth) scoreMonth = dto.ScoreMonth;
            if (dto.HasScoreAdd) scoreAdd = dto.ScoreAdd;
            if (dto.HasRankingWeek) rankingWeek = dto.RankingWeek;
            if (dto.HasRankingMonth) rankingMonth = dto.RankingMonth;
            if (dto.HasRankChangeMonth) rankChangeMonth = dto.RankChangeMonth;
            if (dto.HasRankChangeWeek) rankChangeWeek = dto.RankChangeWeek;
            if (dto.HasScore) score = dto.Score;
            if (dto.HasScoreShare) scoreShare = dto.ScoreShare;
            if (dto.HasPointShare) pointShare = dto.PointShare;
            if (dto.HasPointAdd) pointAdd = dto.PointAdd;
            if (dto.HasCamp) camp = dto.Camp;
        }
    }

    public override void Destroy()
    {
    }
}

public static class UserRankDetailDataInfoHelper
{
    public static UserRankDetailDataInfo GetById(this Dictionary<int, UserRankDetailDataInfo> dict, int id)
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

    public static UserRankDetailDataInfo AddOrUpdate(this Dictionary<int, UserRankDetailDataInfo> dict, UserRankDetailDto dto)
    {
        UserRankDetailDataInfo info = null;
        if (dict != null && dto != null)
        {
            info = dict.GetById(dto.User.Id);
            if (info == null)
            {
                info = new UserRankDetailDataInfo();
            }
            info.SetDto(dto);
            dict[dto.User.Id] = info;
        }
        return info;
    }

    public static void AddOrUpdateRange(this Dictionary<int, UserRankDetailDataInfo> dict, IList<UserRankDetailDto> dtos)
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
