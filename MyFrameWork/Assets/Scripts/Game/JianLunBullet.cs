using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JianLunBullet : BaseBullet
{
    public GameObject headIcon;
    public GameObject headIconObject;

    public void SetInfo(UserDataInfo userDataInfo = null)
    {
        if (userDataInfo != null)
        {
            headIconObject?.SetActive(true);
            if (headIcon != null)
            {
                ToolsHelper.SetWebImage(headIcon, userDataInfo.avatarUrl);
            }
        }
        else
        {
            headIconObject?.SetActive(false);
        }
        
        var table = GiftEffectConfig.Get(8);
        string  para = table.para;
        string[] info = para.Split(";");
        onehitmaxhp  = int.Parse(info[0]); 
        hp = int.Parse(info[1]);
    }

    public override void Move(UserDataInfo info,Vector3 _dir, float _speed, float _duration, int level)
    {
        hp = Mathf.CeilToInt(hp * (1 + GameRootManager.Instance.gameDefCamp.Gift7BuffCoef * GameRootManager.Instance.gameDefCamp.Gift7BuffLevel));
        Info = info;
        dir = _dir;
        speed = _speed;
        duration = _duration;
        StartMove = true;
        transform.rotation = Quaternion.LookRotation(dir);
        Destroy(this.gameObject, duration);
        
    }
    
}