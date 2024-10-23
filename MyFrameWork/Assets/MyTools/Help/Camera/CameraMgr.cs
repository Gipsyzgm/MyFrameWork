//======================================
//	Author: Matrix
//  Create Time：2018/3/9 15:33:02 
//  Function:
//======================================

using UnityEngine;


public class CameraMgr : Singleton<CameraMgr>
{
    public UAbstractCamera activeCamera;

    public void Init(CameraParam.CamMode camMode, Camera cam, Vector3 camPosFocusCenter, Vector3 mapCenter,
        float planeWidth, float planeHeight, float nearClip, float camInitFov = 30, float angleX = 48.32f,
        float angleY = 0)
    {
        if (activeCamera != null && activeCamera.camMode != camMode) activeCamera.Reset();
        switch (camMode)
        {
            case CameraParam.CamMode.WorldMap:
                if (activeCamera == null || activeCamera.camMode != CameraParam.CamMode.WorldMap)
                    activeCamera = new WorldMapCamera();
                else
                    activeCamera.ResetFocus();
                break;
            case CameraParam.CamMode.ArenaMap:
                if (activeCamera == null || activeCamera.camMode != CameraParam.CamMode.ArenaMap)
                    activeCamera = new ArenaCamera();
                else
                    activeCamera.ResetFocus();
                break;
        }

        if (activeCamera != null && !activeCamera.inited)
        {
            cam.nearClipPlane = nearClip;
            activeCamera.Init(cam, camPosFocusCenter, mapCenter, planeWidth, planeHeight, camInitFov, angleX,
                angleY);
            activeCamera.OnCameraForceBeginEvent -= CameraForceBegin;
            activeCamera.OnCameraForceEndEvent -= CameraForceEnd;
            activeCamera.OnCameraForceBeginEvent += CameraForceBegin;
            activeCamera.OnCameraForceEndEvent += CameraForceEnd;
        }
    }

    public void AutoZoomByDxf(float dxf, float time)
    {
        activeCamera.AutoZoomByDxf(dxf, time);
    }

    public void SetCamera(Vector3 vPos, string strCameraItem)
    {
        activeCamera.SetCamera(vPos, strCameraItem);
    }

    public void SetCamera(Vector3 vPos)
    {
        activeCamera.SetCamera(vPos);
    }

    public void Focus(Vector3 targetPos, bool bZoom, bool exitInnerCity, string useCameraItem)
    {
        activeCamera.Focus(targetPos, bZoom, exitInnerCity, useCameraItem);
    }

    public void Focus(Vector3 focusCenter, float time, string useCameraItem = "")
    {
        activeCamera.Focus(focusCenter, time, useCameraItem);
    }

    public void Focus(Vector3 focusPosition)
    {
        activeCamera.Focus(focusPosition);
    }

    public void Following(Transform tranTarget, bool bZoom, bool exitInnerCity, string useCameraItem)
    {
        activeCamera.Following(tranTarget, bZoom, exitInnerCity, useCameraItem);
    }

    public void MoveCamera(float deltaX, float deltaY)
    {
        activeCamera.MoveCamera(deltaX, deltaY);
    }

    public void EnableCameraDrag(bool enable)
    {
        if (activeCamera == null) return;
        activeCamera.camTranslate.MovingEnabled = enable;
    }

    public bool GetEnableCameraDrag()
    {
        if (activeCamera == null) return false;
        return activeCamera.camTranslate.MovingEnabled;
    }

    public Vector3 GetScreenCenterInMap()
    {
        return activeCamera.GetFocusPos();
    }

    public void SetMoveRate(float moveRate)
    {
        activeCamera.camTranslate.moveRate = moveRate;
    }

    public void SetZoomRate(float zoomRate)
    {
        activeCamera.camTranslate.zoomRate = zoomRate;
    }

    public void SetEnlargeFrustumPlanesParams(float height, float coaf)
    {
        activeCamera.camTranslate.SetEnlargeFrustumPlanesParams(height, coaf);
    }

    public void CameraForceBegin()
    {
        UInput.Instance.Enabled = false;
    }

    public void CameraForceEnd()
    {
        UInput.Instance.Enabled = true;
    }
}