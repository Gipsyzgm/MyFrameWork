using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *iphoneX适配使用方法:挂到场景
 *                   :是Stretch可以把SettingIphoneXStretch挂到撑开的节点上
 *                   :是Center可以把SettingIphoneXCenter挂到顶部的节点上,color可控制需要的颜色
 * 
 *     CGRect winSize = [UIScreen mainScreen].bounds;
    if (winSize.size.height / winSize.size.width > 2) {
        winSize.size.height -= 30;
        winSize.origin.y = 30;
    }
    _window = [[UIWindow alloc] initWithFrame: winSize];
    这种有个缺陷，就是跟原生的UI冲突，比如广告，有可能把按钮一部分截掉
 * 
 */

public class SettingIphoneX : MonoBehaviour {

    //IphoneX刚好偏移50
    static int cutSize = 50;
    static Image colorIma = null;
    public static bool isIphoneX;

    void Awake()
    {
#if UNITY_IOS
        if (Screen.height / (Screen.width + 0.0f) > 2)
        {
            IphoneXInit();
        }
#endif
    }
    void IphoneXInit () {
        GameObject iphonexAnchor = new GameObject("IphonexAnchor");
        CanvasScaler scaler = FindObjectOfType<CanvasScaler>();
        iphonexAnchor.transform.SetParent(scaler.transform);
        iphonexAnchor.transform.SetAsFirstSibling();
        iphonexAnchor.gameObject.AddComponent<RectTransform>();
        iphonexAnchor.transform.localScale = Vector3.one;
        iphonexAnchor.transform.localScale = Vector3.one;
        //UI放在摄像机下拉伸相机用这个
        iphonexAnchor.transform.localPosition = new Vector3(0, scaler.referenceResolution.y / 2.0f, 0);
        //普通的用这个
        //iphonexAnchor.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        //iphonexAnchor.transform.localPosition = Vector3.zero;
        //iphonexAnchor.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        GameObject o = new GameObject();
        colorIma = o.AddComponent<Image>();
        o.transform.SetParent(iphonexAnchor.transform);
        o.transform.localScale = Vector3.one;
        o.transform.localPosition = Vector3.zero;
        colorIma.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, cutSize * 2);
        o.name = "ColorBg";

        isIphoneX = true;
    }

    //设置颜色,如关闭界面可以调这个把上边的颜色还原回去
    public static void SetColor(Color color)
    {
        if (!isIphoneX) return;

        colorIma.color = color;
    }

    //平铺用这个,相当于把上面的UI往下移了一点
    public static void SetValueCenter(Transform trans)
    {
        if (!isIphoneX) return;

        Vector3 v = trans.localPosition;
        trans.localPosition = new Vector3(v.x, v.y - cutSize, v.z);
    }
    //如果一个UI的主节点是stretch撑开的,用这句，把节点压缩了一下
    public static void SetValueStretch(Transform trans)
    {
        if (!isIphoneX) return;

        trans.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -cutSize / 2.0f);
        trans.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -cutSize);
    }
}
