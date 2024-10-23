using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuQiDingItem : BaseAtkItem
{
    public GameObject spawneft;

    public Transform spawnpos;

    public int spwannum = 5;
    public int tempHp = 500;


    public int spwanmaxnum = 8;

    public override void Init(UserDataInfo info = null, int ID = 5, bool canspwan = false, float scale = 1, int HP = 0)
    {
        base.Init(info, ID, canspwan, scale);
        spwanmaxnum = 8;
        var table = GiftEffectConfig.Get(ID);
        string para = table.para;
        string[] allpara = para.Split(";");
        spwannum = int.Parse(allpara[0]); // 每次子彈消耗數量
        tempHp = int.Parse(allpara[1]); // 每次循环发射几波
        if (canspwan == false)
        {
            oriHp = tempHp;
            maxHP = tempHp;
            if (hpText)
            {
                hpText.text = maxHP.ToString();
            }
        }

        if (maxHP <= 500)
        {
            curdieEft = dieEft[2];
        }
        else
        {
            curdieEft = dieEft[3];
        }


        if (canspwan)
        {
            StartCoroutine(CrateChild());
        }
    }


    IEnumerator CrateChild()
    {
        yield return new WaitForSeconds(5f);
        while (isDead == false && spwanmaxnum > 0)
        {
            if (GameRootManager.Instance.isPlay && GameRootManager.Instance.IsEnterReady == false)
            {
                AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_penfa, gameObject, 0.6f);
                spawneft.SetActive(true);
                yield return new WaitForSeconds(1f);
                float z = spawnpos.position.z + Random.Range(5, 10);
                Vector3 childpos = new Vector3(spawnpos.position.x, spawnpos.position.y, z);
                GameRootManager.Instance.gameAtkCamp.GenerateMuQiDing(Info, 5, 1, 0.5f, false, childpos);
                spwanmaxnum--;
                yield return new WaitForSeconds(0.5f);
                spawneft.SetActive(false);
            }

            yield return new WaitForSeconds(5f);
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
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_qiaoji3, gameObject, 0.1f);
    }

    public override IEnumerator DestroyGift()
    {
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_daota, gameObject, 0.6f);
        base.AddScore();
        isDead = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        UIItem.SetActive(false);
        curitem.SetActive(false);
        curdieEft.SetActive(true);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}