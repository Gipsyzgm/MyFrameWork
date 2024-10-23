//======================================
//	Author: Matrix
//  Create Time：2018/3/9 15:51:06 
//  Function:
//======================================

using System;
using System.Collections.Generic;
using UnityEngine;

    public interface ICamera
    {
        void Init(UnityEngine.Camera cam, Vector3 camPosFocusCenter, Vector3 mapCenter, float planeWidth, float planeHeight, float camInitFov, float angleX, float angleY);
    }

    public abstract class UAbstractCamera : ICamera
    {
        public CameraParam.CamMode camMode;
        public CameraTranslate camTranslate;
        public Camera camera;
        public bool inited = false;

        public Action OnCameraForceBeginEvent;
        public Action OnCameraForceEndEvent;

        public virtual void Init(UnityEngine.Camera cam, Vector3 camPosFocusCenter, Vector3 mapCenter,float planeWidth, float planeHeight, float camInitFov, float angleX, float angleY)
        {
            camera = cam;
            if (camTranslate == null)
            {
                if (cam.gameObject.GetComponent<CameraTranslate>())
                {
                    camTranslate = cam.gameObject.GetComponent<CameraTranslate>();
                }
                else
                {
                    camTranslate = cam.gameObject.AddComponent<CameraTranslate>();
                }

            }
            
            camTranslate.FoucesFinishEvent += CameraForceEnd;

            OnCameraForceBeginEvent = null;
            OnCameraForceEndEvent = null;

            inited = true;
        }

        
        public virtual void Reset()
        {
            OnCameraForceBeginEvent = null;
            OnCameraForceEndEvent = null;

            if (camTranslate != null)
            {
                camTranslate.FoucesFinishEvent -= CameraForceEnd;
            }

            inited = false;
        }
        public virtual void Focus(Vector3 focusPos, bool bZoom, bool exitInerCity, string useCameraItem)
        {
            CameraForceBegin();
        }

        public virtual void Focus(Vector3 focusCenter, float time, string useCameraItem = "")
        {
            CameraForceBegin();
        }

        public virtual void Following(Transform tranTarget, bool bZoom, bool exitInerCity, string useCameraItem)
        {
        }
        /// <summary>
        /// 单纯聚焦不改变相机其他信息
        /// </summary>
        /// <param name="focusPos"></param>
        public virtual void Focus(Vector3 focusPos)
        {
            CameraForceBegin();
        }
        public virtual void SetCamera(Vector3 vPos, string strCameraItem)
        {
            
        }
        public virtual void SetCamera(Vector3 vPos)
        {

        }

        public virtual void MoveCamera(float deltaX, float deltaY)
        {

        }

        public virtual void MoveCamera(Vector3 deltaPos)
        {

        }

        public virtual bool IsInnerCity()
        {
            return false;
        }
        public virtual bool InCameraRange(Bounds bound)
        {
            return true;
        }

        public virtual void AutoZoomByDxf(float dxf,float time)
        {

        }
        public virtual void ResetFocus()
        {

        }
        public Vector3 GetFocusPos()
        {
            return camTranslate.camFocusPos;
        }

        public virtual void CameraForceBegin()
        {
            OnCameraForceBeginEvent?.Invoke();
        }

        public virtual void CameraForceEnd()
        {
            OnCameraForceEndEvent?.Invoke();
        }

       
    }

