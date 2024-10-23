using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LingYuItem : BaseAtkItem
{
    public float dirtime = 120f;
    public float xiaoguo = 0.3f;

    public override void Init(UserDataInfo info = null, int ID = 7, bool canspwan = false, float scale = 1, int HP = 0)
    {
        base.Init(info, ID, canspwan, scale);
        timeSlider?.gameObject.SetActive(true);
        gameObject.SetActive(true);

        var table = GiftEffectConfig.Get(ID);
        dirtime = table.time;
        xiaoguo = float.Parse(table.para) / 100f;
        GameRootManager.Instance.gameAtkCamp.Gift7BuffAddNum = xiaoguo;
        
        StartCoroutine(CountTime());
        AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_leiting, gameObject, 0.4f);
    }


    public override void OnCollisionEnter(Collision other)
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
    }


    public override void OnTriggerStay(Collider other)
    {
    }

    IEnumerator CountTime()
    {
        yield return new WaitUntil(() => GameRootManager.Instance.isPlay);
        AddHp();
        GameRootManager.Instance.gameAtkCamp.Gift7BuffLevel++;
        float startTime = Time.time; // 获取开始时间
        float endTime = startTime + dirtime; // 计算结束时间
        float startFillAmount = timeSlider.fillAmount; // 获取初始填充量
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / dirtime; // 当前时间占总时间的比例
            timeSlider.fillAmount = Mathf.Lerp(startFillAmount, 0.0f, t); // 逐渐增加填充量
            yield return null; // 等待下一帧
        }

        timeSlider.fillAmount = 0.0f; // 确保最终完全填充
        GameRootManager.Instance.gameAtkCamp.Gift7BuffLevel--;
        Destroy(gameObject);
    }

    public void AddHp()
    {
        var GiftatkItems = GameObject.FindGameObjectsWithTag("AtkItem2");
        if (GiftatkItems.Length > 0)
        {
            // 寻找最近Cube的逻辑
            foreach (var cube in GiftatkItems)
            {
                BaseAtkItem dist = cube.GetComponent<BaseAtkItem>();
                dist.maxHP = Mathf.CeilToInt(dist.maxHP * (1 + GameRootManager.Instance.gameAtkCamp.Gift7BuffAddNum));
                dist.RefreshHp();
            }
        }

        var atkItems = GameObject.FindGameObjectsWithTag("AtkItem");
        if (atkItems.Length > 0)
        {
            foreach (var cube in atkItems)
            {
                BaseAtkItem dist = cube.GetComponent<BaseAtkItem>();
                dist.maxHP = Mathf.CeilToInt(dist.maxHP * (1 + xiaoguo));
                dist.RefreshHp();
            }
        }
    }
}