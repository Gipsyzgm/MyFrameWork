using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DefItem : BaseDefItem
{
    public List<GameObject> eftJianQi;
    public GameObject CuruseeftJianQi;
    private Coroutine myCoroutine;

    public override void Init(UserDataInfo userData, int hp = 0)
    {
        base.Init(userData, hp);
        CheckLvUp(userData);
        RefreshUICount();
        InvokeRepeating("CheckPos", 10, 10);
    }

    public override void ShowLvInfo()
    {
        float XZ = 0.9f;
        float Y = 0.9f;
        if (level > 0)
        {
            var table = GradeInnerConfig.Get(level);
            XZ = table.xCoeffi;
            Y = table.yCoeffi;
            string shootPara = table.shootPara;
            string[] info = shootPara.Split(";");
            NumberOfBullets = int.Parse(info[0]); // 每次子彈消耗數量
            bulletFreq = int.Parse(info[1]); // 每次循环发射几波
            SpreadAngle = table.spreadAngle;
            bulletInv = table.frequency;
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

        int bgindex = (int)Mathf.Ceil(level / 3f);
        if (curUseItem != null)
        {
            curUseItem.SetActive(false);
            curUseItem = Items[bgindex];
            if (bgindex > 0)
            {
                if (CuruseeftJianQi != null)
                {
                    CuruseeftJianQi.SetActive(false);
                }

                CuruseeftJianQi = eftJianQi[bgindex - 1];
            }

            curUseItem.SetActive(true);
            transform.DOScaleX(1 * (XZ), 0.1f);
            transform.DOScaleY(1 * (Y), 0.1f);
            transform.DOScaleZ(1 * (XZ), 0.1f);
        }
        else
        {
            curUseItem = Items[bgindex];
            if (bgindex > 0)
            {
                if (CuruseeftJianQi != null)
                {
                    CuruseeftJianQi.SetActive(false);
                }

                CuruseeftJianQi = eftJianQi[bgindex - 1];
            }

            curUseItem.SetActive(true);
            transform.DOScaleX(1 * (XZ), 0.1f);
            transform.DOScaleY(1 * (Y), 0.1f);
            transform.DOScaleZ(1 * (XZ), 0.1f);
        }
    }

    void Update()
    {
        if (GameRootManager.Instance.isPlay && GameRootManager.Instance.IsEnterReady == false)
        {
            if (IsAttack)
            {
                DoAttack(); // 尝试射击
            }
        }
    }

    public float rayLength = 3.0f;

    void CheckPos()
    {
        if (GameRootManager.Instance.isPlay && GameRootManager.Instance.IsEnterReady == false)
        {
            for (int i = 0; i < 6; i++)
            {
                // 计算射线的方向
                Vector3 direction =
                    DirectionFromAngle(Random.Range(1, 12) * 30, transform.position, transform.rotation);
                // 发射射线
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, rayLength))
                {
                    // 如果检测到物体
                    Debug.Log($"Detected object: {hit.collider.name} at distance: {hit.distance}");

                    // 可以在这里添加更多的处理逻辑
                }
                else
                {
                    StartCoroutine(ApplyForceOverTime(direction));
                    break;
                }
            }
        }
    }

    Vector3 DirectionFromAngle(float angle, Vector3 origin, Quaternion rotation)
    {
        // 将角度转换为弧度
        float radian = angle * Mathf.Deg2Rad;
        // 计算方向向量
        Vector3 direction = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
        // 应用旋转
        return rotation * direction;
    }

    IEnumerator ApplyForceOverTime(Vector3 dir)
    {
        bool stop = false;
        float elapsedTime = 0.0f;
        Debug.DrawRay(transform.position, dir * rayLength, Color.red);
        while (elapsedTime < 2 && stop == false)
        {
            // 应用力
            transform.Translate(dir * 6.6f * Time.deltaTime);
            // 等待一段时间
            yield return new WaitForSeconds(0.02f);
            // 更新已用时间
            elapsedTime += 0.02f;

            Debug.Log("位置" + transform.position);
            if (transform.position.x < -10 || transform.position.x > 10 || transform.position.z < -45 ||
                transform.position.z > -23.5f)
            {
                stop = true;
            }
        }
    }

    void DoAttack()
    {
        // 计算方向
        if (bulletMaxNum > 0)
        {
            totalTime += Time.deltaTime;
            if (totalTime >= bulletInv)
            {
                FireBullet();
                totalTime = 0; // 更新上次发射时间
            }
        }
        else
        {
            totalTime += Time.deltaTime;
            if (totalTime >= 2)
            {
                StartCoroutine(FireBullets());
                totalTime = 0; // 更新上次发射时间
            }
        }
    }

    private void FireBullet()
    {
        if (Getdir())
        {
            bulletMaxNum = bulletMaxNum - NumberOfBullets;
            if (bulletMaxNum < 0)
            {
                bulletMaxNum = 0;
            }

            RefreshUICount();
            Shoot();
        }
    }

    private void Shoot()
    {
        // 获取玩家当前的方向
        //Vector3 targetDirection = ShowItem.transform.forward;
        Vector3 targetDirection = direction;
        float angleStep = 0;
        if (NumberOfBullets > 1)
        {
            // 计算单个子弹之间的角度差
            angleStep = SpreadAngle / (NumberOfBullets - 1);
        }

        for (int i = 0; i < NumberOfBullets; i++)
        {
            // 计算当前子弹的角度偏移
            float angleOffset = angleStep * i - SpreadAngle / 2;
            // 将角度转换为方向向量
            Vector3 newdirection = CalculateDirection(targetDirection, angleOffset);
            GameObject bullet = Instantiate(bulletPrefab, SpwnPos.transform);
            bullet.transform.parent = null;
            bullet.transform.localScale = new Vector3(1, 1, 1);
            Bullet bt = bullet.GetComponent<Bullet>();
            bt.Move(Info, newdirection, 35, 4, level);
        }
    }


    private const string BulletKey = "Prefab/Bullet";

    public void CrateBullet(UserDataInfo userData, Vector3 newdirection, int level)
    {
        PoolMgr.Instance.SpawnGo(BulletKey, (go) =>
        {
            go.transform.parent = transform;
            go.transform.position = SpwnPos.transform.position;
            go.transform.localScale = new Vector3(1, 1, 1);
            Bullet bt = go.GetComponent<Bullet>();
            bt.Move(Info, newdirection, 17, 2, level);
        });
    }


    IEnumerator FireBullets()
    {
        for (int i = 0; i < bulletFreq; i++)
        {
            yield return new WaitForSeconds(0.1f);
            FireBullet();
        }
    }


    Vector3 CalculateDirection(Vector3 targetDirection, float angleOffset)
    {
        // 使用Quaternion来旋转目标方向
        Quaternion rotation = Quaternion.Euler(0, angleOffset, 0); // 只绕Y轴旋转
        Vector3 direction = rotation * targetDirection;
        return direction;
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("发射散弹")]
#endif
    private void ShootTest()
    {
        Shoot();
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("增加子弹")]
#endif
    private void AddSomeBullet()
    {
        AddBullet(100);
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("升級")]
#endif
    public void AddLevel()
    {
        if (level < 24)
        {
            var table = GradeInnerConfig.Get(level + 1);
            int needprice = table.needMoney;
            price = needprice;
            CheckLvUp();
        }
    }


    // public void GetGiftInfo(UserDataInfo info, int giftid, int giftCount)
    // {
    //     switch (giftid)
    //     {
    //         case 21: //仙女棒
    //             StartCoroutine(ShowEftJianQi(giftCount));
    //             AudioMgr.Instance.PlayEffect("voice/liwu_jianqi", 0.8f);
    //             break;
    //
    //         case 10: //能力药丸
    //
    //             if (giftCount == 66)
    //             {
    //                 for (int i = 0; i < 11; i++)
    //                 {
    //                     GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 6);
    //                 }
    //             }
    //             else if (giftCount == 188)
    //             {
    //                 for (int i = 0; i < 11; i++)
    //                 {
    //                     GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 17);
    //                 }
    //             }
    //             else if (giftCount == 520)
    //             {
    //                 for (int i = 0; i < 15; i++)
    //                 {
    //                     GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 35);
    //                 }
    //             }
    //             else if (giftCount == 999)
    //             {
    //                 for (int i = 0; i < 20; i++)
    //                 {
    //                     GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 50);
    //                 }
    //             }
    //             else if (giftCount == 1314)
    //             {
    //                 for (int i = 0; i < 20; i++)
    //                 {
    //                     GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 66);
    //                 }
    //             }
    //             else
    //             {
    //                 for (int i = 0; i < giftCount; i++)
    //                 {
    //                     GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info);
    //                 }
    //             }
    //
    //             AddBulletByID(9, giftCount);
    //
    //             AudioMgr.Instance.PlayEffect("voice/liwu_qingzhujian", 0.8f);
    //             break;
    //         case 11: //甜甜圈
    //             StartCoroutine(ShowBingFeng(info, giftCount));
    //             AudioMgr.Instance.PlayEffect("voice/liwu_bingfeng", 0.8f);
    //             break;
    //         case 25: //能量电池
    //
    //             for (int i = 0; i < giftCount; i++)
    //             {
    //                 GameRootManager.Instance.gameDefCamp.GenerateYuanCiShan(info, 11);
    //             }
    //             AddBulletByID(11, giftCount);
    //             AudioMgr.Instance.PlayEffect("voice/liwu_wujishan", 0.8f);
    //             break;
    //         case 23: //爱的爆炸
    //             for (int i = 0; i < giftCount; i++)
    //             {
    //                 GameRootManager.Instance.gameDefCamp.GenerateHuoLian(info);
    //             }
    //             AddBulletByID(12, giftCount);
    //             AudioMgr.Instance.PlayEffect("voice/liwu_huolian", 0.8f);
    //             break;
    //         case 8: //神秘空投
    //
    //             for (int i = 0; i < giftCount; i++)
    //             {
    //                 GameRootManager.Instance.gameDefCamp.GenerateTianJian(info);
    //             }
    //             AddBulletByID(13, giftCount);
    //             AudioMgr.Instance.PlayEffect("voice/liwu_jianzhen", 0.8f);
    //             break;
    //         case 15: //超能喷射
    //             for (int i = 0; i < giftCount; i++)
    //             {
    //                 GameRootManager.Instance.gameDefCamp.GenerateZhangTianPing(info);
    //             }
    //             AddBulletByID(14, giftCount);
    //             AudioMgr.Instance.PlayEffect("voice/liwu_zhangtianping", 0.8f);
    //             break;
    //         default:
    //             Debug.Log("Unknown Gift Code");
    //             break;
    //     }
    // }

    public void GetGiftInfo(UserDataInfo info, int giftid, int giftCount)
    {
        StartCoroutine(CrateGiftItem(info, giftid, giftCount, 0.1f));
        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
            myCoroutine = null; // 清理引用
        }

        myCoroutine = StartCoroutine(ShowJianQi());
    }

    IEnumerator CrateGiftItem(UserDataInfo info, int giftid, int giftCount, float interval)
    {
        switch (giftid)
        {
            case 21: //仙女棒
                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameDefCamp.GenerateJianLun(info, this);
                    yield return new WaitForSeconds(0.2f);
                }

                // if (AudioMgr.Instance.isJianQiGenerate)
                // {
                //     AudioMgr.Instance.isJianQiGenerate = false;
                //     AudioMgr.Instance.PlayEffectOnTarget("sound/gs_jianqi2", CuruseeftJianQi, 0.4f, AudioMgr.Instance.StopJianQiGenerate,
                //         false);
                // }
                AddBulletByID(8, giftCount);
                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_jianqi, 0.8f);
                break;

            case 10: //能力药丸

                if (giftCount == 66)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 6);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (giftCount == 188)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 17);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (giftCount == 520)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 35);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (giftCount == 999)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 50);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (giftCount == 1314)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info, 66);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else
                {
                    for (int i = 0; i < giftCount; i++)
                    {
                        GameRootManager.Instance.gameDefCamp.GenerateQingZhuJian(info);
                        yield return new WaitForSeconds(0.2f);
                    }
                }

                AddBulletByID(9, giftCount);
                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_qingzhujian, 0.8f);
                break;
            case 11: //甜甜圈
                for (int i = 0; i < giftCount; i++)
                {
                    AddBullet(2000);
                    GameRootManager.Instance.gameDefCamp.GenerateYuanCiShan(info, 10);
                    GameRootManager.Instance.gameDefCamp.GenerateBingFeng(info);
                    yield return new WaitForSeconds(0.5f);
                }

                AddBulletByID(10, giftCount);
                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_bingfeng, 0.8f);
                break;
            case 25: //能量电池

                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameDefCamp.GenerateYuanCiShan(info, 11);
                    yield return new WaitForSeconds(0.1f);
                }

                AddBulletByID(11, giftCount);
                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_wujishan, 0.8f);
                break;
            case 23: //爱的爆炸
                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameDefCamp.GenerateHuoLian(info);
                    yield return new WaitForSeconds(0.5f);
                }

                AddBulletByID(12, giftCount);
                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_huolian, 0.8f);
                break;
            case 8: //神秘空投

                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameDefCamp.GenerateTianJian(info);
                    yield return new WaitForSeconds(0.5f);
                }

                AddBulletByID(13, giftCount);
                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_jianzhen, 0.8f);
                break;
            case 15: //超能喷射
                for (int i = 0; i < giftCount; i++)
                {
                    GameRootManager.Instance.gameDefCamp.GenerateZhangTianPing(info);
                    yield return new WaitForSeconds(0.5f);
                }

                AddBulletByID(14, giftCount);
                AudioMgr.Instance.PlayEffect(MyAudioName.liwu_zhangtianping, 0.8f);
                break;
            default:
                Debug.Log("Unknown Gift Code");
                break;
        }
    }


    IEnumerator ShowBingFeng(UserDataInfo info, int giftCount)
    {
        for (int i = 0; i < giftCount; i++)
        {
            AddBullet(2000);
            GameRootManager.Instance.gameDefCamp.GenerateYuanCiShan(info, 10);
            GameRootManager.Instance.gameDefCamp.GenerateBingFeng(info);
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void AddBulletByID(int ID, int giftCount)
    {
        if (ID > 0)
        {
            var table = GiftEffectConfig.Get(ID);
            AddBulletNum = table.num;
            AddBullet(AddBulletNum * giftCount);
        }
    }


    IEnumerator ShowJianQi()
    {
        if (CuruseeftJianQi != null)
        {
            if (CuruseeftJianQi.activeSelf == true)
            {
                CuruseeftJianQi.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }

            CuruseeftJianQi.SetActive(true);
            yield return new WaitForSeconds(30f);
            CuruseeftJianQi.SetActive(false);
        }
    }
}