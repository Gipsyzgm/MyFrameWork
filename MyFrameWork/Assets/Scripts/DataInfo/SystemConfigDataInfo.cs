using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XOProto;

public class SystemConfigDataInfo : BaseDataInfo
{
    public List<long> gameTime { get; private set; }
    public int firstGiftMax { get; private set; }


    public override void Destroy()
    {

    }

    public SystemConfigDataInfo()
    {
        gameTime = new List<long>();
    }

    public void SetDto(SystemConfigDto dto)
    {
        if (dto != null)
        {
            if (dto.HasFirstGiftMax) firstGiftMax = dto.FirstGiftMax;
            if (dto.GameTime != null)
            {
                gameTime.Clear();
                gameTime.AddRange(dto.GameTime);
            }
        }
    }
}