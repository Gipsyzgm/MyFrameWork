using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuoLian : BaseDefItem
{
    public GameObject shooteft;
    
    private Rigidbody rb;

    public override void Init(UserDataInfo userData, int ID = 12)
    {
        base.Init(userData,ID);
        bulletInv = 1f;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CountTime());
    }


    IEnumerator CountTime()
    {
        yield return new WaitUntil(() => GameRootManager.Instance.isPlay);
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
        
        Destroy(gameObject);
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


    void FixedUpdate()
    {
        // movetime += Time.deltaTime;
        // if (movetime < 1.5f)
        // {
        //     rb.AddForce(transform.forward * Speed);
        // }
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
        if (Getdir())
        {
            AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_feixing, gameObject, 0.2f);
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.transform.parent = null;
            bullet.name = "HuoLianBullet";
            BaseBullet bt = bullet.GetComponent<BaseBullet>();
            bt.Move(Info, direction, 17, 15, level);
            
            //StartCoroutine(showeft());
        }
    }


    public IEnumerator showeft()
    {
        shooteft.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shooteft.SetActive(false);
    }
}