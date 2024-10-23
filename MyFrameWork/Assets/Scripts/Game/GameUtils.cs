using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static float BlockWidth = 5 * Mathf.Sqrt(3);
    public static float BlockHeight = 5;
    public static Vector3 XDirection = new(Mathf.Sqrt(3), 0, 1);
    public static Vector3 cameraStartPos = new(272.5f, 270, -272.5f);

    public static float GetAngleByXY(float x, float y)
    {
        if (x == 0) return y > 0 ? 90 : 270;
        var angle = Mathf.Atan2(y, x);
        angle = angle * Mathf.Rad2Deg;
        if (angle > 360) angle -= 360;
        if (angle < 0) angle += 360;
        return angle;
    }

    public static int GetDirectionByVector(Vector3 vector)
    {
        var angle = GetAngleByXY(-vector.x, -vector.z);
        return AngleToDirection(Mathf.Round(angle));
    }

    public static int AngleToDirection(float angle)
    {
        var direction = Mathf.RoundToInt((angle - 90) / 60);
        if (direction <= 0) direction += 6;
        return direction;
    }

    public static void Get2DXYByPosition(Vector3 position, out int x, out int y)
    {
        Get3DXYByPosition(position, out x, out y);
        Get2DBy3D(ref x, ref y);
    }

    public static void Get3DXYByPosition(Vector3 position, out int x, out int y)
    {
        x = Mathf.RoundToInt(position.x / BlockWidth);
        y = Mathf.RoundToInt((position.z / BlockHeight - x) / 2);
    }

    public static void Get2DBy3D(ref int x, ref int y)
    {
        y = y + Mathf.FloorToInt(x / 2);
    }

    public static void Get3DBy2D(ref int x, ref int y, ref int z)
    {
        y = y - Mathf.FloorToInt(x / 2);
        z = -x - y;
    }

    public static void Get3DBy2D(ref int x, ref int y)
    {
        y = y - Mathf.FloorToInt(x / 2);
    }

    /// <summary>
    ///     根据xy获取世界坐标
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 GetPositionBy2DXY(int x, int y)
    {
        Get3DBy2D(ref x, ref y);
        return GetPositionBy3DXY(x, y);
    }

    /// <summary>
    ///     根据xy获取世界坐标
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 GetPositionBy3DXY(int x, int y)
    {
        return new Vector3(BlockWidth * x, 0, BlockHeight * (x + 2 * y));
    }

    public static int GetDistanceBy2DXY(int x1, int y1, int x2, int y2)
    {
        var z1 = 0;
        var z2 = 0;
        Get3DBy2D(ref x1, ref y1, ref z1);
        Get3DBy2D(ref x2, ref y2, ref z2);
        return GetDistanceBy3DXYZ(x1, y1, z1, x2, y2, z2);
    }

    public static int GetDistanceBy3DXYZ(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        var deltaX = Mathf.Abs(x1 - x2);
        var deltaY = Mathf.Abs(y1 - y2);
        var deltaZ = Mathf.Abs(z1 - z2);
        return (deltaX + deltaY + deltaZ) / 2;
    }

    /// <summary>
    ///     点到直线距离
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static float GetDistancePointToLine(Vector3 start, Vector3 end, Vector3 point)
    {
        var v1 = end - start;
        var v2 = point - start;
        var a = Vector3.Dot(v2, v1);
        var c = v2.magnitude;
        var b = Mathf.Sqrt(c * c - a * a);
        return b;
    }

    public static float GetDistancePower(Vector3 a, Vector3 b)
    {
        var dx = a.x - b.x;
        var dz = a.z - b.z;
        return dx * dx + dz * dz;
    }

    public static float GetDistance(Vector3 a, Vector3 b)
    {
        var dx = a.x - b.x;
        var dz = a.z - b.z;
        return Mathf.Sqrt(dx * dx + dz * dz);
    }

    public static void SortPolygonPoints(List<Vector3> points)
    {
        var center = Vector3.zero;
        foreach (var p in points) center += p;
        center /= points.Count;

        points.Sort((a, b) =>
        {
            if (ComparePolygonPoint(a, b, center))
                return 1;
            return -1;
        });
    }

    private static bool ComparePolygonPoint(Vector3 a, Vector3 b, Vector3 center)
    {
        if (a.x >= 0 && b.x < 0) return true;

        if (a.x == 0 && b.x == 0) return a.y > b.y;

        var value = (a.x - center.x) * (b.z - center.z) - (b.x - center.x) * (a.z - center.z);
        if (value < 0) return true;
        if (value > 0) return false;
        var d1 = (a.x - center.x) * (a.x - center.x) + (a.z - center.z) * (a.z - center.z);
        var d2 = (b.x - center.x) * (b.x - center.x) + (b.z - center.z) * (b.z - center.z);
        return d1 > d2;
    }
}