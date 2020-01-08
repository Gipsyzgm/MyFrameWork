using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;


/*
 * 使用方法:
 * 配合PaymentViewController
 * 进入游戏时调用IAP.Initialize();
 */
public class IAP : MonoBehaviour {

    public static Action BuySuccessAction;
    public static Action BuyFailedAction;
    public static Action BuyCancleAction;
    public static Action BuyAlreadyAction;

    public static Action RestoreSuccessAction;
    public static Action RestoreFailedAction;


    public static void Initialize()
    {
        IAP iap = new GameObject("IAP").AddComponent<IAP>();
        DontDestroyOnLoad(iap.gameObject);
#if UNITY_IOS && !UNITY_EDITOR
        initByUnity();
#endif
    }

#if UNITY_IOS

    //初始化购买对象
    [DllImport("__Internal")]
    static extern void initByUnity();
    //请求购买商品
    [DllImport("__Internal")]
    static extern void requestProductData(string url);
    //恢复购买
    [DllImport("__Internal")]
    static extern void restoreByUnity();

#endif

    //购买 url:shopping.250.dimond.0.99
    public static void BuySomething(string url,Action _Success,Action _Failed,Action _Cancle,Action _Already)
    {
        BuySuccessAction = _Success;
        BuyFailedAction = _Failed;
        BuyCancleAction = _Cancle;
        BuyAlreadyAction = _Already;

#if UNITY_IOS
        requestProductData(url);
#endif
    }

    //购买成功
    public void BuySuccess()
    {
        if (BuySuccessAction != null)
            BuySuccessAction();
    }
    //购买失败
    public void BuyFailed()
    {
        if (BuyFailedAction != null)
            BuyFailedAction();
    }
    //取消交易
    public void BuyCancle()
    {
        if (BuyCancleAction != null)
            BuyCancleAction();
    }

    //恢复购买
    public static void RestoreBuy(Action _success, Action _failed)
    {
        RestoreSuccessAction = _success;
        RestoreFailedAction = _failed;
#if UNITY_IOS
        restoreByUnity();
#endif
    }




    //恢复订阅
    public static void RestoreBuy()
    {
#if UNITY_IOS
        restoreByUnity();
#endif
    }
    //恢复成功
    public void RestoreSuccess()
    {
        if (RestoreSuccessAction != null)
            RestoreSuccessAction();
    }
    //恢复失败
    public void RestoreFailed()
    {
        if (RestoreFailedAction != null)
            RestoreFailedAction();
    }
    //购买过此商品
    public void BuyAlready()
    {
        if (BuyAlreadyAction != null)
            BuyAlreadyAction();
    }
}
