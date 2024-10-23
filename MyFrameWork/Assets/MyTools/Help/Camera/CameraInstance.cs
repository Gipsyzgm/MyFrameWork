//======================================
//	Author: Matrix
//  Create Time：2018/3/9 18:04:11 
//  Function:
//======================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class WorldMapCamera : UAbstractCamera
    {
        public override void Init(UnityEngine.Camera cam, Vector3 camPosFocusCenter,Vector3 mapCenter,float planeWidth, float planeHeight, float camInitFov, float angleX, float angleY)
        {
            base.Init(cam, camPosFocusCenter, mapCenter, planeWidth, planeHeight, camInitFov, angleX, angleY);
            camMode = CameraParam.CamMode.WorldMap;
            //cam.fieldOfView = 30;
            
            cam.transform.eulerAngles = new Vector3(angleX, angleY, 0);
            if (camTranslate != null)
            {
                camTranslate.enabled = true;
                camTranslate.planeBorderWidth = planeWidth;
                camTranslate.planeBorderHeight = planeHeight;
                camTranslate.mapCenter = mapCenter;

                camTranslate.camPosFocusCenter = camPosFocusCenter;
                cam.transform.position = camTranslate.camPosFocusCenter;
                
                camTranslate.TargetOffset = cam.transform.position - camTranslate.camPosFocusCenter;


                camTranslate.translateDirection = CameraTranslate.Direction.Both;
                camTranslate.damper = 6f;
                camTranslate.needHalf = false;
                camTranslate.needDistance = false;
                camTranslate.InitOffset();
            }
        }
        public override void SetCamera(Vector3 vPos, string strCameraItem)
        {
            base.SetCamera(vPos, strCameraItem);
            camTranslate.SetCamera(vPos, strCameraItem);
        }
        public override void SetCamera(Vector3 vPos)
        {
            base.SetCamera(vPos);
            camTranslate.SetCamera(vPos);
        }
        public override bool InCameraRange(Bounds bound)
        {
            return camTranslate.InCameraRange(bound);
        }
        public override void AutoZoomByDxf(float dxf,float time)
        {
            base.AutoZoomByDxf(dxf,time);
            camTranslate.AutoZoomByDxf(dxf, time);
        }
        public override void MoveCamera(float deltaX, float deltaY)
        {
            base.MoveCamera(deltaX, deltaY);
            camTranslate.Move(deltaX, deltaY);
        }
        public override bool IsInnerCity()
        {
            return camTranslate.dfSetting.IsInnerCity();
        }
        public override void Focus(Vector3 focusPos)
        {
            base.Focus(focusPos);
            camTranslate.Focus(focusPos);
        }
        public override void Focus(Vector3 focusPos, bool bZoom, bool exitInerCity, string useCameraItem)
        {
            base.Focus(focusPos, bZoom, exitInerCity, useCameraItem);
            camTranslate.Focus(focusPos, bZoom, exitInerCity,useCameraItem);
        }
        public override void Following(Transform tranTarget, bool bZoom, bool exitInerCity, string useCameraItem)
        {
            base.Following(tranTarget, bZoom, exitInerCity, useCameraItem);
            camTranslate.Following(tranTarget, bZoom, exitInerCity, useCameraItem);
        }

        public override void Focus(Vector3 focusCenter, float time, string useCameraItem = "")
        {
            base.Focus(focusCenter, time, useCameraItem);
            camTranslate.Focus(focusCenter, time, useCameraItem);
        }

        public override void ResetFocus()
        {
            base.ResetFocus();
        }
    }

    public class ArenaCamera : UAbstractCamera
    {
        public override void Init(UnityEngine.Camera cam, Vector3 camPosFocusCenter, Vector3 mapCenter, float planeWidth, float planeHeight, float camInitFov, float angleX, float angleY)
        {
            base.Init(cam, camPosFocusCenter, mapCenter, planeWidth, planeHeight, camInitFov, angleX, angleY);
            camMode = CameraParam.CamMode.ArenaMap;
            cam.fieldOfView = camInitFov;

            cam.transform.eulerAngles = new Vector3(angleX, angleY, 0);
            if (camTranslate != null)
            {
                camTranslate.enabled = true;
                camTranslate.planeBorderWidth = planeWidth;
                camTranslate.planeBorderHeight = planeHeight;
                camTranslate.mapCenter = mapCenter;
                camTranslate.camPosFocusCenter = camPosFocusCenter;
                cam.transform.position = camTranslate.camPosFocusCenter;

                camTranslate.TargetOffset = cam.transform.position - camTranslate.camPosFocusCenter;

                camTranslate.translateDirection = CameraTranslate.Direction.Both;
                camTranslate.damper = 6f;
                camTranslate.needHalf = true;
                camTranslate.needDistance = false;
                camTranslate.InitOffset();
                //SetCamera(camTranslate.mapCenter, CameraDFLevel.MAP_CITY);
            }
        }
        public override void SetCamera(Vector3 vPos, string strCameraItem)
        {
            base.SetCamera(vPos, strCameraItem);
            camTranslate.SetCamera(vPos, strCameraItem);
        }
        public override bool InCameraRange(Bounds bound)
        {
            return camTranslate.InCameraRange(bound);
        }
        public override void AutoZoomByDxf(float dxf, float time)
        {
            base.AutoZoomByDxf(dxf,time);
            camTranslate.AutoZoomByDxf(dxf,time);
        }
        public override void MoveCamera(float deltaX, float deltaY)
        {
            base.MoveCamera(deltaX, deltaY);
            camTranslate.Move(deltaX, deltaY);
        }

        public override void MoveCamera(Vector3 deltaPos)
        {
            base.MoveCamera(deltaPos);
            camTranslate.Move(deltaPos);
        }

        public override bool IsInnerCity()
        {
            return camTranslate.dfSetting.IsInnerCity();
        }
        public override void Focus(Vector3 focusPos)
        {
            base.Focus(focusPos);
            camTranslate.Focus(focusPos);
        }
        public override void Focus(Vector3 focusPos, bool bZoom, bool exitInerCity, string useCameraItem)
        {
            base.Focus(focusPos, bZoom, exitInerCity, useCameraItem);
            camTranslate.Focus(focusPos, bZoom, exitInerCity, useCameraItem);
        }
        public override void Following(Transform tranTarget, bool bZoom, bool exitInerCity, string useCameraItem)
        {
            base.Following(tranTarget, bZoom, exitInerCity, useCameraItem);
            camTranslate.Following(tranTarget, bZoom, exitInerCity, useCameraItem);
        }

        public override void Focus(Vector3 focusCenter, float time, string useCameraItem = "")
        {
            base.Focus(focusCenter, time, useCameraItem);
            camTranslate.Focus(focusCenter, time, useCameraItem);
        }

        public override void ResetFocus()
        {
            base.ResetFocus();
        }
    }

