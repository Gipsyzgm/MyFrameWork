using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IBezier : MonoBehaviour
{
    static int vertexCount = 20; //越大曲线越光滑

    public Transform[] tempTransform;
    List<Vector3> tempPoints = new List<Vector3>();

    void Update()
    {
        tempPoints = BezierCurve(tempTransform);
    }

    //三个点获得
    public static List<Vector3> BezierCurve(Vector3 pos0,Vector3 pos1,Vector3 pos2)
    {
        List<Vector3> points = new List<Vector3>();
        for (float i = 0; i <= 1; i += 1.0f / vertexCount)
        {
            Vector3 tangentLineVertex1 = Vector3.Lerp(pos0, pos1, i);
            Vector3 tangentLineVertex2 = Vector3.Lerp(pos1, pos2, i);
            Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, i);
            points.Add(bezierPoint);
        }
        points.Add(pos2);
        return points;
    }
    public static List<Vector3> BezierCurve(Transform pos0, Transform pos1, Transform pos2)
    {
        List<Vector3> points = new List<Vector3>();
        for (float i = 0; i <= 1; i += 1.0f / vertexCount)
        {
            Vector3 tangentLineVertex1 = Vector3.Lerp(pos0.position, pos1.position, i);
            Vector3 tangentLineVertex2 = Vector3.Lerp(pos1.position, pos2.position, i);
            Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, i);
            points.Add(bezierPoint);
        }
        points.Add(pos2.position);
        return points;
    }
    //多个点获得
    public static List<Vector3> BezierCurve(Transform[] trans)
    {
        if (trans == null || trans.Length == 0) return new List<Vector3>();

        List<Vector3> points = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            points.Add(UnlimitBezierCurve(trans, ratio));
        }
        points.Add(trans[trans.Length - 1].position);
        return points;
    }
    static Vector3 UnlimitBezierCurve(Transform[] trans, float t)
    {
        Vector3[] temp = new Vector3[trans.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = trans[i].position;
        }
        int n = temp.Length - 1;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n - i; j++)
            {
                temp[j] = Vector3.Lerp(temp[j], temp[j + 1], t);
            }
        }
        return temp[0];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < tempPoints.Count; i++)
        {
            if(i > 0)
            {
                Gizmos.DrawLine(tempPoints[i], tempPoints[i - 1]);
            }
        }
    }
}
