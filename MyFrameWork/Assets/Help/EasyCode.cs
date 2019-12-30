using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class EasyCode : MonoBehaviour {

    /// <summary>
    /// 延迟执行
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public static IEnumerator DelayInvoke(System.Action action,float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

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
    /// <summary>
    /// 在范围内选出n个不同对象
    /// </summary>
    /// <param name="list"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static List<int> RandomCount(List<int> list, int n)
    {
        if (list.Count <= n) return null;

        List<int> newList = new List<int>();

        int m = UnityEngine.Random.Range(0, list.Count);
        newList.Add(list[m]);



        while (newList.Count < n)
        {
            int a = UnityEngine.Random.Range(0, list.Count);
            int num = list[a];
            if (!newList.Contains(num))
                newList.Add(num);
        }
        return newList;
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

    public static string GetLongString(long money)
    {
        if(money < Mathf.Pow(10,3))
        {
            return money.ToString();
        }
        else if(money >= Mathf.Pow(10, 3) && money < Mathf.Pow(10, 6))
        {
            return (money / (Mathf.Pow(10, 3) + 0.0f)).ToString("f1") + "K";
        }
        else if (money >= Mathf.Pow(10, 6) && money < Mathf.Pow(10, 9))
        {
            return (money / (Mathf.Pow(10, 6) + 0.0f)).ToString("f1") + "M";
        }
        else if (money >= Mathf.Pow(10, 9) && money < Mathf.Pow(10, 12))
        {
            return (money / (Mathf.Pow(10, 9) + 0.0f)).ToString("f1") + "B";
        }

        string B = (money / (Mathf.Pow(10, 9) + 0.0f)).ToString("f1") + "B";

        return B;
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

    //color转hexColor
    public static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }
    /// <summary>
    /// hex转换到color
    /// </summary>
    /// <param name="hex">ABCE56AE</param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }
    //十六进制转color (#FF4357)
    public static Color HtmlStringRGB(string htmlString)
    {
        Color color;
        ColorUtility.TryParseHtmlString(htmlString,out color);
        return color;
    }

    ///点到UI上
    public static bool IsPointerOverGameObject()
    {
        PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, list);
        return list.Count > 0;
    }
}


public class TweenClass
{

    //public static Tweener TweenAlpha(Image target, float time, float alpha = 0)
    //{
    //    return DOTween.To(() => target.color, r => target.color = r,
    //           new Color(1, 1, 1, alpha), time).SetEase(Ease.Linear);
    //}
    //public static Tweener TwenBoard(Transform trans)
    //{
    //    return trans.DOScale(0.8f, 0.05f).OnComplete(() =>
    //    {
    //        trans.DOScale(1.1f, 0.075f).OnComplete(() =>
    //        {
    //            trans.DOScale(0.95f, 0.05f).OnComplete(() =>
    //            {
    //                trans.DOScale(1f, 0.04f);
    //            });
    //        });
    //    });
    //}

    //public static Tweener TweenBack(Transform target,float scale,float time)
    //{
    //    return target.DOScale(scale, time).SetEase(Ease.Linear).OnComplete(() =>
    //    {
    //        target.DOScale(1f, time).SetEase(Ease.Linear);
    //    });
    //}
    //public static Tweener TweenColorBack(Image target,float color,float time)
    //{
    //    return target.DOColor(new Color(color, color, color, 1),time).SetEase(Ease.Linear).OnComplete(()=>
    //    {
    //        target.DOColor(new Color(1, 1, 1, 1), time).SetEase(Ease.Linear);
    //    });
    //}
    //public static Tweener TweenBedRoom(Transform target)
    //{
    //    return target.DOScale(1.2f, 0.2f).OnComplete(() =>
    //     {
    //         target.DOScale(0.8f, 0.1f).OnComplete(()=>
    //         {
    //             target.DOScale(1.1f, 0.1f).OnComplete(()=>
    //             {
    //                 target.DOScale(1, 0.05f);
    //             });
    //         });
    //     });
    //}
    //public static Tweener TweenYazi(Transform target)
    //{
    //    return target.DOLocalRotate(new Vector3(0,0, 15), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
    //       {
    //           target.DOLocalRotate(new Vector3(0,0, -10), 0.15f).SetEase(Ease.Linear).OnComplete(() =>
    //           {
    //               target.DOLocalRotate(new Vector3(0, 0, 8), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
    //               {
    //                   target.DOLocalRotate(new Vector3(0,0, -6), 0.08f).SetEase(Ease.Linear).OnComplete(() =>
    //                   {
    //                       target.DOLocalRotate(Vector3.zero, 0.05f).SetEase(Ease.Linear);
    //                   });
    //               });
    //           });
    //       });
    //}
    //public static Tweener TextNum(Text text, long startNum, long toNum, float time = 1.5f, float delay = 0, Action startAction = null, Action endAction = null, string suplus = "")
    //{
    //    return DOTween.To(() => startNum, x => startNum = x, toNum,time).SetEase(Ease.Linear).OnUpdate(()=>
    //    {
    //        text.text = EasyCode.GetLongString(startNum).ToString() + suplus;
    //    }).OnStart(()=>
    //    {
    //        if (startAction != null)
    //            startAction();
    //    }).OnComplete(()=>
    //    {
    //        if (endAction != null)
    //            endAction();
    //    });
    //}

    //public static void CreateGoldIcon(Vector3 startPos, Vector3 endPos, Transform parent, Sprite iconSpr, float scale = 1)
    //{
    //    GameObject o = new GameObject();
    //    o.transform.SetParent(parent);
    //    o.transform.position = startPos;
    //    o.transform.localScale = Vector3.one * scale;
    //    int x = UnityEngine.Random.Range(-50, 50);
    //    int y = UnityEngine.Random.Range(-50, 50);
    //    Vector3 newV = new Vector3(x, y);
    //    Image ima = o.AddComponent<Image>();
    //    ima.sprite = iconSpr;
    //    ima.SetNativeSize();
    //    o.transform.DOLocalMove(o.transform.localPosition + newV, 0.15f).OnComplete(() =>
    //    {
    //        o.transform.DOMove(endPos, 0.3f).OnComplete(() =>
    //        {
    //            GameObject.Destroy(o.gameObject);
    //        });
    //    });
    //}
    //public static Tweener ImageFillAmount(Image image,float endValue,float time = 0.5f,Action action = null)
    //{
    //    if (image.type != Image.Type.Filled) return null;
    //    return DOTween.To(() => image.fillAmount, x => image.fillAmount = x, endValue, time).OnComplete(() =>
    //    {
    //        if (action != null)
    //            action();
    //    });
    //}

}
