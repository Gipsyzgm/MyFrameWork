using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShengNvItem : BaseAtkItem
{
    public GameObject ChuXianEft;
    private Vector3 previousPosition;


    public override void Init(UserDataInfo info = null, int ID = 4, bool canspwan = false, float scale = 1, int HP = 0)
    {
        base.Init(info, ID, canspwan, scale);
        
        InvokeRepeating("AutoHit", 1f, 1f);
    }
    

    
    
    public void AutoHit()
    {
        if (GameRootManager.Instance.isPlay)
        {
            if (GameRootManager.Instance.gameDefCamp.IsYuanCiShanAlive)
            {
                maxHP = maxHP - 800;
                if (maxHP <= 0)
                {
                    StartCoroutine(DestroyGift());
                }
                else
                {
                    if (hpText)
                    {
                        hpText.text = maxHP.ToString();
                    }
                }
            }
        }
    }
    
    public override void OnCollisionEnter(Collision other)
    {
        if (isDead == false)
        {
            if (other.collider.tag == "bullet")
            {
                BaseBullet bullet = other.collider.gameObject.GetComponent<BaseBullet>();
                maxHP = maxHP - bullet.hp;
                if (maxHP <= 0)
                {
                    if (maxHP == 0)
                    {
                        GameRootManager.Instance.AddScore(CampType.Def, bullet.hp, 1);
                        bullet.Info?.AddScore(bullet.hp, 1);
                        if (bullet.Type == 0)
                        {
                            Destroy(other.collider.gameObject);
                        }
                    }
                    else
                    {
                        bullet.hp = Mathf.Abs(maxHP);
                    }

                    StartCoroutine(DestroyGift());
                }
                else
                {
                    GameRootManager.Instance.AddScore(CampType.Def, bullet.hp, 1);
                    bullet.Info?.AddScore(bullet.hp, 1);
                    if (bullet.Type == 0)
                    {
                        Destroy(other.collider.gameObject);
                    }

                    if (hpText)
                    {
                        hpText.text = maxHP.ToString();
                    }
                }

                PlaySounds();
            }
        }

    }

    public override void PlaySounds()
    {
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_qiaoji, gameObject, 0.1f);
    }

    public override IEnumerator DestroyGift()
    {
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_baozha, gameObject, 0.6f);
        base.AddScore();
        isDead = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        UIItem.SetActive(false);
        curitem.SetActive(false);
        curdieEft.SetActive(true);
        dieEft[0].SetActive(true);
        yield return new WaitForSeconds(0.4f);
        GameRootManager.Instance.gameDefCamp.HitYuanCiShan(-9500);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}