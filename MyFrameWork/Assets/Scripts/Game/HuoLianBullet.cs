using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HuoLianBullet : BaseBullet
{
    public bool IsBloom = false;
    public GameObject hiteft;


    public override void OnCollisionEnter(Collision other)
    {
        if (IsBloom == false)
        {
            if (other.collider.tag == "AtkItem" || other.collider.tag == "AtkItem2")
            {
                IsBloom = true;
                curuseitem.SetActive(false);
                hiteft.SetActive(true);
                AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_baozha2, gameObject, 0.5f);
                gameObject.transform.DOScale(2f, 0.5f).OnComplete(() => { Destroy(gameObject); });
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (IsBloom == false)
        {
            if (other.tag == "AtkItem" || other.tag == "AtkItem2")
            {
                IsBloom = true;
                curuseitem.SetActive(false);
                hiteft.SetActive(true);
                AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_baozha2, gameObject, 0.5f);
                gameObject.transform.DOScale(2, 0.5f).OnComplete(() => { Destroy(gameObject); });
            }
        }
    }
}