using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class HuangTaItem : BaseAtkItem
{
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
    
        AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_qiaoji3, gameObject, 0.1f);
    }

    public override IEnumerator DestroyGift()
    {
        AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_daota2, gameObject, 0.6f);
        base.AddScore();
        isDead = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        curitem.SetActive(false);
        UIItem.SetActive(false);
        curdieEft.SetActive(true);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}