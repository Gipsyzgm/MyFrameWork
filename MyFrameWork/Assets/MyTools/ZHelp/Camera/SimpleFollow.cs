using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机简单控制，平滑跟随玩家移动，玩家速度过快会产生晃动，fixedTime可不晃动
/// </summary>
public class SimpleTarget : MonoBehaviour {

    public Vector3 offset = new Vector3(0, 0, -10);//相机相对于玩家的位置
    public Transform target;
    Vector3 pos;
    float speed = 2;

    void Update()
    {
       
        pos = target.position + offset;
        this.transform.position = Vector3.Lerp(this.transform.position, pos, speed * Time.deltaTime);//调整相机与玩家之间的距离
        Quaternion angel = Quaternion.LookRotation(target.position - this.transform.position);//获取旋转角度
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, angel, speed * Time.deltaTime);

    }
}
