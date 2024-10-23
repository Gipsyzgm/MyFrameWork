using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XOProto;

public class PlayerDataInfo : BaseDataInfo
{
    public int type { get; private set; }   //类型 0:选边  1:评论  2:礼物  3:点赞  4:粉丝团
    public string giftId { get; private set; }  //抖音礼物id
    public int count { get; private set; }  //数量   type = 2 礼物数量    3 点赞数量
    public string content { get; private set; } //评论内容

    public void SetDto(PlayerDto dto)
    {
        if (dto != null)
        {
            if (dto.HasType) type = dto.Type;
            if (dto.HasGiftId) giftId = dto.GiftId;
            if (dto.HasCount) count = dto.Count;
            if (dto.HasContent) content = dto.Content;
        }
    }


    public override void Destroy()
    {
    }
}

