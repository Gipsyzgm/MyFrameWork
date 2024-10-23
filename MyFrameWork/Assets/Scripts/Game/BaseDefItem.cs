using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class BaseDefItem : MonoBehaviour
{
    [Header("通用基础数据")] public UserDataInfo Info; //用户信息
    public int level = 0; //當前等級
    public int price = 0; //送礼数量
    public int bulletMaxNum = 0; //子彈縂數量
    public float bulletInv = 0.2f; //子弹时间间隔
    public int NumberOfBullets = 1; // 每次子彈消耗數量
    public int bulletFreq = 6; // 每次循环发射几波
    public float SpreadAngle = 3f; // 总的散射角度范围
    public bool isDead = false;
    public float totalTime = 0; //计时标记
    public Vector3 direction = Vector3.forward; //子彈當前方向
    public float dirtime = 45; //物体持续时长
    public float icedebuff = 3;

    public bool IsAttack = true;
    public bool IsOnlyAtkNormalItem = false;

    [Header("基础组件")] public Text bulletNum;
    public GameObject headIcon;
    public GameObject headIconObject;
    public GameObject bulletPrefab;
    public List<GameObject> Items;
    public List<GameObject> toplevel;
    public GameObject ShowItem; //用于判断头像等朝向的物体
    public GameObject SpwnPos;
    public GameObject curUseItem;
    public Image timeSlider;
    public int AddBulletNum = 0;
    public int AddYuanCiShanNum = 0;

    public virtual void Init(UserDataInfo userData = null, int ID = 0)
    {
        Info = null;
        level = 0;
        price = 0;
        bulletMaxNum = 0;
        bulletInv = 0.125f;
        NumberOfBullets = 1;
        bulletFreq = 6;
        SpreadAngle = 0;
        isDead = false;
        if (userData != null)
        {
            Info = userData;
            headIconObject?.SetActive(true);
            if (headIcon != null)
            {
                ToolsHelper.SetWebImage(headIcon, userData.avatarUrl);
            }
        }
        else
        {
            headIconObject?.SetActive(false);
        }

        if (ID > 0)
        {
            var table = GiftEffectConfig.Get(ID);
            dirtime = table.time;
            AddBulletNum = table.num;
            //AddBullet(AddBulletNum);
            AddYuanCiShanNum = table.life;
            if (AddYuanCiShanNum > 0)
            {
                GameRootManager.Instance.gameDefCamp.GenerateYuanCiShan(userData, ID);
            }
        }
    }


    public virtual void AddBullet(int _num)
    {
        bulletMaxNum = bulletMaxNum + _num;
        RefreshUICount();
    }


    public virtual void RefreshUICount()
    {
        if (bulletNum != null)
        {
            if (bulletMaxNum <= 0)
            {
                bulletNum.text = "";
            }
            else
            {
                bulletNum.text = bulletMaxNum.ToString();
            }
        }
    }


    public virtual void CheckLvUp(UserDataInfo info = null)
    {
        if (info != null)
        {
            price = info.curprice;
        }

        CountLevel();
        //GameRootManager.Instance.gameDefCamp.UpDatePlayerInfo();
    }

    public virtual void CountLevel()
    {
        if (level == 24)
        {
            return;
        }

        var table = GradeInnerConfig.Get(level + 1);
        int needprice = table.needMoney;
        if (price >= needprice)
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

            Debug.Log("玩家名称：" + Info.nickname + "玩家目前总消费：" + Info.curprice);
            int toNextLevel;
            if (needprice1 - price > 0)
            {
                toNextLevel = needprice1 - price;
            }
            else
            {
                toNextLevel = 0;
            }

            if (Info != null)
            {
                EventMgr.Instance.InvokeEvent(EventConst.GameUserLevelup, Info.id, level, toNextLevel);
            }

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

    public virtual void ShowLvInfo()
    {
    }

    public virtual bool Getdir()
    {
        var GiftatkItems = GameObject.FindGameObjectsWithTag("AtkItem2");

        if (GiftatkItems.Length == 0 || IsOnlyAtkNormalItem)
        {
            var atkItems = GameObject.FindGameObjectsWithTag("AtkItem");

            if (atkItems.Length == 0)
            {
                return false;
            }

            GameObject closestCube = atkItems[0].gameObject;
            if (closestCube)
            {
                float minDist = Vector3.Distance(SpwnPos.transform.position, closestCube.transform.position);
                // 寻找最近Cube的逻辑
                foreach (var cube in atkItems)
                {
                    float dist = Vector3.Distance(SpwnPos.transform.position, cube.gameObject.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestCube = cube.gameObject;
                    }
                }

                if (ShowItem != null)
                {
                    var idealtargetpos = new Vector3(closestCube.transform.position.x, ShowItem.transform.position.y,
                        closestCube.transform.position.z);
                    ShowItem.transform.LookAt(idealtargetpos);
                }

                direction = closestCube.transform.position - SpwnPos.transform.position;
                direction.Normalize(); // 单位化方向向量
                if (IsObjectInBoxArea(closestCube.transform.position))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            GameObject closestCube = GiftatkItems[0].gameObject;
            if (closestCube)
            {
                BaseAtkItem atkItem = closestCube.GetComponent<BaseAtkItem>();

                float level = atkItem.AtkLevel;
                // 寻找最近Cube的逻辑
                foreach (var cube in GiftatkItems)
                {
                    BaseAtkItem dist = cube.GetComponent<BaseAtkItem>();
                    if (level < dist.AtkLevel)
                    {
                        level = dist.AtkLevel;
                        closestCube = dist.gameObject;
                    }
                }

                if (ShowItem != null)
                {
                    var idealtargetpos = new Vector3(closestCube.transform.position.x, ShowItem.transform.position.y,
                        closestCube.transform.position.z);
                    ShowItem.transform.LookAt(idealtargetpos);
                }

                direction = closestCube.transform.position - SpwnPos.transform.position;
                direction.Normalize(); // 单位化方向向量

                if (IsObjectInBoxArea(closestCube.transform.position))
                {
                    return true;
                }
                else
                {
                    DestroyImmediate(closestCube);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }


    private Vector3 minBounds = new Vector3(-20f, -5f, -11f);
    private Vector3 maxBounds = new Vector3(20f, 495f, 139f);


    public bool IsObjectInBoxArea(Vector3 position)
    {
        return position.x >= minBounds.x && position.x <= maxBounds.x &&
               position.y >= minBounds.y && position.y <= maxBounds.y &&
               position.z >= minBounds.z && position.z <= maxBounds.z;
    }


    private const string EftHitKey = "Prefab/xukongjingHit";
    private const string EftGroundKey = "Prefab/xukongjingdimian1";
    private const string EftJianSuKey = "Prefab/Ice_jiansu";

    public virtual void ShowICEGround()
    {
        var obj = Addressables.LoadAssetAsync<GameObject>(EftGroundKey).WaitForCompletion();
        if (obj != null)
        {
            GameObject newobj = Instantiate(obj);
            newobj.transform.position = SpwnPos.transform.position;
            newobj.SetActive(true);
            Destroy(newobj, 5f);
        }
    }

    public virtual void ShowICEEft()
    {
        var obj = Addressables.LoadAssetAsync<GameObject>(EftHitKey).WaitForCompletion();
        if (obj != null)
        {
            GameObject newobj = Instantiate(obj);
            newobj.transform.position = SpwnPos.transform.position;
            newobj.SetActive(true);
            Destroy(newobj, 5f);
        }
    }


    public virtual void ShowJianSuEft()
    {
        var obj = Addressables.LoadAssetAsync<GameObject>(EftJianSuKey).WaitForCompletion();
        if (obj != null)
        {
            GameObject newobj = Instantiate(obj);
            newobj.transform.position = gameObject.transform.position;
            newobj.SetActive(true);
            StartCoroutine(DestoryJianSuEft(newobj));
        }
    }

    public virtual IEnumerator DestoryJianSuEft(GameObject gameObject)
    {
        bulletInv = bulletInv * 2;
        yield return new WaitForSeconds(icedebuff + 2f);
        bulletInv = bulletInv / 2;
        Destroy(gameObject);
    }


    public void UnInit()
    {
        Info = null;
        level = 0;
        price = 0;
        bulletMaxNum = 0;
        bulletInv = 0.125f;
        NumberOfBullets = 1;
        bulletFreq = 6;
        SpreadAngle = 0;
        isDead = true;
        curUseItem.SetActive(false);
        transform.position = Vector3.down * 100;
    }
}