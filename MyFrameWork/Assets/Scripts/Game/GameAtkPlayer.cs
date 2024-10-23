using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameAtkPlayer : MonoBehaviour
{
    public Image imgBg;
    public GameObject imgHead;
    public UserDataInfo info;
    public List<Sprite> levelBg;
    public List<GameObject> toplevel;

    /// <summary>
    /// 局内等级
    /// </summary>
    public int level = 0;

    /// <summary>
    /// 送礼数量
    /// </summary>
    public int price = 0;

    /// <summary>
    /// 生成方块血量
    /// </summary>
    public int hp = 0;

    [Header("玩家每秒生成数量")] public float SpwanNum = 1f;

    [Header("生成频率")] public float generateSoldierInterval = 2f;

    //计时
    private float totalTime;
    private float totalTime2;

    /// <summary>
    /// 玩家付费间隔
    /// </summary>
    private float totalTime2Interval = 0.5f;

    private bool IsAlive = true;

    public void Init(UserDataInfo info)
    {
        this.info = info;
        IsAlive = true;
        if (info != null)
        {
            imgHead.SetActive(true);
            ToolsHelper.SetWebImage(imgHead, info.avatarUrl);
            CheckLvUp(info);
        }
    }

    void Update()
    {
        if (GameRootManager.Instance.isPlay && IsAlive)
        {
            if (GameRootManager.Instance.gameAtkCamp.StartSpwan)
            {
                if (GameRootManager.Instance.gameAtkCamp.CurActiveNum < GameRootManager.Instance.gameAtkCamp.MaxAtkNum)
                {
                    totalTime2 += Time.deltaTime;
                    if (totalTime2 >= totalTime2Interval)
                    {
                        if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 400)
                        {
                            totalTime2Interval = 0.5f;
                        }
                        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 500)
                        {
                            totalTime2Interval = 1f;
                        }
                        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 600)
                        {
                            totalTime2Interval = 2f;
                        }
                        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 700)
                        {
                            totalTime2Interval = 3f;
                        }
                        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 800)
                        {
                            totalTime2Interval = 5f;
                        }
                        else
                        {
                            totalTime2Interval = 10f;
                        }

                        if (info.curprice > 0)
                        {
                            int hp = level * 2;
                            if (hp <= 0)
                            {
                                hp = 0;
                            }

                            GameRootManager.Instance.gameAtkCamp.GenerateSoldierByPlayer(this, 0, 0, hp);
                        }

                        totalTime2 = 0;
                    }


                    totalTime += Time.deltaTime;
                    if (totalTime >= generateSoldierInterval)
                    {
                        GameRootManager.Instance.gameAtkCamp.GenerateSoldierByPlayer(this, 0, 1);
                        totalTime = 0; // 更新上次发射时间
                    }
                }
            }
        }
    }

    public void CheckLvUp(UserDataInfo info)
    {
        price = info.curprice;
        CountLevel();
        GameRootManager.Instance.gameAtkCamp.UpDatePlayerInfo();
    }


    private void CountLevel()
    {
        if (level == 24)
        {
            return;
        }

        var table = GradeInnerConfig.Get(level + 1);
        int needprice = table.needMoney;
        if (info.curprice >= needprice)
        {
            level++;
            int needprice1;
            if (level < 24)
            {
                needprice1 = table.needMoney;
            }
            else
            {
                needprice1 = 0;
            }

            Debug.Log("玩家名称：" + info.nickname + "玩家目前总消费：" + info.curprice);
            int toNextLevel;
            if (needprice1 - price > 0)
            {
                toNextLevel = needprice1 - price;
            }
            else
            {
                toNextLevel = 0;
            }

            if (info != null)
            {
                EventMgr.Instance.InvokeEvent(EventConst.GameUserLevelup, info.id, level, toNextLevel);
            }

            Debug.Log("玩家现在等级：" + level + "玩家目前消费分数：" + price + "玩家下级需要的分数：" + (needprice1 - price));
            if (level >= 24)
            {
                level = 24;
                ShowLvInfo();
            }
            else
            {
                Debug.Log("收到玩家升级消息。玩家目前等级：" + level);
                CountLevel();
            }
        }
        else
        {
            ShowLvInfo();
        }
    }


    private void ShowLvInfo()
    {
        int bgindex = (int)Mathf.Ceil(level / 3f);
        imgBg.sprite = levelBg[bgindex];

        if (level > 0)
        {
            var table = GradeInnerConfig.Get(level);
            hp = table.blockLift;
            int topshow = level % 3;
            if (topshow == 0)
            {
                topshow = 3;
            }

            for (int i = 0; i < toplevel.Count; i++)
            {
                if (i < topshow)
                {
                    toplevel[i].SetActive(true);
                }
                else
                {
                    toplevel[i].SetActive(false);
                }
            }
        }
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("生成一个方块")]
#endif
    private void SpwnAtkItem()
    {
        GameRootManager.Instance.gameAtkCamp.GenerateSoldierByPlayer(this);
    }


    public void UnInit()
    {
        info = null;
        level = 0;
        price = 0;
        IsAlive = false;
    }


    public void GetGiftInfo(UserDataInfo info, int giftid, int giftCount)
    {
        StartCoroutine(CrateGiftItem(info, giftid, giftCount, 0.1f));
    }

    IEnumerator CrateGiftItem(UserDataInfo info, int giftid, int giftCount, float interval)
    {
        switch (giftid)
        {
            case 21: //仙女棒 大方块

                int remain = GameRootManager.Instance.gameAtkCamp.MaxDaShenShiNum -
                             GameRootManager.Instance.gameAtkCamp.CurDaShenShiNum;

                if (remain > giftCount)
                {
                    for (int i = 0; i < giftCount; i++)
                    {
                        GameRootManager.Instance.gameAtkCamp.CrateSuperSoldier(info);
                        yield return new WaitForSeconds(interval);
                    }
                }
                else
                {
                    if (remain > 0)
                    {
                        float ratio = (float)giftCount / remain;
                        for (int i = 0; i < remain; i++)
                        {
                            GameRootManager.Instance.gameAtkCamp.CrateSuperSoldier(info, 1, ratio);
                            yield return new WaitForSeconds(interval);
                        }
                    }
                    else
                    {
                        DaFangKuaiItem daFangKuaiItem = null;
                        foreach (var item in GameRootManager.Instance.gameAtkCamp.DaShenShiList)
                        {
                            if (item.Info.id == info.id)
                            {
                                daFangKuaiItem = item;
                                break;
                            }
                        }

                        if (daFangKuaiItem != null)
                        {
                            daFangKuaiItem.AddHpForNoPos(giftCount);
                        }
                        else
                        {
                            GameRootManager.Instance.gameAtkCamp.CrateSuperSoldier(info, 1, giftCount);
                        }
                    }
                }

                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_dafangkuai, 0.8f);
                break;
            case 10: //能力药丸  魔罐
                int remain2 = GameRootManager.Instance.gameAtkCamp.MaxMoGuanNum -
                              GameRootManager.Instance.gameAtkCamp.CurMoGuanNum;
                if (remain2 > giftCount)
                {
                    for (int i = 0; i < giftCount; i++)
                    {
                        GameRootManager.Instance.gameAtkCamp.GenerateMoGuan(info);
                        yield return new WaitForSeconds(interval);
                    }
                }
                else
                {
                    if (remain2 > 0)
                    {
                        float ratio = (float)giftCount / remain2;
                        for (int i = 0; i < remain2; i++)
                        {
                            GameRootManager.Instance.gameAtkCamp.GenerateMoGuan(info, 2, 1, 0.6f, true, default, ratio);
                            yield return new WaitForSeconds(interval);
                        }
                    }
                    else
                    {
                        MoGuanItem moguanItem = null;
                        foreach (var item in GameRootManager.Instance.gameAtkCamp.MoGuanList)
                        {
                            if (item.Info.id == info.id)
                            {
                                moguanItem = item;
                                break;
                            }
                        }

                        if (moguanItem != null)
                        {
                            moguanItem.AddHpForNoPos(giftCount);
                        }
                        else
                        {
                            GameRootManager.Instance.gameAtkCamp.GenerateMoGuan(info, 2, 1, 0.6f, true, default,
                                giftCount);
                        }
                    }
                }

                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_moguan, 0.8f);
                break;
            case 11: //甜甜圈  虚空镜
                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameAtkCamp.GenerateXuKongJing(info);
                    yield return new WaitForSeconds(interval);
                }

                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_xukongjing, 0.8f);
                break;
            case 25: //能量电池  圣女炉
                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameAtkCamp.GenerateShengNv(info);
                    yield return new WaitForSeconds(interval);
                }

                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_shengnvlu, 0.8f);
                break;
            case 23: //爱的爆炸  万物母气鼎
                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameAtkCamp.GenerateMuQiDing(info, 5, 1, 1, true);
                    yield return new WaitForSeconds(interval);
                }

                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_muqiding, 0.8f);
                break;
            case 8: //神秘空投  荒塔
                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameAtkCamp.CrateHuangTa(info);
                    yield return new WaitForSeconds(interval);
                }

                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_huangta, 0.8f);
                break;
            case 15: //超能喷射  杀神领域
                Console.WriteLine("Color code for blue is #0000FF");
                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameAtkCamp.CrateLingYu(info);
                    yield return new WaitForSeconds(interval);
                }

                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_shashenlingyu, 0.8f);
                break;
            default:
                yield return new WaitForSeconds(interval);
                Console.WriteLine("Unknown color");
                break;
        }
    }
}