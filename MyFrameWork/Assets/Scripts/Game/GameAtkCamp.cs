using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameAtkCamp : MonoBehaviour
{
    public const string SoldierPoolKey = "Prefab/AtkItem";
    public const string DaShenShiKey = "Prefab/dafangkuai";
    public const string MoGuanKey = "Prefab/moguan";
    public const string XuKongJingKey = "Prefab/xukonjing";
    public const string MuQingDingKey = "Prefab/muqiding";
    public const string HuangTaKey = "Prefab/HuangTa";
    public const string PlayerPoolKey = "Prefab/GameAtkPlayer";
    public List<GameObject> SpwanPos;
    public List<GameObject> PlayerSpwanPos;
    public GameObject PlayerDefaultPos;
    public GameObject GiftItemSpwanPos;
    public CampType campType; //阵营类型
    public List<GameAtkPlayer> AtkPlayerList = new List<GameAtkPlayer>();
    public List<BaseAtkItem> AtkItemList = new List<BaseAtkItem>();
    public long atkPointPool;
    public List<GameObject> GiftItems;
    [Header("每秒生成数量")] public int SpwanNum = 2;
    [Header("生成频率")] public float generateSoldierInterval = 0.5f;
    [Header("当前玩家数量")] public int playercount = 0;
    [Header("生成方块上限")] public int MaxAtkNum = 500;
    [Header("当前方块数量")] public int CurActiveNum = 0;

    [Header("生成打神石上限")] public int MaxDaShenShiNum = 50;
    [Header("当前打神石数量")] public int CurDaShenShiNum = 0;
    public List<DaFangKuaiItem> DaShenShiList = new List<DaFangKuaiItem>();
    [Header("生成魔罐上限")] public int MaxMoGuanNum = 40;
    public List<MoGuanItem> MoGuanList = new List<MoGuanItem>();
    [Header("当前魔罐数量")] public int CurMoGuanNum = 0;

    public bool StartSpwan = false;

    //计时
    private float totalTime;

    public int Gift7BuffLevel = 0;

    public float Gift7BuffAddNum = 0.3f;

    private int shengnvluposindex = 0;

    private void Awake()
    {
        EventMgr.Instance.AddEventListener(EventConst.GameOver, OnGameOver);
    }


    private void Start()
    {
    }

    void Update()
    {
        if (GameRootManager.Instance.isPlay)
        {
            if (StartSpwan)
            {
                if (CurActiveNum < MaxAtkNum)
                {
                    totalTime += Time.deltaTime;
                    if (totalTime >= generateSoldierInterval)
                    {
                        if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 400)
                        {
                            SpwanNum = 5;
                        }
                        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 500)
                        {
                            SpwanNum = 4;
                        }
                        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 600)
                        {
                            SpwanNum = 3;
                        }
                        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 700)
                        {
                            SpwanNum = 2;
                        }
                        else if (GameRootManager.Instance.gameAtkCamp.CurActiveNum <= 800)
                        {
                            SpwanNum = 1;
                        }
                        else
                        {
                            SpwanNum = 0;
                        }

                        if (SpwanNum > 0)
                        {
                            for (int i = 0; i < SpwanNum; i++)
                            {
                                GenerateSoldier();
                            }
                        }

                        totalTime = 0; // 更新上次发射时间
                    }
                }
            }
        }
    }

    public void Init(int campType)
    {
        GameRootManager.Instance.gameAtkCamp = this;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData()
    {
    }

    /// <summary>
    /// 检查是否升级
    /// </summary>
    /// <param name="info"></param>
    public IEnumerator GetGiftInfo(UserDataInfo info, int giftid, int giftcount)
    {
        GameAtkPlayer atkPlayer = null;
        //这个位置可能出问题在于我还在遍历list的时候，那边已经排序了，现在break没问题。
        //也可以先遍历找出对应的player,然后再检查升级就不会出问题。
        foreach (var player in AtkPlayerList)
        {
            if (info.id == player.info.id)
            {
                atkPlayer = player;
                break;
            }
        }

        if (atkPlayer == null)
        {
            yield return new WaitForSeconds(0.5f);

            foreach (var player in AtkPlayerList)
            {
                if (info.id == player.info.id)
                {
                    atkPlayer = player;
                    atkPlayer.CheckLvUp(info);
                    atkPlayer.GetGiftInfo(info, giftid, giftcount);
                    break;
                }
            }
        }
        else
        {
            atkPlayer.CheckLvUp(info);
            atkPlayer.GetGiftInfo(info, giftid, giftcount);
        }
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("开始/停止生成士兵")]
#endif
    public void StartGenerateSoldier()
    {
        if (StartSpwan)
        {
            StartSpwan = false;
        }
        else
        {
            StartSpwan = true;
        }
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("生成普通士兵")]
#endif
    private void GenerateSoldier()
    {
        GenerateSoldierById();
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("生成超级士兵")]
#endif
    private void GenerateSuperSoldier()
    {
        CrateSuperSoldier();
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("生成魔罐")]
#endif
    private void GenerateMoGuanGiftItem()
    {
        GenerateMoGuan(null);
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("生成圣女炉")]
#endif
    private void GenerateShengNvGiftItem()
    {
        GenerateShengNv(null);
    }


    public void GenerateMoGuan(UserDataInfo info, int ID = 2, int num = 1, float scale = 0.6f, bool canspwan = true,
        Vector3 oripos = default, float ratio = 1f)
    {
        Vector3 pos;
        if (oripos != default)
        {
            pos = oripos;
        }
        else
        {
            GameObject target = GameMap.Instance.GetBestPos(GameMap.Instance.CheckGameObject2);
            pos = new Vector3(target.transform.position.x, 40, target.transform.position.z);
            // pos = new Vector3(Random.Range(-8f, 8f), GiftItemSpwanPos.transform.position.y,
            //     GiftItemSpwanPos.transform.position.z);
        }

        for (int i = 0; i < num; i++)
        {
            PoolMgr.Instance.SpawnGo(MoGuanKey, (go) =>
            {
                go.transform.parent = transform;
                go.transform.position = pos;
                go.transform.localScale = Vector3.one * scale;
                MoGuanItem gameSoldier = go.GetComponent<MoGuanItem>();
                AddMoGuan(gameSoldier);
                gameSoldier.Init(info, ID, canspwan);
                gameSoldier.Healthbonus(ratio);
                go.SetActive(true);
            });
        }
    }


    public void AddMoGuan(MoGuanItem soldier)
    {
        CurMoGuanNum++;
        if (!MoGuanList.Contains(soldier))
        {
            MoGuanList.Add(soldier);
        }
    }

    public void RemoveMoGuan(MoGuanItem soldier)
    {
        if (soldier == null)
        {
            return;
        }

        if (soldier.gameObject == null)
        {
            return;
        }

        CurMoGuanNum--;
        PoolMgr.Instance.DisSpawnGo(MoGuanKey, soldier.gameObject);
    }


    public void GenerateMuQiDing(UserDataInfo info, int ID = 5, int num = 1, float scale = 1, bool canspwan = false,
        Vector3 oripos = default)
    {
        // Vector3 pos = new Vector3(Random.Range(-8f, 8f), GiftItemSpwanPos.transform.position.y,
        //     GiftItemSpwanPos.transform.position.z);
        Vector3 pos;
        if (oripos != default)
        {
            pos = oripos;
        }
        else
        {
            GameObject target = GameMap.Instance.GetBestPos(GameMap.Instance.CheckGameObject2);
            pos = new Vector3(target.transform.position.x, 40, target.transform.position.z);
        }

        for (int i = 0; i < num; i++)
        {
            GameObject item = Instantiate(GiftItems[5], transform);
            item.transform.parent = GameRootManager.Instance.GameObjectPar;
            item.transform.position = pos;
            item.transform.localScale = Vector3.one * scale;
            MuQiDingItem moGuanItem = item.GetComponent<MuQiDingItem>();
            moGuanItem.Init(info, ID, canspwan);
            item.SetActive(true);
        }
    }


    public void GenerateShengNv(UserDataInfo info, int ID = 4)
    {
        // Vector3 pos = new Vector3(Random.Range(-8f, 8f), GiftItemSpwanPos.transform.position.y,
        //     GiftItemSpwanPos.transform.position.z - 6);


        GameObject target = GameMap.Instance.CheckGameObject3[shengnvluposindex];
        //GameObject target =  GameMap.Instance.GetBestPos(GameMap.Instance.CheckGameObject3);
        Vector3 pos = new Vector3(target.transform.position.x, 1.5f, target.transform.position.z);

        GameObject item = Instantiate(GiftItems[1], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        ShengNvItem moGuanItem = item.GetComponent<ShengNvItem>();
        moGuanItem.Init(info, ID);
        item.SetActive(true);
        shengnvluposindex++;
        if (shengnvluposindex > 2)
        {
            shengnvluposindex = 0;
        }
    }


    public void GenerateXuKongJing(UserDataInfo info, int ID = 3)
    {
        // Vector3 pos = new Vector3(Random.Range(-8f, 8f), GiftItemSpwanPos.transform.position.y,
        //     GiftItemSpwanPos.transform.position.z - 6);
        GameObject target = GameMap.Instance.GetBestPos(GameMap.Instance.CheckGameObject2);
        Vector3 pos = new Vector3(target.transform.position.x, 40, target.transform.position.z);


        GameObject item = Instantiate(GiftItems[3], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        XuKongJingItem moGuanItem = item.GetComponent<XuKongJingItem>();
        moGuanItem.Init(info, ID);
        item.SetActive(true);
    }


    /// <summary>
    /// 新增一个玩家
    /// </summary>
    /// <param name="info"></param>
    public void AddNewSoldier(UserDataInfo info)
    {
        playercount++;
        Vector3 spwanPos;
        if (playercount > 25)
        {
            spwanPos = PlayerDefaultPos.transform.position;
        }
        else
        {
            spwanPos = PlayerSpwanPos[playercount - 1].transform.position;
        }

        PoolMgr.Instance.SpawnGo(PlayerPoolKey, (go) =>
        {
            go.transform.parent = transform;
            go.transform.position = spwanPos;
            go.SetActive(true);
            GameAtkPlayer gameSoldier = go.GetComponent<GameAtkPlayer>();
            AddPlayer(gameSoldier);
            gameSoldier.Init(info);
        });
    }

    /// <summary>
    /// 生成某玩家的普通士兵
    /// </summary>
    /// <param name="info"></param>
    public void GenerateSoldierByPlayer(GameAtkPlayer atkPlayer = null, int ID = 0, int type = 0, int hp = 0)
    {
        GenerateSoldierById(atkPlayer, ID, type, hp);
    }


    /// <summary>
    /// 生成超级士兵
    /// </summary>
    /// <param name="info"></param>
    public void CrateSuperSoldier(UserDataInfo info = null, int ID = 1, float ratio = 1f)
    {
        Vector3 pos = new Vector3(Random.Range(-10.5f, 10.5f), GiftItemSpwanPos.transform.position.y,
            Random.Range(-2, 40));
        PoolMgr.Instance.SpawnGo(DaShenShiKey,(go) =>
        {
            go.transform.parent = transform;
            go.transform.position = pos;
            var gameSoldier = go.GetComponent<DaFangKuaiItem>();
            AddDaShenShi(gameSoldier);
            gameSoldier.Init(info, ID);
            gameSoldier.Healthbonus(ratio);
            go.SetActive(true);
        });

        // GameObject item = Instantiate(GiftItems[2], transform);
        // item.transform.parent = GameRootManager.Instance.GameObjectPar;
        // item.transform.position = pos;
        // var moGuanItem = item.GetComponent<DaFangKuaiItem>();
        // AddDaShenShi(moGuanItem);
        // moGuanItem.Init(info, ID);
        // item.SetActive(true);
    }

    public void AddDaShenShi(DaFangKuaiItem soldier)
    {
        CurDaShenShiNum++;
        if (!DaShenShiList.Contains(soldier))
        {
            DaShenShiList.Add(soldier);
        }
    }

    public void RemoveDaShenShi(DaFangKuaiItem soldier)
    {
        if (soldier == null)
        {
            return;
        }

        if (soldier.gameObject == null)
        {
            return;
        }

        CurDaShenShiNum--;
        PoolMgr.Instance.DisSpawnGo(DaShenShiKey, soldier.gameObject);
    }


    /// <summary>
    /// 生成荒塔
    /// </summary>
    /// <param name="info"></param>
    public void CrateHuangTa(UserDataInfo info = null, int ID = 6)
    {
        // Vector3 pos = new Vector3(Random.Range(-8f, 8f), GiftItemSpwanPos.transform.position.y,
        //     GiftItemSpwanPos.transform.position.z);

        GameObject target = GameMap.Instance.GetBestPos(GameMap.Instance.CheckGameObject1);
        Vector3 pos = new Vector3(target.transform.position.x, 40, target.transform.position.z);
        GameObject item = Instantiate(GiftItems[4], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        var moGuanItem = item.GetComponent<HuangTaItem>();
        moGuanItem.Init(info, ID);
        item.SetActive(true);
    }


    public void CrateLingYu(UserDataInfo info = null, int ID = 7)
    {
        Vector3 pos = new Vector3(0, 5.2f, -15f);
        GameObject item = Instantiate(GiftItems[6], transform);
        item.transform.parent = GameRootManager.Instance.GameObjectPar;
        item.transform.position = pos;
        var moGuanItem = item.GetComponent<LingYuItem>();
        moGuanItem.Init(info, ID);
        item.SetActive(true);
    }

    /// <summary>
    /// 普通生成
    /// </summary>
    private void GenerateSoldierById(GameAtkPlayer info = null, int ID = 0, int type = 0, int Hp = 0)
    {
        int posindex = Random.Range(1, 13);
        if (posindex > SpwanPos.Count)
        {
            posindex = 1;
        }

        Vector3 pos = SpwanPos[posindex].transform.position;
        if (info == null)
        {
            PoolMgr.Instance.SpawnGo(SoldierPoolKey, (go) =>
            {
                go.transform.parent = transform;
                go.transform.position = pos;
                go.transform.localRotation = Quaternion.identity;
                go.SetActive(true);
                var gameSoldier = go.GetComponent<AtkItem>();
                AddSoldier(gameSoldier);
                gameSoldier.Init();
            });
        }
        else
        {
            if (info.price > 0 && info.transform.position.y < 18 && type == 0)
            {
                pos = info.transform.position;
            }
            
            GameObject go = PoolMgr.Instance.SpawnGo(SoldierPoolKey);
            go.transform.parent = null;
            go.transform.position = pos;
            go.transform.localRotation = Quaternion.identity;
            go.SetActive(true);
            var gameSoldier = go.GetComponent<AtkItem>();
            AddSoldier(gameSoldier);
            gameSoldier.level = info.level;
            gameSoldier.Init(info.info, ID, false, 1f, Hp);
        }
    }


    public void AddPlayer(GameAtkPlayer soldier)
    {
        if (!AtkPlayerList.Contains(soldier))
        {
            AtkPlayerList.Add(soldier);
        }
    }

    public void RemovePlayer(GameAtkPlayer soldier)
    {
        soldier.UnInit();
        PoolMgr.Instance.DisSpawnGo(PlayerPoolKey, soldier.gameObject);
    }

    public void AddSoldier(BaseAtkItem soldier)
    {
        CurActiveNum++;
        if (!AtkItemList.Contains(soldier))
        {
            AtkItemList.Add(soldier);
        }
    }

    public void RemoveSoldier(BaseAtkItem soldier)
    {
        if (soldier == null)
        {
            return;
        }

        if (soldier.gameObject == null)
        {
            return;
        }

        CurActiveNum--;
        soldier.UnInit();
        PoolMgr.Instance.DisSpawnGo(SoldierPoolKey, soldier.gameObject);
    }

    private void OnGameOver(params object[] arg)
    {
    }


    public void Reset()
    {
        Gift7BuffLevel = 0;
        CurActiveNum = 0;
        CurDaShenShiNum = 0;
        CurMoGuanNum = 0;
        foreach (BaseAtkItem soldier in AtkItemList)
        {
            RemoveSoldier(soldier);
        }

        AtkItemList.Clear();
        foreach (DaFangKuaiItem shenshi in DaShenShiList)
        {
            RemoveDaShenShi(shenshi);
        }

        DaShenShiList.Clear();

        foreach (MoGuanItem moGuan in MoGuanList)
        {
            RemoveMoGuan(moGuan);
        }

        MoGuanList.Clear();


        foreach (GameAtkPlayer atkPlayer in AtkPlayerList)
        {
            atkPlayer.UnInit();
            PoolMgr.Instance.DisSpawnGo(PlayerPoolKey, atkPlayer.gameObject);
        }

        AtkPlayerList.Clear();
    }


    public void Dispose()
    {
        foreach (BaseAtkItem soldier in AtkItemList)
        {
            soldier.UnInit();
            PoolMgr.Instance.DisSpawnGo(SoldierPoolKey, soldier.gameObject);
        }

        AtkItemList.Clear();

        foreach (GameAtkPlayer atkPlayer in AtkPlayerList)
        {
            atkPlayer.UnInit();
            PoolMgr.Instance.DisSpawnGo(PlayerPoolKey, atkPlayer.gameObject);
        }

        AtkPlayerList.Clear();

        DestroyImmediate(gameObject);
    }


    /// <summary>
    /// 更新付费玩家位置
    /// </summary>
    public void UpDatePlayerInfo()
    {
        AtkPlayerList.Sort((x, y) => { return y.price.CompareTo((int)x.price); });

        for (int j = 0; j < AtkPlayerList.Count; j++)
        {
            if (j < 25)
            {
                if (j == 0)
                {
                    AtkPlayerList[j].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                }
                else if (j == 1 || j == 2)
                {
                    AtkPlayerList[j].transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
                }
                else
                {
                    AtkPlayerList[j].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }

                AtkPlayerList[j].transform.position = PlayerSpwanPos[j].transform.position;
            }
            else
            {
                AtkPlayerList[j].transform.position = PlayerDefaultPos.transform.position;
            }
        }
    }
}