using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JianZhenBullet : BaseBullet
{
    
    
    
    public override void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "AtkItem")
        {
         
            
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (other.collider.tag == "AtkItem")
        {
           
            
        }
    }
    
    
    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AtkItem")
        {
        
            
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AtkItem")
        {
           
            
        }
    }
}