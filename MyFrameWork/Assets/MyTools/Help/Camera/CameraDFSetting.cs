using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraDFLevel
{
    public const string LOWEST = "limit_min"; //最低
    public const string MAP_LOWEST = "map_limit_min"; //沙盘最低
    public const string INNER_CITY = "min"; //内城默认
    public const string MAP_MIN = "map_min"; //内城默认
    public const string INNER_CITY_BOUNDARY = "init"; //内外城分界线
    public const string MAP_CITY = "city"; //沙盘默认城市，缩成单个城市的时候
    public const string MAP_VIEW = "max"; //沙盘抬高,是否可以删除？后续考虑和lod绑定成一条线？
    public const string HIGHEST = "limit_max"; //最高
}

public class CameraDFSetting
{
    public Dictionary<string, CameraInfoItem> dictCameraItem = new Dictionary<string, CameraInfoItem>();
    public List<CameraInfoItem> cameraItemList = new List<CameraInfoItem>();

    public float cameraDistance = 0.0f; //-30.0f;
    public Action OnCameraZoomEvent;
    public Action AutoZoomCameraFinishEvent;
    int curFrameIndex = 0;
    float curDxF = 0;

    private float m_additionHeightForMinDxf = 0.0f;
    private float m_addDxfforAdditionHeight_delta = 0.0f;
    public float innerCityLine = 0.0f;

    public bool enableCamZoom = false;
    private float cameraDistanceTime = 0.0f;
    public float cameraDistanceTotalTime = 0.0f;
    public float zoomStartDxf = 0.0f;
    public float zoomEndDxf = 0.0f;

    public float customMaxDxf = 20000.0f;

    public float customMinDxf = 1.0f;
    public Dictionary<int, string> dfLvSortDict = new Dictionary<int, string>();

    public bool needChangeLowest = false;

    void InitDfLvSort()
    {
        dfLvSortDict.Clear();
        dfLvSortDict.Add(0, CameraDFLevel.LOWEST);
        dfLvSortDict.Add(1, CameraDFLevel.INNER_CITY);
        dfLvSortDict.Add(2, CameraDFLevel.INNER_CITY_BOUNDARY);
        dfLvSortDict.Add(3, CameraDFLevel.MAP_CITY);
        dfLvSortDict.Add(4, CameraDFLevel.MAP_VIEW);
        dfLvSortDict.Add(5, CameraDFLevel.HIGHEST);
    }

    #region CameraInfoItem

    public void AddCameraItem(string key, CameraInfoItem item)
    {
        dictCameraItem.TryAdd(key, item);
    }

    public void UpdateCameraItem(string key, CameraInfoItem item)
    {
        if (!dictCameraItem.TryAdd(key, item))
        {
            dictCameraItem[key] = item;
        }
    }

    public void SetDxfRange(float minValue, float maxValue)
    {
        customMinDxf = Mathf.Max(minValue, 1);
        customMaxDxf = Mathf.Min(maxValue, 20000);
    }

    public CameraInfoItem Get(string key)
    {
        CameraInfoItem value = null;
        if ((key == CameraDFLevel.LOWEST || key == CameraDFLevel.INNER_CITY) && needChangeLowest && !IsInnerCity())
        {
            if (key == CameraDFLevel.LOWEST)
            {
                if (!dictCameraItem.TryGetValue(CameraDFLevel.MAP_LOWEST, out value))
                {
                    Debug.LogError("can't find camera item " + CameraDFLevel.MAP_LOWEST);
                    if (!dictCameraItem.TryGetValue(key, out value))
                    {
                        Debug.LogError("can't find camera item " + key);
                    }
                }
            }
            else if (key == CameraDFLevel.INNER_CITY)
            {
                if (!dictCameraItem.TryGetValue(CameraDFLevel.MAP_MIN, out value))
                {
                    Debug.LogError("can't find camera item " + CameraDFLevel.MAP_MIN);
                    if (!dictCameraItem.TryGetValue(key, out value))
                    {
                        Debug.LogError("can't find camera item " + key);
                    }
                }
            }
        }
        else
        {
            if (!dictCameraItem.TryGetValue(key, out value))
            {
                Debug.LogError("can't find camera item "+key);
            }
        }

        return value;
    }

    public void ChangeCameraItem(string camerItemKey)
    {
        CameraInfoItem cameraInfo = Get(camerItemKey);
        if (cameraInfo == null)
        {
            Debug.LogError("cameraInfo is null");
            return;
        }

        cameraDistance = cameraInfo.dist;
        SetFov(cameraInfo.fov);
    }

    public void Init(bool _needChangeLowest = false, float innerCityCamDeltaDxf = 1)
    {
        needChangeLowest = _needChangeLowest;
        InitDfLvSort();
        cameraDistance = Get(CameraDFLevel.INNER_CITY).dist;
        SetFov(Get(CameraDFLevel.INNER_CITY).fov);

        SetAdditionHeightForMinDxf(GetCurFrameDxf());

        innerCityLine = Get(CameraDFLevel.INNER_CITY_BOUNDARY).dxf + innerCityCamDeltaDxf;
    }

    public float GetCurFrameDxf()
    {
        if (Time.frameCount == curFrameIndex)
        {
            return curDxF;
        }

        curDxF = CameraMgr.Instance.activeCamera.camera.fieldOfView * cameraDistance;
        //UDebug.LogError("fov: "+CameraMgr.Instance.activeCamera.camera.fieldOfView+"  dist:"+ cameraDistance+ "     curDxF      "+ curDxF);
        return curDxF;
    }

    public float GetCurrentCameraDistance()
    {
        return cameraDistance;
    }

    public float GetAdditionHeightBaseInnerCity(string key)
    {
        return Get(key).dxf + m_additionHeightForMinDxf * (Get(key).dxf / Get(CameraDFLevel.LOWEST).dxf);
    }

    public void SetCameraByDxf(float dxf)
    {
        float fov3 = 0f;
        float dist3 = 0f;
        if (GetDistFovByDxf(dxf, out dist3, out fov3))
        {
            cameraDistance = dist3;

            CameraMgr.Instance.activeCamera.camera.fieldOfView = fov3;
            SetAdditionHeightForMinDxf(GetCurFrameDxf() - Get(CameraDFLevel.LOWEST).dxf);

            if (OnCameraZoomEvent != null)
            {
                OnCameraZoomEvent();
            }
        }
    }

    void SetFov(float _fov)
    {
        CameraMgr.Instance.activeCamera.camera.fieldOfView = _fov;
    }

    void SetAdditionHeightForMinDxf(float value)
    {
        m_addDxfforAdditionHeight_delta = value - m_additionHeightForMinDxf;
        if (m_addDxfforAdditionHeight_delta < 0f)
        {
            m_addDxfforAdditionHeight_delta = 0f;
        }

        m_additionHeightForMinDxf = value;
    }

    public bool IsInnerCity()
    {
        bool bInInner = false;
        if (innerCityLine > GetCurFrameDxf())
        {
            bInInner = true;
        }

        return bInInner;
    }

    public void SetZoomCameraParam(float destdistance, float time)
    {
        zoomStartDxf = GetCurFrameDxf(); // - destCameraDistance;
        cameraDistanceTotalTime = time;
        zoomEndDxf = destdistance;
        enableCamZoom = true;
        cameraDistanceTime = 0;
    }

    public bool AutoDistanceProcess()
    {
        float fDeltaTime = Time.deltaTime;
        if (fDeltaTime > Time.fixedDeltaTime)
        {
            fDeltaTime = Time.fixedDeltaTime;
        }

        cameraDistanceTime += fDeltaTime;
        if (cameraDistanceTime > cameraDistanceTotalTime)
        {
            SetCameraByDxf(zoomEndDxf);
            if (AutoZoomCameraFinishEvent != null)
            {
                AutoZoomCameraFinishEvent();
            }

            //UDebug.Log(()=>"AutoZoomCameraFinishEvent cameraDistance:" + cameraDistance + "  fov:" + CameraMgr.Instance.activeCamera.camera.fieldOfView
            //     + "   zoomEndDxf:" + zoomEndDxf);
            CameraMgr.Instance.activeCamera.camTranslate.cameraTranslateState = CameraTranslateState.Still;
            cameraDistanceTime = 0.0f;
            enableCamZoom = false;
            return true;
        }

        float num2 = (zoomEndDxf - zoomStartDxf);
        float num3 = (cameraDistanceTime / cameraDistanceTotalTime);

        float num4 = 0.7f;
        float num5 = 1f - num4;
        if (num3 < num4)
        {
            num3 /= num4;
            num3 *= num3;
            num3 *= num4;
        }
        else
        {
            num3 = (num3 - num4) / num5;
            num3 = Mathf.Sqrt(num3);
            num3 = num3 * num5 + num4;
        }

        float
            dxf = zoomStartDxf +
                  num2 * num3; // (zoomEndDxf - zoomStartDxf)*(cameraDistanceTime / cameraDistanceTotalTime);
        SetCameraByDxf(dxf);
        CameraMgr.Instance.activeCamera.camTranslate.cameraTranslateState = CameraTranslateState.FocusZoom;
        return false;
    }

    bool GetDistFovByDxf(float dxf, out float dist, out float fov)
    {
        dist = 0f;
        fov = 0f;
        CameraInfoItem lowestItem = Get(CameraDFLevel.LOWEST);
        CameraInfoItem highestItem = Get(CameraDFLevel.HIGHEST);
        float dxfMax = highestItem.dxf;
        if (customMaxDxf > 0f && dxfMax > customMaxDxf)
        {
            dxfMax = customMaxDxf;
        }

        float dxfMin = lowestItem.dxf;
        float num = m_additionHeightForMinDxf; // m_additionHeightForMinDxf;
        if (customMinDxf > 0f && dxfMin < customMinDxf)
        {
            dxfMin = customMinDxf;
            num = 0f;
        }

        // dxf = Mathf.Max(dxf3 + num, Mathf.Min(dxf, dxf2));
        if (dxf <= lowestItem.dxf)
        {
            dist = lowestItem.dist;
            fov = lowestItem.fov;
            return true;
        }

        if (dxf >= highestItem.dxf)
        {
            dist = highestItem.dist;
            fov = highestItem.fov;
            return true;
        }

        CameraInfoItem cameraInfoItem3 = null;
        CameraInfoItem cameraInfoItem4 = null;
        int dfLvCount = dfLvSortDict.Count;
        for (int i = 0; i < dfLvCount - 1; i++)
        {
            cameraInfoItem3 = Get(dfLvSortDict[i]);
            cameraInfoItem4 = Get(dfLvSortDict[i + 1]);
            if (dxf == cameraInfoItem4.dxf)
            {
                dist = cameraInfoItem4.dist;
                fov = cameraInfoItem4.fov;
                return true;
            }

            if (dxf > cameraInfoItem3.dxf && dxf < cameraInfoItem4.dxf && dxf <= dxfMax && dxf >= dxfMin)
            {
                double dist2 = 0.0;
                double fov2 = 0.0;
                if (GetDistFovWithTwoCameraInfo(cameraInfoItem3.dist, cameraInfoItem3.fov, cameraInfoItem4.dist,
                        cameraInfoItem4.fov, dxf, out dist2, out fov2))
                {
                    dist = (float)dist2;
                    fov = (float)fov2;
                    return true;
                }
            }
        }

        return false;
    }

    bool GetDistFovWithTwoCameraInfo(double dist_min, double fov_min, double dist_max, double fov_max, double dxf,
        out double dist, out double fov)
    {
        dist = 0.0;
        fov = 0.0;
        if (dist_min * fov_min > dist_max * fov_max)
        {
            return false;
        }

        if (dxf < dist_min * fov_min || dxf > dist_max * fov_max)
        {
            return false;
        }

        if (dxf == dist_min * fov_min)
        {
            dist = dist_min;
            fov = fov_min;
            return true;
        }

        if (dxf == dist_max * fov_max)
        {
            dist = dist_max;
            fov = fov_max;
            return true;
        }

        if (dist_max == dist_min)
        {
            dist = dist_min;
            fov = dxf / dist_min;
            return true;
        }

        if (fov_max == fov_min)
        {
            fov = fov_min;
            dist = dxf / fov_min;
            return true;
        }

        //double num = -1.0;
        //double num2 = -1.0;
        double num3 = 0.0;
        double num4 = 1.0;
        double num5 = 0.0;
        double num6 = 0.0;
        double num7 = -1.0;
        num5 = (dist_min + num3 * (dist_max - dist_min)) * (fov_min + num3 * (fov_max - fov_min));
        num6 = (dist_min + num4 * (dist_max - dist_min)) * (fov_min + num4 * (fov_max - fov_min));
        for (int i = 0; i < 24; i++)
        {
            if (dxf == num5)
            {
                num7 = num3;
                break;
            }

            if (dxf == num6)
            {
                num7 = num4;
                break;
            }

            double num8 = (num4 + num3) / 2.0;
            double num9 = (dist_min + num8 * (dist_max - dist_min)) * (fov_min + num8 * (fov_max - fov_min));
            if (dxf == num9)
            {
                num7 = num8;
                break;
            }

            if (dxf > num9)
            {
                if (num3 == num8)
                {
                    num7 = num8;
                    break;
                }

                num3 = num8;
                num5 = num9;
            }
            else if (dxf < num9)
            {
                if (num4 == num8)
                {
                    num7 = num8;
                    break;
                }

                num4 = num8;
                num6 = num9;
            }
        }

        if (num7 == -1.0)
        {
            num7 = num3;
        }

        dist = dist_min + num7 * (dist_max - dist_min);
        fov = fov_min + num7 * (fov_max - fov_min);
        //UDebug.LogError("dist_min:"+ dist_min+ "  dist_max:" + dist_max + " fov_min:" + fov_min+ " fov_max：" + fov_max +"  dxf:" + dxf+" dist:" +dist+"  fov:"+fov);
        return true;
    }

    #endregion
}