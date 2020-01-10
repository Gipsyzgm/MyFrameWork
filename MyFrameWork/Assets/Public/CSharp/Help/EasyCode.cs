using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
#if Do_Tween
using DG.Tweening;
#endif
using System;
using UnityEngine.EventSystems;

public class EasyCode : MonoBehaviour {



    /// <summary>
    /// 射线
    /// </summary>
    /// <param name="start"></param>
    /// <param name="director"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static RaycastHit Ray3D(Vector3 start, Vector3 director, float line)
    {
        RaycastHit hit;
        Physics.Raycast(start, director, out hit, line);
        return hit;
    }
    public static RaycastHit2D Ray2D(Vector2 start, Vector2 director, float line)
    {
        return Physics2D.Raycast(start, director, line);
    }
    /// <summary>
    /// t1朝着target
    /// </summary>
    /// <param name="t1"></param>
    /// <param name="target"></param>
    /// <param name="lerp"></param>
    /// <param name="ladder">增量</param>
    public static void LookAtPos(Transform t1,Transform target,float lerp = 0,float ladder = 10)
    {
        Vector3 dir = t1.position - target.position;
        dir.z = 0;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + lerp;
        target.rotation = Quaternion.Slerp(target.rotation, Quaternion.Euler(0, 0, angle), ladder * Time.deltaTime);
    }
    //场景3D坐标转UI坐标(坐标，增量，画布)
    public static Vector3 PosToUiPos(Vector3 pos3D,Vector3 tinkerUp,Canvas uiCanvas)
    {
        Vector3 uiPos = pos3D + tinkerUp;
        uiPos = Camera.main.WorldToScreenPoint(uiPos);
        Vector2 v;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform, uiPos, uiCanvas.worldCamera, out v);
        uiPos = new Vector3(v.x, v.y,0);

        return uiPos;
    }
    //获得一个点为球心半径为radius的随机点
    public Vector3 GetRandomPos(Vector3 circlePoint, float radius)
    {
        Vector3 dir = (new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f))).normalized;
        dir = dir * radius;
        Debug.Log(circlePoint + " __ " + dir);
        return circlePoint + dir;
    }

    public static Vector3 GetVector_CameraMain(Vector3 targetPos)
    {
        if (Camera.main == null) return Vector3.zero;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z);
        Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePos);
        return newPos;
    }
   

    public static int[] GetInts(string str,char c = '_')
    {
        int[] arr = System.Array.ConvertAll<string, int>(str.Split(c), s => int.Parse(s));
        return arr;
    }
    public static long[] GetLong(string str, char c = '_')
    {
        long[] arr = System.Array.ConvertAll<string, long>(str.Split(c), s => long.Parse(s));
        return arr;
    }
    public static float[] GetFloat(string str, char c = '_')
    {
        float[] arr = System.Array.ConvertAll<string, float>(str.Split(c), s => float.Parse(s));
        return arr;
    }
    public static string[] GetString(string str, char c = '_')
    {
        string[] arr = str.Split('_');
        return arr;
    }

   
    /// <summary>
    /// 震动
    /// </summary>
    public static void DeviceShake()
    {
#if UNITY_EDITOR
#else
        //Handheld.Vibrate();
#endif
    }



}

#if Do_Tween
public class TweenClass
{

    public static Tweener TweenAlpha(Image target, float time, float alpha = 0)
    {
        return DOTween.To(() => target.color, r => target.color = r,
               new Color(1, 1, 1, alpha), time).SetEase(Ease.Linear);
    }
    public static Tweener TwenBoard(Transform trans)
    {
        return trans.DOScale(0.8f, 0.05f).OnComplete(() =>
        {
            trans.DOScale(1.1f, 0.075f).OnComplete(() =>
            {
                trans.DOScale(0.95f, 0.05f).OnComplete(() =>
                {
                    trans.DOScale(1f, 0.04f);
                });
            });
        });
    }

    public static Tweener TweenBack(Transform target,float scale,float time)
    {
        return target.DOScale(scale, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            target.DOScale(1f, time).SetEase(Ease.Linear);
        });
    }
    public static Tweener TweenColorBack(Image target,float color,float time)
    {
        return target.DOColor(new Color(color, color, color, 1),time).SetEase(Ease.Linear).OnComplete(()=>
        {
            target.DOColor(new Color(1, 1, 1, 1), time).SetEase(Ease.Linear);
        });
    }
    public static Tweener TweenBedRoom(Transform target)
    {
        return target.DOScale(1.2f, 0.2f).OnComplete(() =>
         {
             target.DOScale(0.8f, 0.1f).OnComplete(()=>
             {
                 target.DOScale(1.1f, 0.1f).OnComplete(()=>
                 {
                     target.DOScale(1, 0.05f);
                 });
             });
         });
    }
    public static Tweener TweenYazi(Transform target)
    {
        return target.DOLocalRotate(new Vector3(0,0, 15), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
           {
               target.DOLocalRotate(new Vector3(0,0, -10), 0.15f).SetEase(Ease.Linear).OnComplete(() =>
               {
                   target.DOLocalRotate(new Vector3(0, 0, 8), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                   {
                       target.DOLocalRotate(new Vector3(0,0, -6), 0.08f).SetEase(Ease.Linear).OnComplete(() =>
                       {
                           target.DOLocalRotate(Vector3.zero, 0.05f).SetEase(Ease.Linear);
                       });
                   });
               });
           });
    }
    public static Tweener TextNum(Text text, long startNum, long toNum, float time = 1.5f, float delay = 0, Action startAction = null, Action endAction = null, string suplus = "")
    {
        return DOTween.To(() => startNum, x => startNum = x, toNum,time).SetEase(Ease.Linear).OnUpdate(()=>
        {
            text.text = EasyCode.GetLongString(startNum).ToString() + suplus;
        }).OnStart(()=>
        {
            if (startAction != null)
                startAction();
        }).OnComplete(()=>
        {
            if (endAction != null)
                endAction();
        });
    }

    public static void CreateGoldIcon(Vector3 startPos, Vector3 endPos, Transform parent, Sprite iconSpr, float scale = 1)
    {
        GameObject o = new GameObject();
        o.transform.SetParent(parent);
        o.transform.position = startPos;
        o.transform.localScale = Vector3.one * scale;
        int x = UnityEngine.Random.Range(-50, 50);
        int y = UnityEngine.Random.Range(-50, 50);
        Vector3 newV = new Vector3(x, y);
        Image ima = o.AddComponent<Image>();
        ima.sprite = iconSpr;
        ima.SetNativeSize();
        o.transform.DOLocalMove(o.transform.localPosition + newV, 0.15f).OnComplete(() =>
        {
            o.transform.DOMove(endPos, 0.3f).OnComplete(() =>
            {
                GameObject.Destroy(o.gameObject);
            });
        });
    }
    public static Tweener ImageFillAmount(Image image,float endValue,float time = 0.5f,Action action = null)
    {
        if (image.type != Image.Type.Filled) return null;
        return DOTween.To(() => image.fillAmount, x => image.fillAmount = x, endValue, time).OnComplete(() =>
        {
            if (action != null)
                action();
        });
    }

}
#endif
