using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoGuanItem : BaseAtkItem
{
    public int spwannum = 8;
    public int tempHp = 100;

    public float spwanratio = 1f;

    public GameObject DaMOGuan;
    public GameObject XiaoMOGuan;
    public override void Init(UserDataInfo info = null, int ID = 1, bool canspwan = false, float scale = 1, int HP = 0)
    {
        base.Init(info, ID, canspwan, scale);
        var table = GiftEffectConfig.Get(ID);
        string para = table.para;
        string[] allpara = para.Split(";");
        spwannum = int.Parse(allpara[0]); // 每次子彈消耗數量
        tempHp = int.Parse(allpara[1]); // 每次循环发射几波
        if (canspwan == false)
        {
            oriHp = tempHp;
            maxHP = tempHp;
            hpText.text = maxHP.ToString();
            curitem = XiaoMOGuan;
        }
        else
        {
            curitem = DaMOGuan;
        }
        curitem.SetActive(true);
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        curdieEft.SetActive(false);
        dieEft[0].SetActive(false);
    }

    public void Healthbonus(float ratio)
    {
        spwanratio = ratio;
        maxHP = Mathf.CeilToInt(maxHP * ratio);
        oriHp = maxHP;
        if (hpText != null) hpText.text = maxHP.ToString();
    }

    public void AddHpForNoPos(int giftCount)
    {
        spwanratio += giftCount;
        maxHP += oriHp * giftCount;
        if (hpText != null) hpText.text = maxHP.ToString();
        StartCoroutine(ShowUPHpEft());

    }
    
    
    public IEnumerator ShowUPHpEft()
    {
        UpHpEft.SetActive(true);
        yield return new WaitForSeconds(1f);
        UpHpEft.SetActive(false);
    }
    

    public override IEnumerator DestroyGift()
    {
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_baozha, gameObject, 0.4f);
        base.AddScore();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        isDead = true;
        curitem.SetActive(false);
        curdieEft.SetActive(true);
        dieEft[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);


        if (canspwan)
        {
            GameRootManager.Instance.gameAtkCamp.GenerateMoGuan(Info, 2, spwannum, 0.48f, false, transform.position,
                spwanratio);
        }

        transform.position = Vector3.down * 100;
        GameRootManager.Instance.RemoveFromMoGuanList(this);
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
}