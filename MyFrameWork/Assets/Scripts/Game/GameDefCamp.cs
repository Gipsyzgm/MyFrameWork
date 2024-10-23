using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameDefCamp : MonoBehaviour
{
    public const string SoldierPoolKey = "Prefab/DefItem";
    public int maxSoldierCount = 100; //最大生成士兵数量
    public List<DefItem> DefItemList = new List<DefItem>();
    public float updateTargetInterval = 1; //更新士兵目标频率
    private float updateTargetTime; //更新士兵目标检测
    public List<GameObject> GiftItems;
    public Vector3 position;
    public YuanCiShanItem YuanCiShan;
    public bool IsYuanCiShanAlive = false;
    public long defPointPool;
    public int Gift7BuffLevel = 0;
    public float Gift7BuffCoef = 0;

    private void Awake()
    {
        EventMgr.Instance.AddEventListener(EventConst.GameOver, OnGameOver);

        var table = GiftEffectConfig.Get(14);
        Gift7BuffCoef = float.Parse(table.para) / 100f;
    }


    private void Start()
    {
    }

    public void UpdateCamp()
    {
    }

    public void Init(int campType)
    {
        GameRootManager.Instance.gameDefCamp = this;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="index"></param>
    /// <param name="gameKing"></param>
    /// <param name="enemyKing"></param>
    /// <param name="enemyCamp"></param>
    public void SetData()
    {
    }


    public IEnumerator GetGiftInfo(UserDataInfo info, int giftid, int giftcount)
    {
        DefItem defItem = null;
        foreach (var player in DefItemList)
        {
            if (info.id == player.Info.id)
            {
                defItem = player;
                break;
            }
        }

        if (defItem == null)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (var player in DefItemList)
            {
                if (info.id == player.Info.id)
                {
                    defItem = player;
                    defItem.CheckLvUp(info);
                    defItem.GetGiftInfo(info, giftid, giftcount);
                    break;
                }
            }
        }
        else
        {
            defItem.CheckLvUp(info);
            defItem.GetGiftInfo(info, giftid, giftcount);
        }
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("加入一個玩家")]
#endif
    private void GenerateSoldier()
    {
        GenerateSoldierById(null);
    }


    public void GenerateSoldierById(UserDataInfo soldierData)
    {
        DefItem gameSoldier;
        PoolMgr.Instance.SpawnGo(SoldierPoolKey,  (go) =>
        {
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(Random.Range(-9, 9), 1.5f, Random.Range(0, 15));
            gameSoldier = go.GetComponent<DefItem>();
            AddSoldier(gameSoldier);
            gameSoldier.Init(soldierData);
        });
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("加入一個青竹剑")]
#endif
    private void GenerateQingZhuJianItem()
    {
        GenerateQingZhuJian(null);
    }


    public void GenerateJianLun(UserDataInfo info, DefItem target)
    {
        GameObject item = Instantiate(GiftItems[6], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = target.transform.position;
        JianLunBullet bt = item.GetComponent<JianLunBullet>();
        bt.SetInfo(info);
        item.SetActive(true);
        StartCoroutine(FireJianLun(bt, info, target));
    }


    public void GenerateQingZhuJian(UserDataInfo soldierData, int ratio = 1)
    {
        Vector3 pos = new Vector3(Random.Range(-6, 6), 1.5f, -55f);
        GameObject item = Instantiate(GiftItems[0], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        QingZhuJianItem moGuanItem = item.GetComponent<QingZhuJianItem>();
        moGuanItem.Init(soldierData);
        moGuanItem.AtkBonus(ratio);
        item.SetActive(true);
    }

    public void GenerateHuoLian(UserDataInfo info)
    {
        Vector3 pos = new Vector3(Random.Range(-6, 6), 1.5f, -43f);
        GameObject item = Instantiate(GiftItems[3], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        HuoLian moGuanItem = item.GetComponent<HuoLian>();
        moGuanItem.Init(info);
        item.SetActive(true);
    }


    public void GenerateTianJian(UserDataInfo info)
    {
        Vector3 pos = new Vector3(Random.Range(-6, 6), 1.5f, -43f);
        GameObject item = Instantiate(GiftItems[4], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        JianZhenItem moGuanItem = item.GetComponent<JianZhenItem>();
        moGuanItem.Init(info);
        item.SetActive(true);
    }


    public void GenerateBingFeng(UserDataInfo info)
    {
        Vector3 pos = new Vector3(0, 1.5f, -42f);
        GameObject item = Instantiate(GiftItems[1], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        BingFengBullet bt = item.GetComponent<BingFengBullet>();
        bt.SetInfo(info);

        AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_fengming, gameObject, 0.3f);
        item.SetActive(true);
        StartCoroutine(FireBingFeng(bt, info));
        //bt.Move(info, Vector3.forward, 17, 8, 0);
    }

    IEnumerator FireJianLun(JianLunBullet bt, UserDataInfo info, DefItem target)
    {
        yield return new WaitUntil(() => GameRootManager.Instance.isPlay);
        // 使用Quaternion来旋转目标方向
        Quaternion rotation = Quaternion.Euler(0, Random.Range(-75, 75), 0); // 只绕Y轴旋转
        Vector3 direction = rotation * Vector3.forward;
        bt.Move(info, direction, 17, 20, 0);
    }

    IEnumerator FireBingFeng(BingFengBullet bt, UserDataInfo info)
    {
        yield return new WaitUntil(() => GameRootManager.Instance.isPlay);
        bt.Move(info, Vector3.forward, 17, 8, 0);
    }


    public void GenerateYuanCiShan(UserDataInfo info, int ID)
    {
        if (IsYuanCiShanAlive)
        {
            var table = GiftEffectConfig.Get(ID);
            int hp = table.life;
            YuanCiShan.SetHp(hp);
        }
        else
        {
            if (YuanCiShan == null)
            {
                Vector3 pos = new Vector3(0, 5.2f, -12.5f);
                GameObject item = Instantiate(GiftItems[2], transform);
                item.transform.parent = GameRootManager.Instance.GameObjectPar;
                item.transform.position = pos;
                YuanCiShan = item.GetComponent<YuanCiShanItem>();
            }

            YuanCiShan.Init(info, ID);
        }
    }


    public void GenerateZhangTianPing(UserDataInfo info)
    {
        Vector3 pos = new Vector3(0, 5.2f, -28f);
        GameObject item = Instantiate(GiftItems[5], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        ZhangTianPingItem ztp = item.GetComponent<ZhangTianPingItem>();
        ztp.Init(info);
    }


    public void HitYuanCiShan(int hp = -1000)
    {
        if (IsYuanCiShanAlive)
        {
            YuanCiShan.SetHp(hp);
        }
    }


    public void AddSoldier(DefItem soldier)
    {
        if (!DefItemList.Contains(soldier))
        {
            DefItemList.Add(soldier);
        }
    }

    public void RemoveSoldier(DefItem soldier)
    {
        soldier.UnInit();
        PoolMgr.Instance.DisSpawnGo(SoldierPoolKey, soldier.gameObject);
    }


    private void OnGameOver(params object[] args)
    {
        
    }

    public int CountScore()
    {
        int score = 0;
        foreach (DefItem soldier in DefItemList)
        {
            // if (soldier.IsAlive())
            // {
            //     score += soldier.GetScore();
            // }
        }

        return score;
    }


    public void Reset()
    {
        Gift7BuffLevel = 0;
        IsYuanCiShanAlive = false;
        foreach (DefItem soldier in DefItemList)
        {
            soldier.UnInit();
            PoolMgr.Instance.DisSpawnGo(SoldierPoolKey, soldier.gameObject);
        }

        DefItemList.Clear();
    }

    public void Dispose()
    {
        foreach (DefItem soldier in DefItemList)
        {
            soldier.UnInit();
            PoolMgr.Instance.DisSpawnGo(SoldierPoolKey, soldier.gameObject);
        }

        DefItemList.Clear();
        DestroyImmediate(gameObject);
    }

    public void UnloadYuanCiShan()
    {
        IsYuanCiShanAlive = false;
        YuanCiShan.gameObject.SetActive(false);
    }


    /// <summary>
    /// 更新付费玩家位置
    /// </summary>
    public void UpDatePlayerInfo()
    {
        DefItemList.Sort((x, y) => { return y.price.CompareTo((int)x.price); });

        //GameMap.Instance.SetTopThreeInfo(DefItemList);
    }
}