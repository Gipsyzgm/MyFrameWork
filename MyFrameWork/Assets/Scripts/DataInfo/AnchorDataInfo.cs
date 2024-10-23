using System.Collections;
using UnityEngine;
using XOProto;

public class AnchorDataInfo : BaseDataInfo
{
    public int id { get; private set; }
    public string openId { get; private set; }
    public string nickname { get; private set; }
    public string avatarUrl { get; private set; }

    public void SetDto(AnchorDto dto)
    {
        if (dto != null)
        {
            if (dto.HasId) id = dto.Id;
            if (dto.HasOpenId) openId = dto.OpenId;
            if (dto.HasNickname) nickname = dto.Nickname;
            if (dto.HasAvatarUrl) avatarUrl = dto.AvatarUrl;
        }
    }

    public override void Destroy()
    {
    }
}
