using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseBullet : MonoBehaviour
{
    public float speed = 100.0f;
    public Vector3 dir = Vector3.forward;
    public float duration = 3.0f;
    public bool StartMove = false;
    public GameObject[] items;
    public GameObject curuseitem;
    public int hp = 1;
    public int onehitmaxhp = 1;

    public UserDataInfo Info;

    /// <summary>
    /// 子弹类型。
    /// 0,普通子弹,hp归零销毁
    /// 1,普通子弹,自行销毁
    /// 2,持续检测类型
    /// 3,目标单次伤害不大于最大伤害，hp归零销毁
    /// </summary>
    public int Type = 0;


    public void SetHp(int ratio)
    {
        hp = hp * ratio;
    }

    public virtual void Move(UserDataInfo info, Vector3 _dir, float _speed, float _duration, int level)
    {
        hp = Mathf.CeilToInt(hp * (1 + GameRootManager.Instance.gameDefCamp.Gift7BuffCoef *
            GameRootManager.Instance.gameDefCamp.Gift7BuffLevel));
        Info = info;
        dir = _dir;
        speed = _speed;
        duration = _duration;
        StartMove = true;
        transform.rotation = Quaternion.LookRotation(dir);
        curuseitem.SetActive(true);
        Destroy(this.gameObject, duration);
    }

    public virtual void FixedUpdate()
    {
        if (StartMove)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    #region 碰撞检测

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "wall")
        {
            Vector3 direction = Vector3.Reflect(dir, other.GetContact(0).normal);
            //Debug.Log("反弹的向量" + other.GetContact(0).normal);
            direction.y = 0;
            dir = direction;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            Vector3 currentPosition = transform.position;
            // 找到最近的点
            Vector3 closestPoint = other.bounds.ClosestPoint(currentPosition);
            // 计算法线方向
            Vector3 normal = CalculateNormal(other.bounds, closestPoint);
            Vector3 direction = Vector3.Reflect(dir, normal);
            // 计算反弹力的方向
            direction.y = 0;
            dir = direction;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public virtual Vector3 CalculateNormal(Bounds bounds, Vector3 point)
    {
        // 获取边界框的中心点
        Vector3 center = bounds.center;

        // 获取边界框的尺寸
        Vector3 size = bounds.size;

        // 计算法线方向
        Vector3 normal = Vector3.zero;

        // 检查 x 轴方向
        if (point.x == center.x + size.x / 2)
        {
            normal = new Vector3(1, 0, 0);
        }
        else if (point.x == center.x - size.x / 2)
        {
            normal = new Vector3(-1, 0, 0);
        }

        // 检查 y 轴方向
        if (point.y == center.y + size.y / 2)
        {
            normal = new Vector3(0, 1, 0);
        }
        else if (point.y == center.y - size.y / 2)
        {
            normal = new Vector3(0, -1, 0);
        }

        // 检查 z 轴方向
        if (point.z == center.z + size.z / 2)
        {
            normal = new Vector3(0, 0, 1);
        }
        else if (point.z == center.z - size.z / 2)
        {
            normal = new Vector3(0, 0, -1);
        }

        return normal;
    }

    #endregion 碰撞检测


    public virtual void UnloadGameObject()
    {
        Destroy(gameObject);
    }
}