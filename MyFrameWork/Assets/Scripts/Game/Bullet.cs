using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : BaseBullet
{
    
    public override void Move( UserDataInfo info, Vector3 _dir, float _speed, float _duration, int level)
    {
        
        hp = Mathf.CeilToInt(hp * (1 + GameRootManager.Instance.gameDefCamp.Gift7BuffCoef * GameRootManager.Instance.gameDefCamp.Gift7BuffLevel));
        Info = info;
        dir = _dir;
        speed = _speed;
        duration = _duration;
        StartMove = true;
        transform.rotation = Quaternion.LookRotation(dir);
        int bgindex = (int)Mathf.Ceil(level / 3f);
        if (items.Length > bgindex)
        {
            curuseitem = items[bgindex];
        }
        curuseitem.SetActive(true);
        //Invoke("UnloadGameObject", _duration);
        Destroy(this.gameObject, duration);
    }


    public override void UnloadGameObject()
    {
        this.gameObject.SetActive(false);
    }
    
}