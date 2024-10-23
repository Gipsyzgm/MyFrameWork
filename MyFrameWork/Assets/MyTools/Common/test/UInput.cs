//======================================
//	Author: Matrix
//  Create Time：2018/1/9 16:26:50 
//  Function:
//======================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum KeyId
{
    MoveUp = 1,
    MoveDown = 2,
    MoveLeft = 3,
    MoveRight = 4,
    CameraNear = 5,
    CameraFar = 6,
    CameraDefault = 7,
    CameraClose = 8,
    KingLeft = 9,
    KingRight = 10,
    Setting1 = 21,
    Setting2 = 22,
    RankMonth = 31,
    RankWeek = 32,
}

public class UInput : Singleton<UInput>
{
    #region UI element

    private UnityEngine.Camera _UICamera;

    private UnityEngine.Camera UICamera
    {
        get
        {
            if (_UICamera == null)
            {
                GameObject uicam = GameObject.Find("UICamera");
                if (uicam != null)
                {
                    _UICamera = uicam.GetComponent<UnityEngine.Camera>();
                }
            }

            return _UICamera;
        }
    }

    private Canvas _canvas;

    private Canvas canvas
    {
        get
        {
            if (_canvas == null)
            {
                GameObject canvasObj = GameObject.Find("Canvas");
                if (canvasObj != null)
                {
                    _canvas = canvasObj.GetComponent<Canvas>();
                }

                if (_canvas != null)
                {
                    AddCanvas(_canvas);
                }
            }

            return _canvas;
        }
    }

    List<Canvas> mCanvas = new List<Canvas>();

    #endregion

    #region ray cam

    private Camera _rayCam = Camera.main;

    public Camera rayCam
    {
        get { return _rayCam; }
    }

    public void SetRayCam(Camera cam)
    {
        _rayCam = cam;
    }

    public void ResetRayCamToMainCam()
    {
        _rayCam = Camera.main;
    }

    #endregion

    public List<RaycastResult> touchedUIObjs = new List<RaycastResult>();

    //public List<RaycastHit> lastTouchedSceneObjs = new List<RaycastHit>();
    public List<int> lastTouchedSceneObjs = new List<int>();

    //public List<RaycastResult> lastReportedTouchedSceneObjs = new List<RaycastResult>();
    public void AddCanvas(Canvas canvs)
    {
        if (canvs != null && !mCanvas.Contains(canvs))
            mCanvas.Add(canvs);
    }

    public void RemoveCanvas(Canvas canvs)
    {
        if (mCanvas.Contains(canvs))
            mCanvas.Remove(canvs);
    }

    public bool IsPointerOverTargetUIObject(string name)
    {
        bool isOverUI = false;
        for (int i = 0; i < mCanvas.Count; i++)
        {
            isOverUI = IsPointerOverTargetUIObject(mCanvas[i], UnityEngine.Input.mousePosition, name);
            if (isOverUI)
                return true;
        }

        return false;
    }

    public void GetUIRaycastObjects(List<RaycastResult> results)
    {
        for (int i = 0; i < mCanvas.Count; i++)
        {
            GetUIRaycastObjects(mCanvas[i], UnityEngine.Input.mousePosition, results);
        }
    }

    bool IsPointerOverUIObject(List<RaycastResult> results)
    {
        results.Clear();
        for (int i = 0; i < mCanvas.Count; i++)
        {
            //IsPointerOverUIObject(mCanvas[i], UnityEngine.Input.mousePosition, results);
            GetUIRaycastObjects(mCanvas[i], Input.mousePosition, results);
        }

        return results.Count > 0;
    }

    public Action OnClickDownAnyScreen = null;
    public Action OnClickUpAnyScreen = null;

    /// <summary>
    /// 尽量少用
    /// </summary>
    public Action OnMoveAnyScreen = null;

    public Action OnClickUpAnyScreenWithoutObjectAndUI = null;
    public Action OnClickDownAnyScreenWithoutObjectAndUI = null;

    /// <summary>
    /// vector3 点击射线与y=0平面的交点 bool 是否有移动
    /// </summary>
    public Action<Vector3, bool> OnClickUpBlockArea = null;

    public Action<Vector3> OnClickDownBlockArea = null;
    public Action<GameObject> OnClickUIEvent = null;
    public Action<GameObject, TouchPhase, float, float, Vector3> OnHitObjectEvent = null;
    public Action<GameObject> OnClickObjectEvent = null;
    public Action<Vector3, GameObject> OnClickObjEvent = null;
    public Action OnEscape = null;

    /// <summary>
    /// GameObject 点击到的物体，bool 是否有移动
    /// </summary>
    public Action<GameObject, bool> OnClickObjectUpEvent = null;

    public Action<float> OnZoomEvent = null;
    public Action<float> OnZoomScrollEvent = null;
    public Action<GameObject> OnHit2DObjectEvent = null;

    public Action<bool> OnZoomMaxMinEvent = null;


    #region multiTouch

    private bool _enableMultiTouch = true;

    public bool enableMultiTouch
    {
        get { return _enableMultiTouch; }
        set { _enableMultiTouch = value; }
    }

    public Action OnMultiTouch = null;
    bool hasMultiTouch = false;

    #endregion

    /// <summary>
    /// float  delatX, float deltaY
    /// </summary>
    public Action<GameObject, TouchPhase, Vector3> OnMouseFingerMoveEvent = null;

    /// <summary>
    /// GameObject  begin object ,GameObject hover object
    /// </summary>
    public Action<GameObject, GameObject, TouchPhase, float, float> OnMouseFingerSelectThenMoveEvent = null;

    public Action OnDragEnd = null;

    public Func<int> GetLodLevel = null;
    private int scaleLevel = 0;
    bool isPressUI = false;
    bool isFromUI = false;

    private Vector3 curSelectObjHitPoint;

    private GameObject curSelectThenHoverObj;

    #region selectObj  待全改

    private GameObject curSelectObj;
    private GameObject curClickUpObj;

    #endregion

    bool mEnabled = true;
    int enableChangeFrameCount = 0;

    public bool Enabled
    {
        get { return mEnabled; }
        set
        {
            // 限制重复设置 false 导致 update 会起效一次
            if (mEnabled == value) return;

            mEnabled = value;
            enableChangeFrameCount = Time.frameCount;
        }
    }

    private bool _enableTouchObj = false;

    public bool enableTouchObj
    {
        get { return _enableTouchObj; }
        set { _enableTouchObj = value; }
    }

    private bool _enableTouchMove = true;

    public bool enableTouchMove
    {
        get { return _enableTouchMove; }
        set { _enableTouchMove = value; }
    }

    private bool _enableTouchSelectThenMove = true;

    public bool enableTouchSelectThenMove
    {
        get { return _enableTouchSelectThenMove; }
        set { _enableTouchSelectThenMove = value; }
    }

    private bool _enableZoom = true;

    public bool enableZoom
    {
        get { return _enableZoom; }
        set { _enableZoom = value; }
    }

    bool moved = false;

    public bool isInCity = false;

    #region long click

    public Action<TouchPhase, float> OnLongPressWithoutObjectAndUI = null;

    //gameobject clicked object when touch begin
    public Action<GameObject, TouchPhase, float> OnLongPressWithoutUI = null;
    public Action OnLongPressCanceled = null;
    private bool _disableLongClick = false;

    public bool disableLongClick
    {
        get { return _disableLongClick; }
        set { _disableLongClick = value; }
    }

    float lastClickDownTime;
    private bool isDuringLongClick = false;

    #endregion

    #region double click

    float lastClickUpTime;
    public Action<Vector3, GameObject> OnDoubleClickObjectEvent = null;

    #endregion

    #region block area

    private bool _enbableBlock = false;

    public bool enbableBlock
    {
        get { return _enbableBlock; }
        set { _enbableBlock = value; }
    }

    /// <summary>
    /// float vector3 点击射线与y=0平面的交点x值  float 交点z值
    /// </summary>
    public Func<float, float, bool> IsBlockArea = null;

    bool isOnBlockArea = false;

    private void CheckBlockArea(TouchPhase tp, out bool isOnBlockArea)
    {
        isOnBlockArea = false;
        if (!enbableBlock) return;
        if (IsBlockArea == null) return;
        Vector3 zeroInsect = GetZeroIntersect();
        isOnBlockArea = IsBlockArea(zeroInsect.x, zeroInsect.z);
        if (!isOnBlockArea) return;
        if (tp == TouchPhase.Began)
        {
            if (OnClickDownBlockArea != null)
                OnClickDownBlockArea(zeroInsect);
        }
        else if (tp == TouchPhase.Ended)
        {
            if (OnClickUpBlockArea != null)
                OnClickUpBlockArea(zeroInsect, moved);
        }
    }

    #endregion

    #region dpi

    float dpiParam = 0.01f;
    bool dpiParamInited = false;

    float GetDpiParam()
    {
        if (!dpiParamInited)
        {
            dpiParam = 10 / Screen.dpi;
            dpiParamInited = true;
        }

        return dpiParam;
    }

    #endregion

    #region safe area

    private float _safeAreaToSideRatio = 0f;

    public float safeAreaToSideRatio
    {
        get { return _safeAreaToSideRatio; }
        set { _safeAreaToSideRatio = Mathf.Clamp(value, 0, 0.05f); }
    }

    public bool isClickDownInSafeArea = true;

    #endregion

    public Dictionary<KeyCode, KeyId> usedKeyDown = new Dictionary<KeyCode, KeyId>
    {
        { KeyCode.Tab, KeyId.RankMonth },
        { KeyCode.LeftShift, KeyId.RankWeek },

        { KeyCode.Alpha1, KeyId.CameraDefault },
        { KeyCode.Alpha2, KeyId.CameraClose },
        { KeyCode.Alpha3, KeyId.KingLeft },
        { KeyCode.Alpha4, KeyId.KingRight },
        { KeyCode.Alpha5, KeyId.Setting1 },
        { KeyCode.Alpha6, KeyId.Setting2 },
    };

    public Dictionary<KeyCode, KeyId> usedKeyHold = new Dictionary<KeyCode, KeyId>
    {
        { KeyCode.W, KeyId.MoveUp },
        { KeyCode.S, KeyId.MoveDown },
        { KeyCode.A, KeyId.MoveLeft },
        { KeyCode.D, KeyId.MoveRight },
        { KeyCode.Q, KeyId.CameraNear },
        { KeyCode.E, KeyId.CameraFar },
    };

    public Action<int> OnKeyDown = null;
    public Action<int> OnKeyHold = null;

    public void Update()
    {
        if (Time.frameCount > enableChangeFrameCount && !Enabled)
        {
            // Debug.LogError("frame count == " + Time.frameCount + "enable change count == " + enableChangeFrameCount + "enabled == " + Enabled);
            return;
        }
        else
        {
            //     Debug.LogError("multouch return");
        }

        if (UnityEngine.Input.mousePosition.x < 0 || UnityEngine.Input.mousePosition.x > Screen.width
                                                  || UnityEngine.Input.mousePosition.y < 0 ||
                                                  UnityEngine.Input.mousePosition.y > Screen.height)
        {
            //UDebug.Log(()=>"Input.mousePosition "+ Input.mousePosition+ "  Screen.width " + Screen.width+ " Screen.height "+ Screen.height);
            return;
        }

        if (canvas == null)
        {
            Debug.Log("canvas is null");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnEscape != null)
            {
                OnEscape();
            }
        }

        DispatchTouch();

        foreach (KeyCode keyCode in usedKeyDown.Keys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                if (OnKeyDown != null)
                {
                    OnKeyDown((int)usedKeyDown[keyCode]);
                }
            }
        }

        foreach (KeyCode keyCode in usedKeyHold.Keys)
        {
            if (Input.GetKey(keyCode))
            {
                if (OnKeyHold != null)
                {
                    OnKeyHold((int)usedKeyHold[keyCode]);
                }
            }
        }
    }

    float lastDistance;
    float curDistance;
    float deltaDistance = 0;

    void DispatchTouch()
    {
        //模拟多点点击
        if (Input.GetKey(KeyCode.LeftControl))
        {
            var delta = Input.GetAxis("Mouse ScrollWheel");
            if (Math.Abs(delta) > 0.001f && OnZoomScrollEvent != null && enableZoom)
            {
                OnZoomScrollEvent(delta);
            }
        }

        if (Input.touchCount > 1)
        {
            hasMultiTouch = true;

            if (OnMultiTouch != null)
            {
                OnMultiTouch();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                lastDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                curDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                if (Mathf.Abs(lastDistance) > 0.01f)
                {
                    deltaDistance = curDistance - lastDistance;
                    if (OnZoomEvent != null && enableZoom && Mathf.Abs(deltaDistance) > 0.01f)
                    {
                        OnZoomEvent(deltaDistance);
                    }
                }

                lastDistance = curDistance;
            }
            else
            {
                lastDistance = 0;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePos = Input.mousePosition;
                isClickDownInSafeArea = CheckInSafeArea();
                isPressUI = IsPointerOverUIObject(touchedUIObjs);
                isFromUI = isPressUI;
                lastClickDownTime = Time.time;
                if (!isPressUI)
                {
                    CheckBlockArea(TouchPhase.Began, out isOnBlockArea);
                }

                TouchObject(Input.mousePosition, TouchPhase.Began);
                if (OnClickDownAnyScreen != null)
                {
                    OnClickDownAnyScreen();
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (isPressUI)
                {
                    isPressUI = IsPointerOverUIObject(touchedUIObjs);
                }

                TouchObject(Input.mousePosition, TouchPhase.Moved);
                if (OnMoveAnyScreen != null)
                    OnMoveAnyScreen();

                if (!IsInSideSafeArea())
                {
                    FinishTouch();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                FinishTouch();
            }
        }
    }

    bool IsInSideSafeArea()
    {
        float offset = 0;
        return Input.mousePosition.x > offset && Input.mousePosition.x < Screen.width - offset &&
               Input.mousePosition.y > offset && Input.mousePosition.y < Screen.height - offset;
    }

    public bool CheckInSafeArea()
    {
        //float widthOffset = Screen.width * safeAreaToSideRatio;
        float heightOffset = Screen.height * safeAreaToSideRatio;
        return Input.mousePosition.y > heightOffset && Input.mousePosition.y < Screen.height - heightOffset;
    }

    void FinishTouch()
    {
        if (!hasMultiTouch)
        {
            if (moved || !IsInSideSafeArea())
            {
                isPressUI = IsPointerOverUIObject(touchedUIObjs);
            }

            if (isPressUI)
            {
                OnClickUIEvent?.Invoke(touchedUIObjs[0].gameObject);
            }
            else
            {
                CheckBlockArea(TouchPhase.Ended, out isOnBlockArea);
            }

            TouchObject(Input.mousePosition, TouchPhase.Ended);
        }

        hasMultiTouch = false;
        curSelectObj = null;
        curClickUpObj = null;
        curSelectThenHoverObj = null;
        isPressUI = false;
        isFromUI = false;
        isOnBlockArea = false;
        moved = false;
        isDuringLongClick = false;
        touchedUIObjs.Clear();
        isClickDownInSafeArea = true;
        if (OnClickUpAnyScreen != null)
        {
            OnClickUpAnyScreen();
        }

        if (OnDragEnd != null)
        {
            OnDragEnd();
        }
    }

    float deltaX;
    float deltaY;
    float DELTA_LIMITS;
    Vector3 lastMousePos;

    void TouchObject(Vector3 screenPos, TouchPhase tp)
    {
        DELTA_LIMITS = 0.01f;
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            deltaX = (UnityEngine.Input.GetAxis("Mouse X"));
            deltaY = (UnityEngine.Input.GetAxis("Mouse Y"));
            //var offset = Input.mousePosition - lastMousePos;
            //Vector2 delta = new Vector2(offset.x / Screen.width, offset.y / Screen.height);
            //delta = delta.normalized;
            //deltaX = GetDpiParam() * delta.x * 10;
            //deltaY = GetDpiParam() * delta.y * 10;
            //lastMousePos = Input.mousePosition;
            if (UnityEngine.Input.touchCount == 1) //Remote使用
            {
                deltaX = (GetDpiParam() * UnityEngine.Input.touches[0].deltaPosition.x);
                deltaY = (GetDpiParam() * UnityEngine.Input.touches[0].deltaPosition.y);
                DELTA_LIMITS = 0.2f;
            }
        }
        else if (Application.platform == RuntimePlatform.Android ||
                 Application.platform == RuntimePlatform.IPhonePlayer)
        {
            DELTA_LIMITS = 0.0005f * Screen.dpi;

            if (UnityEngine.Input.touchCount == 1)
            {
                deltaX = (GetDpiParam() * UnityEngine.Input.touches[0].deltaPosition.x);
                deltaY = (GetDpiParam() * UnityEngine.Input.touches[0].deltaPosition.y);
            }
        }

        //UDebug.Log(()=>"deltaX: " + deltaX + " deltaY: " + deltaY + " DELTA_LIMITS: " + DELTA_LIMITS);
        if (Mathf.Max(Mathf.Abs(deltaX), Mathf.Abs(deltaY)) > DELTA_LIMITS)
        {
            moved = true;
            lastClickDownTime = float.MaxValue;
            lastClickUpTime = float.MaxValue;
        }

        if (isPressUI) return;
        if (!isOnBlockArea)
        {
            if (!enableTouchObj || curSelectObj == null) // 暂时enableTouchObj为false 全部走下面逻辑
            {
                if (tp != TouchPhase.Moved)
                {
                    bool hasTouchedObj = false;
                    GameObject hitObj = CheckHitObjectMultiLayer(tp, screenPos, out Vector3 point);
                    if (hitObj != null)
                    {
                        if (tp == TouchPhase.Began)
                        {
                            curSelectObj = hitObj;
                            curSelectObjHitPoint = point;
                        }
                        else if (tp == TouchPhase.Ended)
                        {
                            curClickUpObj = hitObj;
                            curSelectObjHitPoint = point;
                        }
                    }

                    if (enableTouchObj)
                    {
                        if (hitObj != null)
                        {
                            hasTouchedObj = true;
                            if (OnHitObjectEvent != null)
                            {
                                OnHitObjectEvent(hitObj, tp, deltaX, deltaY, curSelectObjHitPoint);
                            }
                        }
                    }

                    if (tp == TouchPhase.Began)
                    {
                        if (!hasTouchedObj)
                        {
                            if (OnClickDownAnyScreenWithoutObjectAndUI != null)
                            {
                                OnClickDownAnyScreenWithoutObjectAndUI();
                            }
                        }
                    }
                    else if (tp == TouchPhase.Ended)
                    {
                        CheckLongPress(tp, lastClickDownTime);
                        if (OnClickObjEvent != null && !moved)
                        {
                            GameObject selectObj = null;
                            Ray ray = Camera.main.ScreenPointToRay(screenPos);
                            Vector3 pos = GetHitPosition(ray);
                            RaycastHit[] hits = Physics.RaycastAll(ray, 10000, -1);
                            if (hits.Length > 0)
                            {
                                //int maplayer = LayerMask.NameToLayer("map");
                                int troopslayer = LayerMask.NameToLayer("Troop");
                                //bool hasvalue = false;
                                for (int i = 0; i < hits.Length; i++)
                                {
                                    //if (hits[i].collider.gameObject.layer == maplayer) 
                                    //{

                                    //}
                                    if (hits[i].collider.gameObject.layer == troopslayer)
                                    {
                                        selectObj = hits[i].collider.gameObject;
                                    }
                                }
                            }

                            OnClickObjEvent(pos, selectObj);
                        }

                        if (!hasTouchedObj)
                        {
                            if (OnClickUpAnyScreenWithoutObjectAndUI != null && !moved) //抬起时无拖动
                            {
                                OnClickUpAnyScreenWithoutObjectAndUI();
                            }
                        }
                    }
                }
                else
                {
                    CheckLongPress(tp, lastClickDownTime);
                    curSelectThenHoverObj = CheckHitObjectMultiLayer(tp, screenPos, out Vector3 point);
                }
            }
            else
            {
                if (tp == TouchPhase.Began)
                {
                    GameObject hitObj = CheckHitObjectMultiLayer(tp, screenPos, out Vector3 point);
                    if (hitObj != null)
                    {
                        curSelectObj = hitObj;
                        curSelectObjHitPoint = point;
                    }
                }
                else if (tp == TouchPhase.Moved)
                {
                    CheckLongPress(tp, lastClickDownTime);
                    curSelectThenHoverObj = CheckHitObjectMultiLayer(tp, screenPos, out Vector3 point);
                }
                else if (tp == TouchPhase.Ended)
                {
                    if (enableTouchObj)
                    {
                        CheckLongPress(tp, lastClickDownTime);
                        if (OnClickObjectEvent != null)
                        {
                            if (curSelectObj == CheckHitObjectMultiLayer(tp, screenPos, out Vector3 point))
                            {
                                curSelectObjHitPoint = point;
                                OnClickObjectEvent?.Invoke(curSelectObj);
                            }
                        }

                        if (OnClickObjectUpEvent != null)
                        {
                            OnClickObjectUpEvent(curSelectObj, moved);
                        }

                        if (OnDoubleClickObjectEvent != null)
                        {
                            float deltaTimeBetweenTwoClick = Time.time - lastClickUpTime;
                            if (deltaTimeBetweenTwoClick > 0 && deltaTimeBetweenTwoClick < 0.5f)
                            {
                                OnDoubleClickObjectEvent?.Invoke(screenPos, curSelectObj);
                            }
                            else
                            {
                                lastClickUpTime = Time.time;
                            }
                        }
                    }
                }

                if (enableTouchObj)
                {
                    if (OnHitObjectEvent != null)
                    {
                        OnHitObjectEvent(curSelectObj, tp, deltaX, deltaY, curSelectObjHitPoint);
                    }
                }
            }
        }

        if (_enableTouchMove)
        {
            if (OnMouseFingerMoveEvent != null && !isFromUI && moved)
            {
                Vector3 cameraOffsetPos = CalCameraOffSet(screenPos, lastMousePos);
                lastMousePos = screenPos;
                OnMouseFingerMoveEvent(curSelectObj, tp, cameraOffsetPos);
            }
        }

        if (_enableTouchSelectThenMove)
        {
            if (OnMouseFingerSelectThenMoveEvent != null)
            {
                OnMouseFingerSelectThenMoveEvent(curSelectObj, curSelectThenHoverObj, tp, deltaX, deltaY);
            }
        }
    }

    private Vector3 GetHitPosition(Ray rayhit)
    {
        RaycastHit hit;
        if (Physics.Raycast(rayhit, out hit, 10000, 1 << LayerMask.NameToLayer("Terrain")))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public Vector3 CalCameraOffSet(Vector3 newinput, Vector3 oldinput)
    {
        Ray ray1 = CameraMgr.Instance.activeCamera.camera.ScreenPointToRay(newinput);
        Ray ray2 = CameraMgr.Instance.activeCamera.camera.ScreenPointToRay(oldinput);
        Vector3 pos1 = GetHitPosition(ray1);
        Vector3 pos2 = GetHitPosition(ray2);

        if ((pos1.x == 0 && pos1.y == 0 && pos1.z == 0) || (pos2.x == 0 && pos2.y == 0 && pos2.z == 0))
        {
            return Vector3.zero;
        }
        else
        {
            return pos1 - pos2;
        }
    }

    GameObject CheckHitObject(Vector3 screenPos, out Vector3 point)
    {
        LayerMask buildingLayer = 1 << (int)UConfig.CommonLayer.Building;
        LayerMask dynamicLayer = 1 << (int)UConfig.CommonLayer.DynamicObj;
        LayerMask troopLayer = 1 << (int)UConfig.CommonLayer.Troop;
        LayerMask selfTroopLayer = 1 << (int)UConfig.CommonLayer.SelfTroop;
        LayerMask tempObjLayer = 1 << (int)UConfig.CommonLayer.TempObj;
        LayerMask monsterLayer = 1 << (int)UConfig.CommonLayer.Monster;
        LayerMask clickableLayer =
            buildingLayer | dynamicLayer | troopLayer | selfTroopLayer | tempObjLayer | monsterLayer;

        point = Vector3.zero;
        Ray ray = rayCam.ScreenPointToRay(screenPos);
        RaycastHit hit;
        GameObject hitObj = null;
        if (Physics.Raycast(ray, out hit, 1000.0f, clickableLayer) && hit.collider != null)
        {
            point = hit.point;
            hitObj = hit.collider.gameObject;
        }

        return hitObj;
    }

    GameObject CheckHitObjectMultiLayer(TouchPhase tp, Vector3 screenPos, out Vector3 point)
    {
        bool isClickDown = tp == TouchPhase.Began;
        List<RaycastHit> hitList = CheckHitObjects(screenPos);
        point = Vector3.zero;
        if (hitList != null && hitList.Count > 0)
        {
            int aValue;
            int bValue;
            hitList.Sort((a, b) =>
            {
                if (CameraMgr.Instance.activeCamera.camTranslate.dfSetting.IsInnerCity())
                {
                    aValue = UConfig.innerCityLayerSortDict[a.collider.gameObject.layer];
                    bValue = UConfig.innerCityLayerSortDict[b.collider.gameObject.layer];
                }
                else
                {
                    aValue = UConfig.layerSortDict[a.collider.gameObject.layer];
                    bValue = UConfig.layerSortDict[b.collider.gameObject.layer];
                }

                if (aValue < bValue) return -1;
                else if (aValue > bValue) return 1;
                else return 0;
            });
            if (isClickDown)
            {
                for (int i = 0; i < hitList.Count; i++)
                {
                    int instanceId = hitList[i].collider.gameObject.GetInstanceID();
                    if (lastTouchedSceneObjs.Contains(instanceId))
                    {
                        if (i == hitList.Count - 1)
                        {
                            lastTouchedSceneObjs.Clear();
                            int instanceId0 = hitList[0].collider.gameObject.GetInstanceID();
                            lastTouchedSceneObjs.Add(instanceId0);
                            point = hitList[0].point;
                            return hitList[0].collider.gameObject;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        point = hitList[i].point;
                        lastTouchedSceneObjs.Add(instanceId);
                        return hitList[i].collider.gameObject;
                    }
                }
            }
            else
            {
                point = hitList[0].point;
                return hitList[0].collider.gameObject;
            }
        }

        if (isClickDown)
        {
            lastTouchedSceneObjs.Clear();
        }

        return null;
    }

    Vector3 GetZeroIntersect()
    {
        Ray ray = rayCam.ScreenPointToRay(Input.mousePosition);
        Vector3 zeroIntersect = ray.origin + ray.direction * (ray.origin.y / -ray.direction.y);
        zeroIntersect.y = 0;
        return zeroIntersect;
    }

    List<RaycastHit> CheckHitObjects(Vector3 screenPos)
    {
        LayerMask buildingLayer = 1 << (int)UConfig.CommonLayer.Building;
        LayerMask dynamicLayer = 1 << (int)UConfig.CommonLayer.DynamicObj;
        LayerMask troopLayer = 1 << (int)UConfig.CommonLayer.Troop;
        LayerMask selfTroopLayer = 1 << (int)UConfig.CommonLayer.SelfTroop;
        LayerMask tempObjLayer = 1 << (int)UConfig.CommonLayer.TempObj;
        LayerMask monsterLayer = 1 << (int)UConfig.CommonLayer.Monster;
        LayerMask clickableLayer =
            buildingLayer | dynamicLayer | troopLayer | selfTroopLayer | tempObjLayer | monsterLayer;
        RaycastHit[] hitResults = new RaycastHit[5];
        Ray ray = rayCam.ScreenPointToRay(screenPos);
        int count = Physics.RaycastNonAlloc(ray, hitResults, 1000f, clickableLayer);

        if (count > 0)
        {
            List<RaycastHit> hitResultList = new List<RaycastHit>(count);
            for (int i = 0; i < count; i++)
            {
                if (hitResults[i].collider != null)
                {
                    hitResultList.Add(hitResults[i]);
                }
            }

            //hitResultList.Sort((a, b) =>
            //{
            //    if (a.distance < b.distance) return -1;
            //    else if (a.distance < b.distance) return 1;
            //    else return 0;
            //});
            return hitResultList;
        }

        return null;
    }

    void CheckLongPress(TouchPhase tp, float lastClickDownTime)
    {
        if (_disableLongClick) return;
        float deltaTime = Time.time - lastClickDownTime;

        if (deltaTime > 0)
        {
            isDuringLongClick = true;
            if (OnLongPressWithoutObjectAndUI != null)
            {
                OnLongPressWithoutObjectAndUI(tp, deltaTime);
            }

            if (OnLongPressWithoutUI != null)
            {
                if (tp == TouchPhase.Ended)
                    OnLongPressWithoutUI(curClickUpObj, tp, deltaTime);
                else
                    OnLongPressWithoutUI(curSelectObj, tp, deltaTime);
            }
        }
        else
        {
            if (isDuringLongClick)
            {
                if (OnLongPressCanceled != null)
                {
                    OnLongPressCanceled();
                }
            }
        }
    }

    //-----------------------------------------------------
    bool IsMainCamHitObject(out RaycastHit hit)
    {
        UnityEngine.Ray ray = rayCam.ScreenPointToRay(UnityEngine.Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsUICamHitObject(out RaycastHit hit)
    {
        if (UICamera == null)
        {
            hit = new RaycastHit();
            return false;
        }

        Ray ray = UICamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    bool IsUICamHit2DObject(out RaycastHit2D hit)
    {
        if (UICamera == null)
        {
            hit = new RaycastHit2D();
            return false;
        }

        Ray ray = UICamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsPointerOverTargetUIObject(Canvas canvas, Vector2 screenPosition, string name)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        GetUIRaycastObjects(canvas, screenPosition, results);
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.name.Equals(name))
            {
                return true;
            }
        }

        return false;
    }

    bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition, List<RaycastResult> results)
    {
        //List<RaycastResult> results = new List<RaycastResult>();
        GetUIRaycastObjects(canvas, screenPosition, results);
        return results.Count > 0;
    }

    void GetUIRaycastObjects(Canvas canvas, Vector2 screenPosition, List<RaycastResult> results)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;
        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
    }

    bool IsPointerOverButton(Canvas canvas, Vector2 screenPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;
        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
        LayerMask blockLayer = 1 << (int)UConfig.CommonLayer.Building | 1 << (int)UConfig.CommonLayer.Troop;
        RayCastGraphic.Instance.Raycast(eventDataCurrentPosition, results, canvas, UICamera, uiRaycaster, blockLayer);
        bool flag = false;
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.GetComponent<Button>())
            {
                flag = true;
                break;
            }
        }

        return flag;
    }

    void ShowRay(PointerEventData eventData)
    {
        Ray ray = UICamera.ScreenPointToRay(eventData.position);
    }
}