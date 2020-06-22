using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutTranfrom : MonoBehaviour {

    //世界坐标转局部坐标，任意位置都可以。
    public Vector3 WorldToLocalPos(Transform parent,Transform worldPos)
    {
        Vector3 LocalPos;
        LocalPos = parent.InverseTransformPoint(worldPos.position);
        return LocalPos;
    }
    //世界坐标（targetTrans）转相对（parent）的世界坐标。
    //例：targetTrans的世界坐标是（1，1, 1）parent的世界坐标是（2,2,2）那么获得的相对坐标是以parent为坐标原点的（1,1,1）位置，即向量相加返回为（3,3,3）
    //可以这么理解，相对于target的局部坐标转化成世界坐标。就是target的子物体的localPosition转成世界坐标。(只能转子物体，孙物体不行。)
    //可以通过孙物体的世界坐标InverseTransformPoint转成相对于parent的子物体坐标。然后再TransformPoint转成世界坐标。（证明两个方法是互逆的）
    //嗯，我都能获取到孙物体的世界坐标了还转什么呢？没太清楚这个方法的具体应用场景。
    public Vector3 targetTransToWorldPos(Transform parent, Transform targetTrans)
    {
        Vector3 WorldPos;
        WorldPos = parent.TransformPoint(targetTrans.localPosition);
        return WorldPos;

    }


  



}
