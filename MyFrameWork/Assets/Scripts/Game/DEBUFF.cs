using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUFF : MonoBehaviour
{
    #region 碰撞检测

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "DefItem")
        {
            BaseDefItem  DF = other.collider.gameObject.GetComponent<BaseDefItem>();
            DF.ShowJianSuEft();
            
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DefItem")
        {
          
            BaseDefItem  DF = other.gameObject.GetComponent<BaseDefItem>();
            DF.ShowJianSuEft();
        }
    }

    #endregion 碰撞检测
}