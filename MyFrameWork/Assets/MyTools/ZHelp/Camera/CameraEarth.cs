using UnityEngine;
using System;
using UnityEngine.UI;
/// <summary>
/// 摄像机简单控制，控制摄像机位置模拟观察目标，围绕目标旋转和缩放，切换观察目标。
/// </summary>

public class CameraEarth : MonoBehaviour
{
    public Transform target;
    float faraway = 10;
    float fingerStartDistance = 0;
    Vector3 touchStartPos;
    Vector2 rotate;
    Vector2 rotateNew;
    float pitchX;

    Vector3 toPos;
    Vector3 toRotate;
    bool isMoving = false;
    bool isWatching = false;
    int finger;

    void Start()
    {
        
    }
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, toPos, Time.deltaTime * 10);
            if (Vector3.Distance(transform.position, toPos) < 0.1f)
            {
                touchStartPos = Input.mousePosition;
                isMoving = false;
            }
        }
        else
        {
            if (isWatching)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(toRotate), Time.deltaTime * 10);
                if (Vector3.Distance(transform.rotation.eulerAngles, toRotate) % 360 < 1)
                {
                    touchStartPos = Input.mousePosition;
                    isWatching = false;
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //====
                    }
                }
#if UNITY_EDITOR
                TouchEditor();
#else
                TouchDevice();
#endif
                pitchX = Mathf.Clamp(rotate.y, -90, 90);
                Vector3 vec = transform.localEulerAngles + new Vector3(-pitchX, rotate.x);
                if (vec.x > 90 && vec.x < 180) vec.x = 90;
                if (vec.x > 180 && vec.x < 270) vec.x = -90;
                transform.localEulerAngles = vec;
            }
        }
    }
    void LateUpdate()
    {
        if (isMoving == false && target != null)
        {
            Vector3 result = target.position - transform.forward * faraway;
            float resultDistance = Vector3.Distance(target.position, result);
            transform.position = target.position - transform.forward * resultDistance;
        }
    }


    void TouchDevice()
    {
        if (Input.touchCount == 1)
        {
            if(finger > 1) //修复两根手指变一根的抖动
            {
                finger = 1;
                touchStartPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                rotateNew = (Input.mousePosition - touchStartPos) * 0.6f * faraway * 0.1f;//距离越近灵敏度越低
                rotate = Vector3.Lerp(rotate, rotateNew, Time.deltaTime * 10);
                touchStartPos = Input.mousePosition;
            }
        }
        else if (Input.touchCount == 2)
        {
            finger = 2;
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);
            if (t1.phase == TouchPhase.Began)
            {
                fingerStartDistance = Vector2.Distance(t1.position, t2.position);
            }
            float updateDistance = Vector2.Distance(t1.position, t2.position) - fingerStartDistance;
            if (updateDistance > 2)
            {
                faraway = Mathf.Lerp(faraway, faraway - 0.8f, 0.15f);
                fingerStartDistance = Vector2.Distance(t1.position, t2.position);
            }
            else if (updateDistance < -2)
            {
                faraway = Mathf.Lerp(faraway, faraway + 0.8f, 0.15f);
                fingerStartDistance = Vector2.Distance(t1.position, t2.position);
            }
            faraway = Mathf.Clamp(faraway, 2, 10);
        }
        else
        {
            rotate = Vector3.Slerp(rotate, Vector3.zero, Time.deltaTime * 10);
        }
    }
    void TouchEditor()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            faraway += Time.deltaTime * 20;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            faraway -= Time.deltaTime * 20;
        }
        faraway = Mathf.Clamp(faraway, 2, 10);

        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            rotateNew = (Input.mousePosition - touchStartPos) * 0.6f * faraway * 0.08f;//距离越近灵敏度越低
            rotate = Vector3.Lerp(rotate, rotateNew, Time.deltaTime * 10);
            touchStartPos = Input.mousePosition;
        }
        else
        {
            rotate = Vector3.Slerp(rotate, Vector3.zero, Time.deltaTime * 10);
        }
    }

    //镜头拉近到一个目标
    public void ToTargetPosition(Transform tmpTarget)
    {
        if (isWatching) return;

        target = tmpTarget;
        isMoving = true;

        Vector3 result = target.position - transform.forward * faraway;
        float resultDistance = Vector3.Distance(target.position, result);
        toPos = target.position - transform.forward * resultDistance;
    }
    //镜头观看目标某个角度,(编辑器里觉得合适的角度传进来)
    public void ToTargetRotate(Vector3 tmpRotate)
    {
        toRotate = tmpRotate;
        isWatching = true;
    }
    public void ToTarget()
    {
        ToTargetRotate(new Vector3(-183,-162,0));
    }
}