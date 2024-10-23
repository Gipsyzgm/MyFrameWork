//======================================
//	Author: Matrix
//  Create Time：2018/3/9 19:07:03 
//  Function:
//======================================

using System;
using System.Collections.Generic;
using UnityEngine;

    public enum CameraTranslateState
    {
        Still,
        FocusMove,
        FocusZoom
    }
    public class CameraTranslate : MonoBehaviour
    {
        public CameraDFSetting dfSetting = new CameraDFSetting();
        public static float INVALID_FLOAT_VALUE = -999f;

        public const float defaultCameraY = 24;
        public const float minCameraY = 16;
        public const float maxCameraY = 32;
        public const float maxCameraDistance = 1500.0f;
        public const float minCameraDistance = -30.0f;


        #region temp
        public bool needHalf = false;
        public bool needDistance = true;
        #endregion
        public float planeBorderWidth;
        public float planeBorderHeight;
        public Vector3 mapCenter;
        private Vector3 _camPosFocusCenter;

        public Vector3 camPosFocusCenter
        {
            get { return _camPosFocusCenter; }
            set
            {
                _camPosFocusCenter = value;
                camDistanceFocusCenter = Vector3.Distance(camPosFocusCenter, mapCenter);
            }
        }
        private Vector3 targetOffset = Vector3.zero;
        private float camDistanceFocusCenter;
        public UnityEngine.Vector3 TargetOffset
        {
            get { return targetOffset; }
            set 
            {
                targetOffset = value;
            }
        }

        public float moveRate = 1f;
        public float zoomRate = 1f;
        private Vector3 _currentOffset;
        private Vector3 currentOffset
        {
            get { return _currentOffset; }
            set
            {
                _currentOffset = value;
            }
        }
        public float damper = 1;

        public Action FoucesFinishEvent;


        public CameraTranslateState cameraTranslateState = CameraTranslateState.Still;
        public void AddStopMovingEvent(Action action)
        {
            FoucesFinishEvent += action;
        }
        public void RemoveStopMovingEvent(Action action)
        {
            FoucesFinishEvent -= action;
        }

        public Action OnCameraFocusStartEvent;
        public Action OnCameraMoveEvent;
        public Action OnCameraMoveEndEvent;
        public Vector3 camFocusPos;

        float ZOOM_REBOUND_SPEED = 0.0f;
        public bool bCameraZoomFirst = false;

        private Plane[] planes;

        private bool canCameraShake = false;

        private float shakeAmount = 1;

        private string curLodParam = "";
        public enum Direction
        {
            Horizontal,
            Vertical,
            Both
        }
        public Direction translateDirection = Direction.Horizontal;
        void Start()
        {
            Init();
        }
        Vector3 velocity = Vector3.zero;
        private void LateUpdate()
        {
            if (!bCameraZoomFirst && inFollowing && tranFollow != null)
            {
                destFocusCenter = tranFollow.transform.position;
                ChangeFocus();
            }
        
        }

        // 扩大剪裁空间相关
        float heightForEnlargeFrustumPlanes = 80f;
        float coafForEnlargeFrustumPlanes = 0.1f;
        public void SetEnlargeFrustumPlanesParams(float height, float coaf)
        {
            heightForEnlargeFrustumPlanes = height;
            coafForEnlargeFrustumPlanes = coaf;
        }
        private void GemCullRange()
        {
            if (planes == null) return;
            float cameraDistance = dfSetting.cameraDistance;

            if (cameraDistance <= heightForEnlargeFrustumPlanes)
            {
                if (cameraTranslateState == CameraTranslateState.FocusMove)
                {
                    UTool.GemCullRange(DestFocusCenter - 1.0f * CameraMgr.Instance.activeCamera.camera.transform.forward * dfSetting.cameraDistance
                        , CameraMgr.Instance.activeCamera.camera.fieldOfView
                        , transform.eulerAngles.x
                        , planes);
                }
                else
                {
                    GeometryUtility.CalculateFrustumPlanes(CameraMgr.Instance.activeCamera.camera, planes);
                }
            }
            else
            {
                float coaf = (cameraDistance - heightForEnlargeFrustumPlanes) * coafForEnlargeFrustumPlanes + 1;
                if (cameraTranslateState == CameraTranslateState.FocusMove)
                {
                    UTool.GemCullRange(DestFocusCenter - 1.0f * CameraMgr.Instance.activeCamera.camera.transform.forward * dfSetting.cameraDistance
    , CameraMgr.Instance.activeCamera.camera.fieldOfView * coaf
    , transform.eulerAngles.x
    , planes);
                }
                else
                {
                    UTool.GemCullRange(CameraMgr.Instance.activeCamera.camera.transform.position
    , CameraMgr.Instance.activeCamera.camera.fieldOfView * coaf
    , transform.eulerAngles.x
    , planes);
                }

            }
        }

        public bool InCameraRange(Bounds bound)
        {
            bool result = GeometryUtility.TestPlanesAABB(planes, bound);
            return result;
        }

        

        private void OnDestroy()
        {
            UInput.Instance.OnMouseFingerMoveEvent -= OnMouseDrag;
            //UInput.Instance.OnZoomEvent -= OnZoomCam;
            UInput.Instance.OnClickDownAnyScreenWithoutObjectAndUI -= CancelFollowing;
            UInput.Instance.OnClickDownBlockArea -= CancelFollowing1;
            UInput.Instance.OnClickObjectEvent -= CancelFollowing2;
            UInput.Instance.OnZoomMaxMinEvent -= OnZoomMaxMin;
            //UInput.Instance.OnDragBegin -= OnDragBegin;
            UInput.Instance.OnDragEnd -= OnDragEnd;
        }
        void Init()
        {
            UInput.Instance.OnZoomMaxMinEvent += OnZoomMaxMin;
            UInput.Instance.OnMouseFingerMoveEvent += OnMouseDrag;
            UInput.Instance.OnClickDownAnyScreenWithoutObjectAndUI += CancelFollowing;
            UInput.Instance.OnClickDownBlockArea += CancelFollowing1;
            UInput.Instance.OnClickObjectEvent += CancelFollowing2;
            //UInput.Instance.OnZoomEvent += OnZoomCam;
            planes = new Plane[6];
            //UInput.Instance.OnDragBegin += OnDragBegin;
            UInput.Instance.OnDragEnd += OnDragEnd;


            // AddCameraInfoList(cameraInfo_limit_max);
        }

        public void SetCamera(Vector3 vPos, string camerItemKey)
        {
            TargetOffset = vPos-mapCenter;
            InitOffset();
            dfSetting.ChangeCameraItem(camerItemKey);
        }

        public void SetCamera(Vector3 vPos)
        {
            transform.position = vPos;
        }
 
        private bool movingEnable = true;
        public bool MovingEnabled
        {
            get { return movingEnable; }
            set { movingEnable = value; }
        }
        private bool mIsFocusing = false;
        public bool IsFocusing
        {
            get { return mIsFocusing; }
            private set
            {
                mIsFocusing = value;
            }
        }
        private Vector3 destFocusCenter = Vector3.zero;
        public UnityEngine.Vector3 DestFocusCenter
        {
            get { return destFocusCenter; }
        }
        public void ChangeFocus()
        {
            TargetOffset = DestFocusCenter - mapCenter;
            camFocusPos = DestFocusCenter;
        }

        public void ProcessAutoCamera()
        {
            if (IsFocusing)
            {
                if (bCameraZoomFirst)
                {
                    if (dfSetting.enableCamZoom && dfSetting.AutoDistanceProcess())
                    {
                        bCameraZoomFirst = false;
                        ChangeFocus();
                    }
                }
                else
                {
                    if ((TargetOffset - currentOffset).sqrMagnitude < 0.00001f)
                    {
                        if (!dfSetting.enableCamZoom || dfSetting.AutoDistanceProcess())
                        {
                            IsFocusing = false;
                            FoucesFinishEvent?.Invoke();
                            cameraTranslateState = CameraTranslateState.Still;
                        }
                    }
                }
            }
        }

        public void AutoZoomByDxf(float dxf,float time)
        {
            //if (!IsFocusing)
            {
                IsFocusing = true;
                bCameraZoomFirst = false;
                dfSetting.SetZoomCameraParam(dxf, time);
            }
        }

        bool inFollowing = false;
        Transform tranFollow;
        public void Following(Transform tranTarget, bool bZoom, bool exitInerCity, string useCameraItem)
        {
            inFollowing = true;
            FoucesFinishEvent?.Invoke();
            tranFollow = tranTarget;
            _Focus(tranTarget.transform.position, bZoom, exitInerCity, useCameraItem);
        }
        public void Focus(Vector3 focusCenter)
        {
            Focus(focusCenter, false, false, curLodParam);
        }
        public void Focus(Vector3 focusCenter, bool bZoom, bool exitInerCity, string useCameraItem)
        {
            CancelFollowing();
            OnCameraFocusStartEvent?.Invoke();
            _Focus(focusCenter, bZoom, exitInerCity, useCameraItem);

        }
        public void FocusNoDelay(Vector3 focusCenter)
        {
            currentOffset = TargetOffset = focusCenter - mapCenter;
            float dxf = dfSetting.Get(CameraDFLevel.MAP_CITY).dxf;
            AutoZoomByDxf(dxf, 0f);
            transform.position = camPosFocusCenter + currentOffset + -1.0f * CameraMgr.Instance.activeCamera.camera.transform.forward * (dfSetting.cameraDistance - camDistanceFocusCenter);
        }
        void _Focus(Vector3 focusCenter, bool bZoom, bool exitInerCity, string useCameraItem)
        {
            IsFocusing = true;
            curLodParam = useCameraItem;
            destFocusCenter = focusCenter;
            bool bInInner = dfSetting.IsInnerCity();
            if (dfSetting.IsInnerCity() && !bZoom && !exitInerCity) //在内城
            {
                ChangeFocus();
                bCameraZoomFirst = false;
            }
            else
            {
                float fInitDxf = dfSetting.dictCameraItem[CameraDFLevel.MAP_CITY].dxf;
                if (useCameraItem != "")
                {
                    if (dfSetting.Get(useCameraItem) != null)
                    {
                        fInitDxf = dfSetting.Get(useCameraItem).dxf;
                    }
                }
                float fZoomSpeed = 0.5f;
                if (bInInner)
                {
                    fZoomSpeed = 0.3f;
                }
                float fCurDxf = dfSetting.GetCurFrameDxf();
                if (fInitDxf > fCurDxf)
                {
                    bCameraZoomFirst = true;
                    dfSetting.SetZoomCameraParam(fInitDxf, fZoomSpeed);
                }
                else
                {
                    bCameraZoomFirst = false;
                    ChangeFocus();
                    if (bZoom)
                    {
                        dfSetting.SetZoomCameraParam(fInitDxf, fZoomSpeed);
                    }
                }
            }

            // Debug.Log("Focus " + focusCenter.ToString() + " " + useCameraItem);


        }


        public void Focus(Vector3 focusCenter, float time, string useCameraItem)
        {
            CancelFollowing();
            _Focus(focusCenter, time, useCameraItem);
        }
        void _Focus(Vector3 focusCenter, float time, string useCameraItem)
        {
            IsFocusing = true;
            curLodParam = useCameraItem;
            destFocusCenter = focusCenter;

            float fInitDxf = dfSetting.dictCameraItem[CameraDFLevel.MAP_CITY].dxf;
            if (useCameraItem != "") {
                if (dfSetting.Get(useCameraItem) != null) {
                    fInitDxf = dfSetting.Get(useCameraItem).dxf;
                }
            }
            float fCurDxf = dfSetting.GetCurFrameDxf();
            if (fInitDxf > fCurDxf) {
                bCameraZoomFirst = true;
                dfSetting.SetZoomCameraParam(fInitDxf, time);
            } else {
                bCameraZoomFirst = false;
                ChangeFocus();
                dfSetting.SetZoomCameraParam(fInitDxf, time);
            }
        }

        public void InitOffset()
        {
            currentOffset = TargetOffset;
            //IsFocusing = false;
            //inFollowing = false;
        }
        void CancelFollowing2(GameObject obj)
        {
            CancelFollowing();
        }
        void CancelFollowing1(Vector3 pos)
        {
            CancelFollowing();
        }
        void CancelFollowing()
        {
            inFollowing = false;
        }
        void OnMouseDrag(GameObject selectObj, TouchPhase tp, Vector3 cameraOffsetPos)
        {
            if (!MovingEnabled) return;
            if (selectObj != null)
            {
                if (selectObj.layer == (int)UConfig.CommonLayer.SelfTroop || selectObj.layer == (int)UConfig.CommonLayer.TempObj) return;
            }
            //deltaX = 0.01f * Screen.dpi * deltaX;
            //deltaY = 0.01f * Screen.dpi * deltaY;
            //if (Mathf.Abs(deltaX) < 0.01f && Mathf.Abs(deltaY) < 0.01f) return;
            //if (translateDirection == Direction.Horizontal) deltaY = 0;
            //else if (translateDirection == Direction.Vertical) deltaX = 0;
            Move(cameraOffsetPos);
        }

        public void Move(float deltaX, float deltaY)
        {
            CancelFollowing();
            if (IsFocusing)
                return;
            float fVar = 0;
            float num6 = dfSetting.GetAdditionHeightBaseInnerCity(CameraDFLevel.INNER_CITY);
            if (dfSetting.dictCameraItem[CameraDFLevel.INNER_CITY].dxf == 0)
                return;
            float num7 = dfSetting.GetAdditionHeightBaseInnerCity(CameraDFLevel.LOWEST);
            if (dfSetting.customMinDxf > 0f && dfSetting.customMinDxf > num6)
            {
                num6 = dfSetting.customMinDxf * (num6 / num7);
                if (dfSetting.customMaxDxf > 0f && num6 > dfSetting.customMaxDxf)
                {
                    num6 = dfSetting.customMaxDxf;
                }
                num7 = dfSetting.customMinDxf;
            }

            fVar = ZOOM_REBOUND_SPEED = (num6 - num7) / 1000.0f;

            float ratio = needHalf ? 0.05f : 1f;
            TargetOffset -= moveRate * transform.right * ratio * deltaX * fVar;
            TargetOffset -= moveRate * Vector3.Cross(transform.right, Vector3.up) * ratio * deltaY * fVar;
            if (needHalf)
                TargetOffset = new Vector3(Mathf.Clamp(TargetOffset.x, -0.5f * planeBorderWidth, 0.5f * planeBorderWidth), TargetOffset.y, Mathf.Clamp(TargetOffset.z, -0.5f * planeBorderHeight, 0.5f * planeBorderHeight));
            else
                TargetOffset = new Vector3(Mathf.Clamp(TargetOffset.x, 0, planeBorderWidth), TargetOffset.y, Mathf.Clamp(TargetOffset.z, 0, planeBorderHeight));
            currentOffset = targetOffset;
            if (OnCameraMoveEvent != null)
            {
                OnCameraMoveEvent();
            }
        }

        public void Move(Vector3 cameraOffsetPos)
        {
            //print("cameraOffsetPos =====> " + "x, " + cameraOffsetPos.x + " y, " + cameraOffsetPos.y + "z, " + cameraOffsetPos.z);
            CancelFollowing();
            if (IsFocusing)
                return;
            transform.position = Vector3.SmoothDamp(transform.position, transform.position - cameraOffsetPos, ref velocity, 0.001f);
            //transform.position -= cameraOffsetPos;
            //transform.DOMove(transform.position - cameraOffsetPos, 0.2f);
            if (OnCameraMoveEvent != null)
            {
                OnCameraMoveEvent();
            }
        }

        void OnDragEnd() 
        {
            if (OnCameraMoveEndEvent != null) 
            {
                OnCameraMoveEndEvent();
            }
        }

        private float[] scaleLevels = { 1, 0.8f, 0.6f };
        void OnZoomCam(int scaleLevel)
        {
        //    Debug.LogError("distance == " + distance);
            CancelFollowing();
            //float fov = CameraMgr.Instance.activeCamera.camera.fieldOfView;
            //Debug.Log("fov =======> "+fov);
            //CameraMgr.Instance.activeCamera.camera.fieldOfView = dragfov * scaleLevels[scaleLevel];

            if (scaleLevel == 2)
            {
                UInput.Instance.OnZoomMaxMinEvent(true);
            }
            else
            {
                UInput.Instance.OnZoomMaxMinEvent(false);
            }

            //float num6 = dfSetting.GetAdditionHeightBaseInnerCity(CameraDFLevel.INNER_CITY);
            //float num7 = dfSetting.GetAdditionHeightBaseInnerCity(CameraDFLevel.LOWEST);
            //if (dfSetting.customMinDxf > 0f && dfSetting.customMinDxf > num6)
            //{
            //    num6 = dfSetting.customMinDxf * (num6 / num7);
            //    if (dfSetting.customMaxDxf > 0f && num6 > dfSetting.customMaxDxf)
            //    {
            //        num6 = dfSetting.customMaxDxf;
            //    }
            //    num7 = dfSetting.customMinDxf;

            //}

            //float num8 = num6 - dfSetting.GetCurFrameDxf();
            ////if (num8 > 0.0f)
            //{
            //    //if (ZOOM_REBOUND_SPEED == 0f)
            //    {
            //        ZOOM_REBOUND_SPEED = (num6 - num7) / 500.0f;
            //    }
            //    float num9 = zoomRate * ZOOM_REBOUND_SPEED * distance * (-1.0f);
            //    float dxf2 = dfSetting.GetCurFrameDxf() + num9;

            //    dfSetting.SetCameraByDxf(dxf2);
            //}
        }

        /// <summary>
        /// ture is max ,false is min
        /// </summary>
        /// <param name=""></param>
        void OnZoomMaxMin(bool ismax)
        {

        }

        private void UpdateCameraPos()
		{
            //if (needDistance)
            //{
            //    if(CameraMgr.Instance.activeCamera!=null)
            //        transform.position = camPosFocusCenter + currentOffset + -1.0f * CameraMgr.Instance.activeCamera.camera.transform.forward * (dfSetting.cameraDistance- camDistanceFocusCenter);
            //}
            //else
            //transform.position = camPosFocusCenter + currentOffset;
        }

        #region shake
        public void SetCameraShakeParam(float distance)
        {
            shakeAmount = distance;
            CalCameraSpeed();
        }

        private void CalCameraSpeed()
        {
          
        
        }

        public void EnableCameraShake(bool bShake)
        {
            canCameraShake = bShake;
        }

       
        public float shakeDistance = 2f;
     
        float radian = 0; 
        public float perRadian = 0.03f; 
      
        private void UpdateCamShake()
        {
            radian += perRadian * shakeAmount * Time.deltaTime;
            if (radian > Mathf.PI * 2)
                radian -= Mathf.PI * 2;
            float dy = Mathf.Cos(radian) * shakeAmount * shakeDistance; //* Time.deltaTime; 
            transform.position = transform.position + new Vector3(dy, 0, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, planeBorderWidth), transform.position.y, Mathf.Clamp(transform.position.z, 0, planeBorderHeight));
        }
        #endregion
    }

