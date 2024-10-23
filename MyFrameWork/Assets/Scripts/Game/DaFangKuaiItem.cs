using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class DaFangKuaiItem : BaseAtkItem
{
    public GameObject eftfangkuai;

    public override void Init(UserDataInfo info = null, int ID = 1, bool canspwan = false, float scale = 1, int HP = 0)
    {
        base.Init(info, ID, canspwan, scale, HP);
        SetBaseInfo();
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_xiaobaoqi, gameObject, 0.5f);
    }


    public void Healthbonus(float ratio)
    {
        maxHP = Mathf.CeilToInt(maxHP * ratio);
        oriHp = maxHP;
        if (hpText != null) hpText.text = maxHP.ToString();
    }

    public void AddHpForNoPos(int giftCount)
    {
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


    public void SetBaseInfo()
    {
        transform.localScale = Vector3.one * 2.4f;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        headIcon.SetActive(true);
        eftfangkuai.SetActive(false);
        curdieEft.SetActive(false);
        curitem.SetActive(true);
    }


    public override IEnumerator DestroyGift()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_suilie, gameObject, 0.4f);
        base.AddScore();
        isDead = true;
        hpText.text = "";
        headIcon.SetActive(false);
        curitem.SetActive(false);
        eftfangkuai.SetActive(false);
        curdieEft.SetActive(true);
        yield return new WaitForSeconds(1f);
        transform.position = Vector3.down * 100;
        GameRootManager.Instance.RemoveFromDaShenShiList(this);
    }
}