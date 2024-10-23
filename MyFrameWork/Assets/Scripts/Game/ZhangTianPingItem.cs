using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhangTianPingItem : BaseDefItem
{
    public Text hpText;
    public int Hp;

    public override void Init(UserDataInfo userData, int ID = 14)
    {
        base.Init(userData, ID);
        gameObject.SetActive(true);
        StartCoroutine(CountTime());
      
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_guangyu, gameObject, 0.5f);
    }


    IEnumerator CountTime()
    {
        yield return new WaitUntil(() => GameRootManager.Instance.isPlay);
        GameRootManager.Instance.gameDefCamp.Gift7BuffLevel++;
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
        GameRootManager.Instance.gameDefCamp.Gift7BuffLevel--;
        Destroy(gameObject);
    }
}