//======================================
//	Author: Matrix
//  Create Time：2018/3/9 15:40:27 
//  Function:
//======================================

using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraParam
{
    public enum CamMode
    {
        None,
        WorldMap,
        ArenaMap
    }
}

public class CameraInfoItem
{
    public string Id;

    public float dist; //相机距离

    public float fov; //相机视野范围

    public float dxf;

    public float angle;

    public Vector3 forward;

    public void Set(string strName, float _dist, float _fov, float _fAngle)
    {
        Id = strName;
        dist = _dist;
        fov = _fov;
        dxf = dist * fov;
        angle = _fAngle;
        forward = new Vector3(_fAngle, 0, 0);
    }
}