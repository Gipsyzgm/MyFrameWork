using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JianZhenItem : BaseDefItem
{
    public GameObject shooteft;
    private Rigidbody rb;
    public float scaleFactor = 1.0f; // 缩放比例
    public float duration = 0.1f;

    public override void Init(UserDataInfo userData, int ID = 13)
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
            if (GameRootManager.Instance.isPlay)
            {
                totalTime += Time.deltaTime;
                if (totalTime > 1.2f)
                {
                    DoAttack(); // 尝试射击
                    totalTime = 0;
                }
            }
        }
        AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_jianqi3, gameObject, 0.1f);
    }


    void FixedUpdate()
    {
        // movetime += Time.deltaTime;
        // if (movetime < 1.5f)
        // {
        //     rb.AddForce(transform.forward * Speed);
        // }
    }

    public void DoAttack()
    {
        if (Getdir())
        {
            shooteft.SetActive(true);
            bulletPrefab.SetActive(true);
            bulletPrefab.transform.localScale = new Vector3(bulletPrefab.transform.localScale.x,
                bulletPrefab.transform.localScale.y, (scaleFactor+9) * 2); // 确保最终到达目标缩放
            shooteft.transform.localScale = new Vector3(1, 1, scaleFactor/25);
        }
        else
        {
            shooteft.SetActive(false);
            bulletPrefab.SetActive(false);
        }
    }


    public override bool Getdir()
    {
        
        
        var GiftatkItems = GameObject.FindGameObjectsWithTag("AtkItem2");

        if (GiftatkItems.Length == 0)
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
                scaleFactor = minDist;
                var idealtargetpos = new Vector3(closestCube.transform.position.x, ShowItem.transform.position.y,
                    closestCube.transform.position.z);
                ShowItem.transform.LookAt(idealtargetpos);
                return true;
                
               
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
                    if ( level < dist.AtkLevel)
                    {
                        level = dist.AtkLevel;
                        closestCube = dist.gameObject;
                    }
                }
                
                scaleFactor = Vector3.Distance(SpwnPos.transform.position, closestCube.transform.position);
                var idealtargetpos = new Vector3(closestCube.transform.position.x, ShowItem.transform.position.y,
                    closestCube.transform.position.z);
                ShowItem.transform.LookAt(idealtargetpos);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        
     


     
       
          
            
     

    
    }

    private void FireBullet()
    {
        if (Getdir())
        {
            //StartCoroutine(ScaleAlongAxisCoroutine());
        }
    }
    
    
    public IEnumerator showeft()
    {
        shooteft.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shooteft.SetActive(false);
    }
}