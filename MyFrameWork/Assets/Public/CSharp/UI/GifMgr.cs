using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
#if Do_Tween
using DG.Tweening;

/*
 * 使用方法：
 * 
 * 直接挂在礼包上，指定金币生成和消失的位置
 */

public class GifMgr : MonoBehaviour {

    DOTweenAnimation tween;
    Text timeText;
    Text goldText;
    public Transform CoinStartPos;
    public Transform CoinEndPos;
    Sprite coinIcon;

    bool canGetGift;
    float giftMin = 15;
    int gold = 500;

    string lastGiftTime;
    static string giftKey = "GetGiftKey_";

    void Start()
    {
        Init();
    }
    void Update()
    {
        UpdateCanGetGift();
    }

    public void OpenCompleteInit()
    {
        tween = GetComponent<DOTweenAnimation>();
        timeText = transform.GetChild(0).GetComponent<Text>();
        goldText = transform.GetChild(1).GetComponent<Text>();
        gameObject.SetActive(true);
        GetComponent<Image>().enabled = true;
        if(!canGetGift)
            timeText.gameObject.SetActive(true);
        goldText.gameObject.SetActive(true);
        goldText.text = gold + "";
    }

    //初始化
    void Init()
    {
        tween = GetComponent<DOTweenAnimation>();
        timeText = transform.GetChild(0).GetComponent<Text>();
        goldText = transform.GetChild(1).GetComponent<Text>();

        lastGiftTime = PlayerPrefs.GetString(giftKey, string.Empty);
        if (string.IsNullOrEmpty(lastGiftTime))
        {
            lastGiftTime = DateTime.Now.ToString();
            PlayerPrefs.SetString(giftKey, lastGiftTime);
        }

        UpdateCutMinutes();
    }
    //每帧检测时间
    void UpdateCanGetGift()
    {
        if (canGetGift) return;

        UpdateCutMinutes();
    }
    //取时间差
    void UpdateCutMinutes()
    {
        DateTime t1 = Convert.ToDateTime(lastGiftTime);
        DateTime t2 = DateTime.Now;
        TimeSpan ts1 = new TimeSpan(t1.Ticks);
        TimeSpan ts2 = new TimeSpan(t2.Ticks);
        double minutes = ts2.Subtract(ts1).TotalMinutes;
        double second = ts2.Subtract(ts1).TotalSeconds;

        if (minutes >= giftMin)
        {
            canGetGift = true;
            tween.DOPlay();
            timeText.gameObject.SetActive(false);
        }
        else
        {
            int m = (int)(giftMin - minutes);
            int s = (int)(60 - (second % 60));
            timeText.text = string.Format("{0:D2}:{1:D2}",m,s);
        }
    }

    //点击领取礼物
    public void GetGift()
    {

    }

    IEnumerator GifYiet()
    {
        for (int i = 0; i < 20; i++)
        {
            CreateGoldIcon();
            yield return new WaitForSeconds(0.05f);
        }
        GetComponent<Image>().enabled = false;
        timeText.gameObject.SetActive(false);
        goldText.gameObject.SetActive(false);


        yield return new WaitForSeconds(3);

        goldText.text = gold +"";
        lastGiftTime = DateTime.Now.ToString();
        PlayerPrefs.SetString(giftKey, lastGiftTime);
        GetComponent<Image>().enabled = true;
        timeText.gameObject.SetActive(true);
        goldText.gameObject.SetActive(true);

        TweenClass.TweenBedRoom(transform);

    }

    void CreateGoldIcon()
    {
        GameObject o = new GameObject();
        o.transform.SetParent(CoinStartPos);
        o.transform.localScale = Vector3.one;
        int x = UnityEngine.Random.Range(-50, 50);
        int y = UnityEngine.Random.Range(-50, 50);
        o.transform.localPosition = Vector3.zero;
        Image ima = o.AddComponent<Image>();
        ima.sprite = coinIcon;
        ima.SetNativeSize();
        o.transform.DOLocalMove(new Vector3(x, y), 0.15f).OnComplete(() =>
        {
            o.transform.DOMove(CoinEndPos.transform.parent.position, 0.3f).OnComplete(() =>
            {
                Destroy(o.gameObject);
                SoundMgr.Instance.PlaySound(5);
            });
        });
    }
}
#endif
