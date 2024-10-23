using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LeiYunBullet : BaseBullet
{
    public GameObject headIcon;
    public GameObject headIconObject;
    public BoxCollider LeiYunBoxCollider;

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

        var table = GiftEffectConfig.Get(10);
        string para = table.para;
        string[] info = para.Split(";");
        onehitmaxhp = int.Parse(info[0]);
        hp = int.Parse(info[1]);
    }

    public override void Move(UserDataInfo info, Vector3 _dir, float _speed, float _duration, int level)
    {
        LeiYunBoxCollider.isTrigger = true;
        Invoke("SetTirgger", 0.5f);
        Destroy(this.gameObject, duration);
    }

    public void SetTirgger()
    {
        LeiYunBoxCollider.enabled = false;
    }


    public override void OnCollisionEnter(Collision other)
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
    }
}