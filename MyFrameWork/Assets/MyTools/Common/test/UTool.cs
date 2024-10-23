//======================================
//	Author: Matrix
//  Create Time：2017/9/12 18:41:42 
//  Function:
//======================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class UTool
{
    public static Vector3 ToVector3(string v3Str)
    {
        string[] v3Array = v3Str.Split(UConfig.splitChar[0]);
        if (v3Array.Length != 3)
        {
            Debug.LogError("Can't convert to vector3:" + v3Str);
            return Vector3.zero;
        }

        float x;
        float y;
        float z;
        var bakCulture = System.Globalization.CultureInfo.CurrentCulture;
        System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        if (float.TryParse(v3Array[0], out x) &&
            float.TryParse(v3Array[1], out y) &&
            float.TryParse(v3Array[2], out z)
           )
        {
            System.Globalization.CultureInfo.CurrentCulture = bakCulture;
            return new Vector3(x, y, z);
        }
        else
        {
            System.Globalization.CultureInfo.CurrentCulture = bakCulture;
            Debug.LogError("Can't convert to vector3:" + v3Str);
            return Vector3.zero;
        }
    }

    #region geometry

    /// <summary>
    /// 暂时别用
    /// </summary>
    public static bool IsRectInsect(Vector3 centerA, Vector3 extensA, Quaternion rotA,
        Vector3 centerB, Vector3 extensB, Quaternion rotB)
    {
        Vector3 rectABottomLeft = rotA * (centerA - extensA);
        Vector3 rectATopRight = rotA * (centerA + extensA);
        float rectALeftX = rectABottomLeft.x;
        float rectALeftY = rectABottomLeft.z;
        float rectARightX = rectATopRight.x;
        float rectARightY = rectATopRight.z;

        Vector3 rectBBottomLeft = rotB * (centerB - extensB);
        Vector3 rectBTopRight = rotB * (centerB + extensB);
        float rectBLeftX = rectBBottomLeft.x;
        float rectBLeftY = rectBBottomLeft.z;
        float rectBRightX = rectBTopRight.x;
        float rectBRightY = rectBTopRight.z;

        float lengthXSum = 2 * (extensA.x + extensB.x);
        float lengthYSum = 2 * (extensA.z + extensB.z);
        return IsRectInsect(rectALeftX, rectALeftY, rectARightX, rectARightY,
            rectBLeftX, rectBLeftY, rectBRightX, rectBRightY, lengthXSum, lengthYSum);
    }

    /// <summary>
    /// 暂时别用
    /// </summary>
    public static bool IsRectInsect(float rectALeftX, float rectALeftY, float rectARightX, float rectARightY,
        float rectBLeftX, float rectBLeftY, float rectBRightX, float rectBRightY,
        float lengthXSum, float lengthYSum)
    {
        bool result = false;
        float distanceXAandBCenter = Mathf.Abs(rectALeftX + rectARightX - (rectBLeftX + rectBRightX));
        float distanceYAandBCenter = Mathf.Abs(rectALeftY + rectARightY - (rectBLeftY + rectBRightY));
        //float lengthXSum = Mathf.Abs(rectALeftX - rectARightX) + Mathf.Abs(rectBLeftX - rectBRightX);
        //float lengthYSum = Mathf.Abs(rectALeftY - rectARightY) + Mathf.Abs(rectBLeftY - rectBRightY);
        if (distanceXAandBCenter <= lengthXSum && distanceYAandBCenter <= lengthYSum)
        {
            result = true;
        }

        return result;
    }

    /// <summary>
    /// 暂时别用
    /// </summary>
    public static bool IsInsect(Bounds boundA, Bounds boundB)
    {
        return boundA.Intersects(boundB);
    }

    public static Vector3 RotatePoint(Vector3 point, Vector3 eulerAngles)
    {
        Quaternion q = Quaternion.Euler(eulerAngles);
        return q * point;
    }

    static Vector3[] corner = new Vector3[4];

    public static Vector3 GetMaxXWorldCorner(GameObject go)
    {
        BoxCollider bc = go.GetComponent<BoxCollider>();
        if (bc == null) return go.transform.position;
        Vector3 halfSize = bc.size * 0.5f;
        corner[0] = bc.center;
        corner[0].x -= halfSize.x;
        corner[0].z -= halfSize.z;

        corner[1] = bc.center;
        corner[1].x -= halfSize.x;
        corner[1].z += halfSize.z;

        corner[2] = bc.center;
        corner[2].x += halfSize.x;
        corner[2].z += halfSize.z;

        corner[3] = bc.center;
        corner[3].x += halfSize.x;
        corner[3].z -= halfSize.z;

        int i;
        for (i = 0; i < corner.Length; i++)
        {
            corner[i] = go.transform.TransformPoint(corner[i]);
        }

        Vector3 maxXPos = corner[0];
        for (i = 0; i < corner.Length; i++)
        {
            if (corner[i].x >= maxXPos.x)
            {
                maxXPos = corner[i];
            }
        }

        return maxXPos;
    }

    public static bool IsAngleBelow90(Vector3 from, Vector3 to)
    {
        float dot = from.x * to.x + from.y * to.y + from.z * to.z;
        return dot > 0;
    }

    #endregion

    # region physics

    public static bool CheckAnyObstacle(Vector3 pos, float radius, float centerValue, float height, int layerMask)
    {
        Vector3 bottom = pos + Vector3.up * (centerValue - height * 0.5F);
        Vector3 top = bottom + Vector3.up * height;
        bool result = false;
        if (Physics.CheckCapsule(bottom, top, radius, layerMask))
            result = true;
        return result;
    }

    #endregion

    #region serialize

    public static byte[] SerializeObject(object obj)
    {
        if (obj == null)
            return null;
        //内存实例
        MemoryStream ms = new MemoryStream();
        //创建序列化的实例
        BinaryFormatter formatter = new BinaryFormatter();

        SurrogateSelector ss = new SurrogateSelector();
        var streamingContext = new StreamingContext(StreamingContextStates.All);

        formatter.SurrogateSelector = ss;

        formatter.Serialize(ms, obj);
        byte[] bytes = ms.GetBuffer();
        return bytes;
    }

    public static object DeserializeObject(byte[] bytes)
    {
        object obj = null;
        if (bytes == null)
            return obj;
        //利用传来的byte[]创建一个内存流
        MemoryStream ms = new MemoryStream(bytes);
        ms.Position = 0;
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector ss = new SurrogateSelector();
        var streamingContext = new StreamingContext(StreamingContextStates.All);
        formatter.SurrogateSelector = ss;
        obj = formatter.Deserialize(ms);
        ms.Close();
        return obj;
    }

    #endregion

    #region Frustum

    //static Plane[] planes = new Plane[6];
    //public static bool InCameraRange(Bounds bound)
    //{
    //    bool result = GeometryUtility.TestPlanesAABB(planes, bound);
    //    return result;
    //}
    public static void GemCullRange(Vector3 camPos, float fov, float angleX, Plane[] planes)
    {
        Camera cam = CameraMgr.Instance.activeCamera.camera;
        GemCullRange(camPos, fov, cam.aspect, angleX, cam.nearClipPlane, cam.farClipPlane, planes);
    }

    public static void GemCullRange(Vector3 camPos, float fov, float aspect, float angleX, float zNear, float zFar,
        Plane[] planes)
    {
        Matrix4x4 projMatrix = CalcProjMatrix(fov, aspect, zNear, zFar);
        Matrix4x4 viewMatrix = CalcWorld2CamMatrix(camPos, angleX);

        GeometryUtility.CalculateFrustumPlanes(projMatrix * viewMatrix, planes);
    }

    static Matrix4x4 CalcProjMatrix(float fov, float aspect, float zNear, float zFar)
    {
        float zRange = zNear - zFar;
        float tanHalfFOV = Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        Matrix4x4 projMatrix = new Matrix4x4();
        projMatrix.m00 = 1.0f / (tanHalfFOV * aspect);
        projMatrix.m01 = 0.0f;
        projMatrix.m02 = 0.0f;
        projMatrix.m03 = 0.0f;
        projMatrix.m10 = 0.0f;
        projMatrix.m11 = 1.0f / tanHalfFOV;
        projMatrix.m12 = 0.0f;
        projMatrix.m13 = 0.0f;
        projMatrix.m20 = 0.0f;
        projMatrix.m21 = 0.0f;
        projMatrix.m22 = (zNear + zFar) / zRange;
        projMatrix.m23 = 2.0f * zFar * zNear / zRange;
        projMatrix.m30 = 0.0f;
        projMatrix.m31 = 0.0f;
        projMatrix.m32 = -1.0f;
        projMatrix.m33 = 0.0f;
        return projMatrix;
    }

    static Matrix4x4 CalcWorld2CamMatrix(Vector3 camPos, float angleX)
    {
        //Transform camTran = CameraMgr.Instance.activeCamera.camTranslate.transform;

        Vector3 dir = GetCamDirect(angleX);
        //x
        Vector3 n1 = Vector3.Cross(dir, Vector3.up);
        n1 = n1.normalized;
        //y
        Vector3 n2 = Vector3.Cross(n1, dir);
        n2 = n2.normalized;
        Matrix4x4 viewMatrix = new Matrix4x4();
        viewMatrix.m00 = n1.x;
        viewMatrix.m01 = n1.y;
        viewMatrix.m02 = n1.z;
        viewMatrix.m03 = -Vector3.Dot(n1, camPos);
        viewMatrix.m10 = n2.x;
        viewMatrix.m11 = n2.y;
        viewMatrix.m12 = n2.z;
        viewMatrix.m13 = -Vector3.Dot(n2, camPos);
        viewMatrix.m20 = dir.x;
        viewMatrix.m21 = dir.y;
        viewMatrix.m22 = dir.z;
        viewMatrix.m23 = -Vector3.Dot(dir, camPos);
        viewMatrix.m30 = 0;
        viewMatrix.m31 = 0;
        viewMatrix.m32 = 0;
        viewMatrix.m33 = 1;
        return viewMatrix;
    }

    static Vector3 GetCamDirect(float angleX)
    {
        float cosX = Mathf.Cos(angleX * Mathf.Deg2Rad);
        float sinX = Mathf.Sin(angleX * Mathf.Deg2Rad);
        return new Vector3(0, sinX, -cosX).normalized;
    }

    #endregion
}