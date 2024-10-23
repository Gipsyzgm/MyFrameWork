using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YuanCiShanItem : BaseDefItem
{
    public Text hpText;
    public int Hp;
    public GameObject DieEft;
    public override void Init(UserDataInfo userData,int ID = 11)
    {
        var table = GiftEffectConfig.Get(ID);
        AddYuanCiShanNum = table.life;
        if (AddYuanCiShanNum > 0)
        {
            GameRootManager.Instance.gameDefCamp.IsYuanCiShanAlive = true;
            Hp = AddYuanCiShanNum;
            hpText.text = Hp.ToString();
            gameObject.SetActive(true);
        }
       
    }




    public void SetHp(int hp)
    {
        Hp = Hp + hp;
        //不同特效
        if (hp > 0)
        {
          
            
        }
        else
        {
            
            
        }
        
        if (Hp <= 0 )
        {
        
            AudioMgr.Instance.PlayEffectOnTarget(MyAudioName.gs_daota3, gameObject, 1f);
           
            StartCoroutine(ShowDieEft());
            GameRootManager.Instance.gameDefCamp.UnloadYuanCiShan();
        }
        else
        {
            hpText.text = Hp.ToString();  
        }
    }

    IEnumerator ShowDieEft()
    {
        GameObject eft = Instantiate(DieEft, transform);
        eft.transform.SetParent(null);
        eft.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Destroy(eft);
    }
    
}