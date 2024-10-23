using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class BaseAtkItem : MonoBehaviour
{
    public Text hpText;
    public GameObject headIcon;
    public Image timeSlider;
    public List<GameObject> Items;
    public List<GameObject> dieEft;
    public GameObject curdieEft;
    public GameObject curitem;
    public GameObject UIItem;
    public GameObject UpHpEft;
    public int AtkLevel = 0;


    public UserDataInfo Info; //用户ID
    public int maxHP = 1;
    public int oriHp = 1;
    public bool isDead;
    public bool canspwan;
    public Vector3 originalScale;
    public Vector3 textoriginalScale;

    public virtual void Init(UserDataInfo info = null, int ID = 1, bool canspwan = false, float scale = 1, int HP = 0)
    {
        var table = GiftEffectConfig.Get(ID);
        int hp = table.life;
        oriHp = hp;
        maxHP = hp;
        originalScale = transform.localScale;
        textoriginalScale = hpText.transform.localScale;
        this.canspwan = canspwan;
        if (info == null)
        {
            headIcon?.SetActive(false);
        }
        else
        {
            Info = info;
            if (headIcon != null)
            {
                headIcon.SetActive(true);
                ToolsHelper.SetWebImage(headIcon.gameObject, info.avatarUrl);
            }
        }

        timeSlider?.gameObject.SetActive(false);
        maxHP = Mathf.CeilToInt(maxHP * (1 + GameRootManager.Instance.gameAtkCamp.Gift7BuffAddNum * GameRootManager.Instance.gameAtkCamp.Gift7BuffLevel));
        oriHp = maxHP;
        if (hpText != null) hpText.text = maxHP.ToString();
        isDead = false;
    }


    public virtual void UnInit()
    {
        curitem?.SetActive(false);
        isDead = true;
        ShowDieEft();
        transform.position = Vector3.down * 100;
        hpText.text = "1";
        headIcon.SetActive(false);
    }

    public virtual void ShowDieEft()
    {
        GameObject eft = Instantiate(curdieEft, transform);
        eft.transform.parent = null;
        eft.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        eft.SetActive(true);
        Destroy(eft, 1f);
    }


    #region 碰撞检测

    public virtual void OnCollisionEnter(Collision other)
    {
        if (isDead == false)
        {
            if (other.collider.tag == "bullet")
            {
                BaseBullet bullet = other.collider.gameObject.GetComponent<BaseBullet>();
                maxHP = maxHP - bullet.onehitmaxhp;
                // 
                if (maxHP <= 0)
                {
                    if (maxHP == 0)
                    {
                        GameRootManager.Instance.AddScore(CampType.Def, bullet.hp, 1);
                        bullet.Info?.AddScore(bullet.hp, 1);
                        if (bullet.Type == 0)
                        {
                            Destroy(other.collider.gameObject);
                        }
                    }
                    else
                    {
                        bullet.hp = Mathf.Abs(maxHP);
                    }

                    StartCoroutine(DestroyGift());
                }
                else
                {
                    GameRootManager.Instance.AddScore(CampType.Def, bullet.hp, 1);
                    bullet.Info?.AddScore(bullet.hp, 1);
                    if (bullet.Type == 0)
                    {
                        Destroy(other.collider.gameObject);
                    }

                    if (hpText)
                    {
                        hpText.text = maxHP.ToString();
                    }
                }
            }
        }
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        if (isDead == false)
        {
            if (other.tag == "bullet")
            {
                BaseBullet bullet = other.gameObject.GetComponent<BaseBullet>();
                if (bullet.Type != 2)
                {
                    if (bullet.Type == 3)
                    {
                        maxHP = maxHP - bullet.onehitmaxhp;
                    }
                    else
                    {
                        maxHP = maxHP - bullet.hp;
                    }

                    if (maxHP <= 0)
                    {
                        if (maxHP == 0)
                        {
                            if (bullet.Type == 0)
                            {
                                GameRootManager.Instance.AddScore(CampType.Def, bullet.hp, 1);
                                bullet.Info?.AddScore(bullet.hp, 1);
                                Destroy(other.gameObject);
                            }
                            else if (bullet.Type == 3)
                            {
                                bullet.hp = bullet.hp - bullet.onehitmaxhp;
                                if (bullet.hp > 0)
                                {
                                    GameRootManager.Instance.AddScore(CampType.Def, bullet.onehitmaxhp, 1);
                                    bullet.Info?.AddScore(bullet.onehitmaxhp, 1);
                                }
                                else
                                {
                                    GameRootManager.Instance.AddScore(CampType.Def, bullet.hp, 1);
                                    bullet.Info?.AddScore(bullet.hp, 1);
                                    Destroy(other.gameObject);
                                }
                            }
                        }
                        else
                        {
                            if (bullet.Type == 3)
                            {
                                bullet.hp = bullet.hp - bullet.onehitmaxhp + Mathf.Abs(maxHP);
                            }
                            else
                            {
                                bullet.hp = Mathf.Abs(maxHP);
                            }
                        }

                        StartCoroutine(DestroyGift());
                    }
                    else
                    {
                        HitTime();
                        if (bullet.Type == 0)
                        {
                            GameRootManager.Instance.AddScore(CampType.Def, bullet.hp, 1);
                            bullet.Info?.AddScore(bullet.hp, 1);
                            Destroy(other.gameObject);
                        }
                        else if (bullet.Type == 3)
                        {
                            bullet.hp = bullet.hp - bullet.onehitmaxhp;
                            if (bullet.hp > 0)
                            {
                                GameRootManager.Instance.AddScore(CampType.Def, bullet.onehitmaxhp, 1);
                                bullet.Info?.AddScore(bullet.onehitmaxhp, 1);
                            }
                            else
                            {
                                GameRootManager.Instance.AddScore(CampType.Def, bullet.hp, 1);
                                bullet.Info?.AddScore(bullet.hp, 1);
                                Destroy(other.gameObject);
                            }
                        }

                        if (hpText)
                        {
                            hpText.text = maxHP.ToString();
                        }
                    }
                }

                PlaySounds();
            }
        }
    }


    public float jianqitotaltime = 0;

    public virtual void OnTriggerStay(Collider other)
    {
        if (isDead == false)
        {
            if (other.tag == "bullet")
            {
                BaseBullet bullet = other.gameObject.GetComponent<BaseBullet>();
                if (bullet.Type == 2)
                {
                    jianqitotaltime += Time.deltaTime;
                    if (jianqitotaltime >= 0.2f)
                    {
                        maxHP = maxHP - bullet.hp;
                        if (hpText)
                        {
                            if (maxHP < 0)
                            {
                                hpText.text = "0";
                            }
                            else
                            {
                                HitTime();
                                hpText.text = maxHP.ToString();
                            }
                        }

                        if (maxHP <= 0)
                        {
                            StartCoroutine(DestroyGift());
                        }

                        jianqitotaltime = 0;
                    }
                }
            }
        }
    }

    #endregion 碰撞检测


    public virtual void HitTime()
    {
        if (hpText != null)
        {
            DOTween.Kill(hpText.transform);
            hpText.transform.localScale = originalScale;
            hpText.transform.DOScale(textoriginalScale * 0.8f, 0.2f).OnComplete(() =>
            {
                // 缩放动画完成后，恢复到原始大小
                hpText.transform.localScale = textoriginalScale;
            });
        }
    }


    public virtual void RefreshHp()
    {
        if (hpText != null) hpText.text = maxHP.ToString();
    }

    public virtual void PlaySounds()
    {
    }


    public virtual IEnumerator DestroyGift()
    {
        isDead = true;
        AddScore();
        GameRootManager.Instance.RemoveFromAtkItemList(this);
        yield return null;
    }

    public virtual void AddScore()
    {
        if (Info != null)
        {
            Info.AddScore(oriHp, 1);
        }

        GameRootManager.Instance.AddScore(CampType.Atk, oriHp, 1);
    }
}