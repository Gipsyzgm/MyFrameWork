using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedTurret : BaseDefItem
{
    public GameObject shooteft;
    
    public bool Isfirst = false;
    
    private int BulletHp = 3;

    private void Start()
    {
        bulletInv = 1f;
        SetInfo(null);
    }


    public void SetInfo(UserDataInfo userData)
    {
        if (userData != null)
        {
            bulletNum.text = userData.nickname;
            CheckLvUp(userData);
        }
        else
        {
            bulletNum.text = "";
        }
    }


    public override void CheckLvUp(UserDataInfo info = null)
    {
        price = info.curprice;
        CountLevel();
    }

    public override void CountLevel()
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
            if (level >= 24)
            {
                level = 24;
                ShowLvInfo();
            }
            else
            {
                CountLevel();
            }
        }
        else
        {
            ShowLvInfo();
        }
    }


    public override void ShowLvInfo()
    {
        if (Isfirst)
        {
            BulletHp = 1 + level;
        }
        else
        {
            BulletHp = 3 + level;
        }
        
    }

    void Update()
    {
        if (isDead == false)
        {
            if (GameRootManager.Instance.isPlay && GameRootManager.Instance.IsEnterReady == false)
            {
                DoAttack(); // 尝试射击
            }
        }
    }

    void DoAttack()
    {
        // 计算方向
        totalTime += Time.deltaTime;
        if (totalTime >= bulletInv)
        {
            FireBullet();
            totalTime = 0; // 更新上次发射时间
        }
    }

    private void FireBullet()
    {
        if (Isfirst)
        {
            GameObject target = RandomTarget();
            if (target != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform);
                bullet.transform.parent = null;
                bullet.transform.localScale = Vector3.one;
                bullet.transform.position = new Vector3(target.transform.position.x, 2.5f,
                    target.transform.position.z);
                LeiYunBullet bt = bullet.GetComponent<LeiYunBullet>();
                bt.onehitmaxhp = BulletHp;
                bt.Move(Info, direction, 100, 0.7f, level);
                StartCoroutine(showeft());
                AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_jianqi, gameObject, 0.1f);
            }
        }
        else
        {
            if (Getdir())
            {
                GameObject bullet = Instantiate(bulletPrefab, transform);
                bullet.transform.parent = null;
                BaseBullet bt = bullet.GetComponent<BaseBullet>();
                bt.hp = BulletHp;
                bt.Move(Info, direction, 100, 2, level);
                StartCoroutine(showeft());
                AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_jianqi, gameObject, 0.1f);
            }
        }
    }

    public GameObject RandomTarget()
    {
        var atkItems = GameObject.FindGameObjectsWithTag("AtkItem");
        if (atkItems.Length == 0)
        {
            return null;
        }

        return atkItems[UnityEngine.Random.Range(0, atkItems.Length)];
    }


    public IEnumerator showeft()
    {
        shooteft.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shooteft.SetActive(false);
    }
}