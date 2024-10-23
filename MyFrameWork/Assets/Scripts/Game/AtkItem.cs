using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class AtkItem : BaseAtkItem
{
    public GameObject ShowItem;
    public GameObject Mask;
    public GameObject Bg;
    public List<GameObject> NewItems;
    public int level = -1;
    
    public override void Init(UserDataInfo info = null, int ID = 0, bool canspwan = false, float scale = 1, int hp = 0)
    {
        oriHp = hp;
        maxHP = hp;
        originalScale = transform.localScale;
        textoriginalScale = hpText.transform.localScale;
        this.canspwan = canspwan;
        if (info == null)
        {
            Info = null;
            level = -1;
            headIcon?.SetActive(false);
            Mask?.SetActive(false);
            Bg?.SetActive(false);
        }
        else
        {
            Info = info;
            if (headIcon != null)
            {
                Mask?.SetActive(true);
                Bg?.SetActive(true);
                headIcon.SetActive(true);
                ToolsHelper.SetWebImage(headIcon.gameObject, info.avatarUrl);
            }
        }

        timeSlider?.gameObject.SetActive(false);

        SetBaseInfo();

        maxHP = Mathf.CeilToInt(maxHP * (1 + GameRootManager.Instance.gameAtkCamp.Gift7BuffAddNum *
            GameRootManager.Instance.gameAtkCamp.Gift7BuffLevel));
        oriHp = maxHP;
        if (hpText != null) hpText.text = maxHP.ToString();
        isDead = false;
    }


    public void SetBaseInfo()
    {
        if (maxHP == 0)
        {
            int hp = Random.Range(1, 6);
            if (hp <= 3)
            {
                maxHP = 1;
            }
            else
            {
                maxHP = 5;
            }
        }

        RandomCuritem();

        if (maxHP < 5)
        {
            transform.localScale = Vector3.one * 1.5f;
            curdieEft = dieEft[0];
        }
        else
        {
            transform.localScale = Vector3.one * 1.7f;
            curdieEft = dieEft[0];
        }

        originalScale = curitem.transform.localScale;
        hpText.text = maxHP.ToString();
    }


    void LateUpdate()
    {
        if (transform == null)
        {
            return;
        }

        // 计算新的局部旋转
        ShowItem.transform.localRotation = Quaternion.Inverse(transform.rotation);
    }


    public override void HitTime()
    {
        base.HitTime();
        if (curitem != null)
        {
            DOTween.Kill(curitem.transform);
            curitem.transform.localScale = originalScale;
            curitem.transform.DOScale(originalScale * 0.9f, 0.2f).OnComplete(() =>
            {
                // 缩放动画完成后，恢复到原始大小
                curitem.transform.localScale = originalScale;
            });
        }
    }

    public override IEnumerator DestroyGift()
    {
        AudioMgr.Instance.PlayEffectOnTarget( MyAudioName.gs_fangkuaibaolie, gameObject, 0.1f);
        return base.DestroyGift();
    }


    public void RandomCuritem()
    {
        int index = 0;
        if (level >= 0 && Info != null)
        {
            index = (int)Mathf.Ceil(level / 3f);
            curitem = NewItems[index];
        }
        else
        {
            //int index = Random.Range(0, 7);
            index = Random.Range(0, 100);
            if (index < 29)
            {
                index = 0;
            }
            else if (index < 32)
            {
                index = 1;
            }
            else if (index < 37)
            {
                index = 3;
            }
            else if (index < 66)
            {
                index = 4;
            }
            else if (index < 94)
            {
                index = 5;
            }
            else if (index < 100)
            {
                index = 6;
            }
            else
            {
                index = 0;
            }

            curitem = Items[index];
        }

        curitem.SetActive(true);
    }
}