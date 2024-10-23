using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    public float mapScrollSpeed = 0.01f;
    public GameObject map;

    /// <summary>
    /// 地图移动速度
    /// </summary>
    public float oriSpeed;

    /// <summary>
    /// 刷新间隔
    /// </summary>
    private float totalTime = 0f;


    private float yuancishantotalTime = 0f;

    private float checkstop = 0f;


    bool startMove = false;
    public bool isJingBao = true;
    public bool isJingBao2 = true;
    public int warningLengh = 100;

    void Start()
    {
    }

    public void SetSpeed(float speed)
    {
        mapScrollSpeed = (speed / oriSpeed) * mapScrollSpeed;
        oriSpeed = speed;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AtkItem" || other.tag == "AtkItem2")
        {
            startMove = true;
        }
    }

    void FixedUpdate()
    {
        if (GameRootManager.Instance.isPlay && GameRootManager.Instance.IsEnterReady == false)
        {
            if (startMove)
            {
                if (GameRootManager.Instance.gameDefCamp.IsYuanCiShanAlive)
                {
                    yuancishantotalTime += Time.deltaTime;
                    if (yuancishantotalTime >= 0.5f)
                    {
                        int damage = CountDamage();

                        GameRootManager.Instance.gameDefCamp.HitYuanCiShan(-damage);
                        yuancishantotalTime = 0; // 更新上次发射时间
                    }
                    StartMove(false);
                }
                else
                {
                    StartMove(true);
                    float offset;
                    if (GameMap.Instance.maxlengh <= 50)
                    {
                        offset = Time.time * mapScrollSpeed / 5;
                    }
                    else if (GameMap.Instance.maxlengh <= 200)
                    {
                        offset = Time.time * mapScrollSpeed / 2;
                    }
                    else
                    {
                        offset = Time.time * mapScrollSpeed;
                    }

                    map.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, offset);
                    totalTime += Time.deltaTime;

                    if (GameMap.Instance.maxlengh <= 33 && GameMap.Instance.btmBuild.transform.position.z < -40)
                    {
                        GameMap.Instance.btmBuild.transform.Translate(Vector3.up * oriSpeed / 2 * Time.fixedDeltaTime);
                    }

                    GameMap.Instance.topBuild.transform.Translate(Vector3.up * oriSpeed * Time.fixedDeltaTime);

                    if (totalTime >= 0.2f)
                    {
                        if (GameMap.Instance.maxlengh <= 50)
                        {
                            GameMap.Instance.maxlengh = GameMap.Instance.maxlengh - (0.2f * oriSpeed / 5);
                        }
                        else if (GameMap.Instance.maxlengh <= 200)
                        {
                            GameMap.Instance.maxlengh = GameMap.Instance.maxlengh - (0.2f * oriSpeed / 2);
                        }
                        else
                        {
                            GameMap.Instance.maxlengh = GameMap.Instance.maxlengh - 0.2f * oriSpeed;
                        }

                        if (GameMap.Instance.maxlengh <= 1000 && isJingBao)
                        {
                            GameRootManager.Instance.DoTipMarquee(MarqueeType.TipKingDodgeLeft2);
                            
                            AudioMgr.Instance.PlayEffect(MyAudioName.gs_jingbao, 0.3f);
                            isJingBao = false;
                        }
                        else if (GameMap.Instance.maxlengh <= 500 && isJingBao2)
                        {
                            GameRootManager.Instance.DoTipMarquee(MarqueeType.TipKingDodgeRight1);
                            AudioMgr.Instance.PlayEffect(MyAudioName.gs_jingbao, 0.3f);
                            isJingBao2 = false;
                        }

                        if (GameMap.Instance.maxlengh <= warningLengh && warningLengh > 0)
                        {
                            AudioMgr.Instance.PlayEffect(MyAudioName.gs_jingbao, 0.3f);
                            warningLengh -= 10;
                        }

                        EventMgr.Instance.InvokeEvent(EventConst.GameMaxLengh, Mathf.Round(GameMap.Instance.maxlengh));

                        if (GameMap.Instance.maxlengh <= 0)
                        {
                            GameMap.Instance.maxlengh = 0;
                            //游戏结束
                            GameRootManager.Instance.OnGameOver(CampType.Def);
                        }

                        totalTime = 0; // 更新上次发射时间
                    }
                }

                checkstop = 0;
                startMove = false;
            }
            else
            {
                checkstop += Time.deltaTime;
                if (checkstop >= 1f)
                {
                    checkstop = 0f;
                    StartMove(false);
                }
            }
        }
    }


    public void StartMove(bool isStop)
    {
        if (isStop)
        {
            if (GameMap.Instance.WINDWALL.activeSelf == true)
            {
                GameMap.Instance.WINDWALL.SetActive(false);
                GameMap.Instance.JIANTOU.SetActive(true);
                AudioMgr.Instance.PlayEffect(MyAudioName.jushi_jingong);
            }
        }
        else
        {
            if (GameMap.Instance.WINDWALL.activeSelf == false)
            {
                GameMap.Instance.WINDWALL.SetActive(true);
                GameMap.Instance.JIANTOU.SetActive(false);
                AudioMgr.Instance.PlayEffect(MyAudioName.jushi_fangyu);
            }
        }
    }

    public int CountDamage()
    {
        int sumHp = 0;
        int damage = 0;
        var GiftatkItems = GameObject.FindGameObjectsWithTag("AtkItem2");
        if (GiftatkItems.Length > 0)
        {
            // 寻找最近Cube的逻辑
            foreach (var cube in GiftatkItems)
            {
                BaseAtkItem dist = cube.GetComponent<BaseAtkItem>();
                sumHp += dist.maxHP;
            }
        }

        var atkItems = GameObject.FindGameObjectsWithTag("AtkItem");
        if (atkItems.Length > 0)
        {
            foreach (var cube in atkItems)
            {
                BaseAtkItem dist = cube.GetComponent<BaseAtkItem>();
                sumHp += dist.maxHP;
            }
        }

        if (sumHp <= 10000)
        {
            damage = (int)Mathf.Max(1, (0.0146f * sumHp) - 21);
            if (damage < 0)
            {
                damage = 0;
            }
        }
        else
        {
            damage = (int)(0.0046f * sumHp) + 79;

            if (damage > 600)
            {
                damage = 600;
            }
        }

        return damage;
    }
}