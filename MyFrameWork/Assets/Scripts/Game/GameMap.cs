using System;
using System.Collections.Generic;
using UnityEngine;

public enum CampType
{
    None = 0,
    Atk = 1,
    Def = 2
}

public enum CampIndex
{
    //Up = 1,
    Mid = 1
    //Down = 3,
}

public class GameMap : MonoBehaviour
{
    public MapMove mapMove;
    public AtkItemMove atkItemMove;
    public AtkItemMove defItemMove;
    public float atkItemMoveSpeed = 1.0f;
    public float mapMoveSpeed = 4.5f;
    public static GameMap Instance { get; private set; }
    public float maxlengh = 540f;

    public GameObject topBuild;
    public GameObject btmBuild;
    public GameObject CHECKBOX;
    public GameObject WINDWALL;
    public GameObject JIANTOU;
    
    public List<FixedTurret> TopThree;

    //荒塔等大型物体
    public List<GameObject> CheckGameObject1;

    //中型物体
    public List<GameObject> CheckGameObject2;

    //圣女炉位置判定
    public List<GameObject> CheckGameObject3;

    public void Awake()
    {
        Instance = this;
        
        
        Reset();
    }

    public void SetAtkItemSpeed(float speed)
    {
        atkItemMove.mspeed = speed;
    }

    public void SetMapMoveSpeed(float speed)
    {
        mapMove.SetSpeed(speed);
    }

    public void SetTopThreeInfo(List<UserDataInfo> defItems)
    {
        for (int i = 0; i < defItems.Count; i++)
        {
            if (i < 3)
            {
                TopThree[i].SetInfo(defItems[i]);
            }
        }
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("增加地图速度")]
#endif
    public void AddMapSpeed()
    {
        var speed = mapMove.oriSpeed + 1f;
        mapMove.SetSpeed(speed);
    }
#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("增加方块速度")]
#endif
    public void AddAtkItemMoveSpeed()
    {
        atkItemMove.mspeed = atkItemMove.mspeed + 0.5f;
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button("还原所有速度")]
#endif
    public void ResetAllSpeed()
    {
        mapMove.SetSpeed(mapMoveSpeed);
        atkItemMove.mspeed = atkItemMoveSpeed;
    }


    public void Reset()
    {
        WINDWALL.SetActive(true);
        JIANTOU.SetActive(false);
        maxlengh = 2000f;
        mapMove.isJingBao = true;
        mapMove.isJingBao2 = true;
        topBuild.transform.position = new Vector3(0, 0, 63);
        btmBuild.transform.position = new Vector3(0, 0, -78);
        for (int i = 0; i < TopThree.Count; i++)
        {
            TopThree[i].SetInfo(null) ;
        }
        mapMove.warningLengh = 100;
    }


    public GameObject GetBestPos(List<GameObject> group)
    {
        float dist = 0;
        GameObject target = group[0];
        foreach (var VARIABLE in group)
        {
            float dist2 = FindPos(VARIABLE);
            if (dist2 > dist)
            {
                dist = dist2;
                target = VARIABLE;
            }
        }

        Debug.Log("target: " + target.name);
        return target;
    }

    public LayerMask groundLayer;

    public float FindPos(GameObject VARIABLE)
    {
        float dist = 250;

        Vector3 origin = VARIABLE.transform.position; // 方块的当前位置
        Vector3 halfExtents = VARIABLE.transform.localScale * 0.5f; // 方块的一半尺寸
        Vector3 direction = Vector3.down; // 向下方向
        Quaternion orientation = Quaternion.identity; // 方块的方向
        // 执行BoxCast
        if (Physics.BoxCast(origin, halfExtents, direction, out RaycastHit hit, orientation, 250, groundLayer))
        {
            // 如果检测到碰撞
            //Debug.Log("Distance to collision: " + hit.distance);
            dist = hit.distance;
            // 可视化BoxCast
            //Debug.DrawLine(origin, hit.point, Color.red);
        }
        else
        {
            //Debug.Log("No collision detected.");
        }
        return dist;
    }
}