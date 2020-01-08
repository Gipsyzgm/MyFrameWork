using UnityEngine;
using System;

 //摄像机简单控制，平移跟随，相机震动
public class FollowShakeCamera : MonoBehaviour
{
    #region
    public void Start()
    {
        FollowInit();
        ShakeInit();
    }
    public void Update()
    {
        FollowUpdate();
        ShakeUpdate();
    }
    public void LateUpdate()
    {
        FollowLateUpdate();
    }
    #endregion

    #region
    public Camera personCamera;
    public Transform target;
    public Vector2 rotate = Vector2.zero;
    public float faraway = 10;
    public float lerpSpeed = 0.2f;
    public float rotateSpeed = 1;
    float pitch;
    float distance;
    public TraceInfo RayTrace = new TraceInfo { Thickness = 0.2f };
    public LimitsInfo PitchLimits = new LimitsInfo { Minimum = -60.0f, Maximum = 60.0f };
    [Serializable]
    public struct LimitsInfo
    {
        public float Minimum;
        public float Maximum;
    }
    [Serializable]
    public struct TraceInfo
    {
        public float Thickness;
        public LayerMask CollisionMask;
    }
    void FollowInit()
    {
        personCamera = GetComponent<Camera>();
        pitch = Mathf.DeltaAngle(0, -transform.localEulerAngles.x);
        distance = faraway;
    }
    void FollowUpdate()
    {
        pitch += rotate.y * rotateSpeed;
        pitch = Mathf.Clamp(pitch, PitchLimits.Minimum, PitchLimits.Maximum);
        transform.localEulerAngles = new Vector3(-pitch, transform.localEulerAngles.y + rotate.x * rotateSpeed, 0);
    }
    void FollowLateUpdate()
    {
        if (target == null) return;

        Vector3 startPos = target.position;
        Vector3 endPos = startPos - transform.forward * faraway;
        Vector3 result = Vector3.zero;
        RaycastHit hit;
        float length = Vector3.Distance(startPos, endPos);
        bool cast = Physics.SphereCast(new Ray(startPos, endPos - startPos), RayTrace.Thickness, out hit, length, RayTrace.CollisionMask.value);

        if (cast) result = hit.point + hit.normal * RayTrace.Thickness;
        else result = endPos;

        float resultDistance = Vector3.Distance(target.position, result);
        if (resultDistance <= distance)
        {
            distance = resultDistance;
            transform.position = result;
        }
        else
        {
            distance = Mathf.Lerp(distance, resultDistance, lerpSpeed);
            transform.position = startPos - transform.forward * distance;
        }
    }
    #endregion

    #region
    //相机震动
    // 震动标志位
    private bool isShakeCamera = false;
    // 震动幅度
    public float shakeLevel = 1f;
    // 震动时间
    public float setShakeTime = 0.1f;
    // 震动的FPS
    public float shakeFps = 45f;
    private float fps;
    private float shakeTime = 0.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 0.005f;

    void ShakeInit()
    {
        shakeTime = setShakeTime;
        fps = shakeFps;
        frameTime = 0.03f;
        shakeDelta = 0.005f;
    }
    void ShakeUpdate()
    {
        if (!isShakeCamera) return;
        if (shakeTime < 0) return;

        shakeTime -= Time.deltaTime;
        if (shakeTime <= 0)
        {
            personCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            isShakeCamera = false;
            shakeTime = setShakeTime;
            fps = shakeFps;
            frameTime = 0.03f;
            shakeDelta = 0.005f;
        }
        else
        {
            frameTime += Time.deltaTime;
            if (frameTime > 1.0 / fps)
            {
                frameTime = 0;
                personCamera.rect = new Rect(shakeDelta * (-1.0f + shakeLevel * UnityEngine.Random.value), shakeDelta * (-1.0f + shakeLevel * UnityEngine.Random.value), 1.0f, 1.0f);
            }
        }
    }
    public void StartShake()
    {
        isShakeCamera = true;
    }
    #endregion
}
