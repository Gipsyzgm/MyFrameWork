using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XuKongJingItem : BaseAtkItem
{
    public GameObject fashe_eft;
    public GameObject eftfashe;
    public GameObject eftfashesize;
    public GameObject target; 
    public override void Init(UserDataInfo info = null, int ID = 3, bool canspwan = false, float scale = 1, int HP = 0)
    {
        base.Init(info, ID, canspwan, scale);
        StartCoroutine(Shoot());
    }
    public bool Getdir()
    {
        var atkItems = GameObject.FindGameObjectsWithTag("DefItem");

        if (atkItems.Length == 0)
        {
            return false;
        }

        target = atkItems[Random.Range(0,atkItems.Length) ].gameObject;
        if (target)
        {
            eftfashe.transform.LookAt(target.transform.position);
            //fashe_eft.transform.LookAt(target.transform.position);
            float dist = Vector3.Distance(transform.position,target.transform.position);
            float size = dist / 31 * 1;
            eftfashesize.transform.localScale = new Vector3(size,1,1);
            return true;
        }
        else
        {
            return false;
        }
    }
    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2f);
        while (isDead == false)
        {
            if (GameRootManager.Instance.isPlay && GameRootManager.Instance.IsEnterReady == false)
            {
                if (Getdir())
                {
                    fashe_eft.SetActive(true);
                    AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_jiguang, gameObject, 0.5f);
                    yield return new WaitForSeconds(1f);
                    BaseDefItem df = target.GetComponent<BaseDefItem>();
                    eftfashe.SetActive(true);
                    yield return new WaitForSeconds(0.2f);
                    df.ShowICEEft();
                    df.ShowICEGround();
                    yield return new WaitForSeconds(2f);
                    eftfashe.SetActive(false);
                    fashe_eft.SetActive(false);
                    yield return new WaitForSeconds(4f);
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public override IEnumerator DestroyGift()
    {
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_jingzisuilie, gameObject, 0.6f);
        base.AddScore();
        isDead = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        curitem.SetActive(false);
        UIItem.SetActive(false);
        curdieEft.SetActive(true);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
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
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_jingmianshouji, gameObject, 0.5f);
    }
}