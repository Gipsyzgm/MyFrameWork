using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceShake : MonoBehaviour {

#if UNITY_IOS
    [DllImport("__Internal")]
    public static extern void setVibratorIOS();
#endif

    public static void setVibrator()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("setVibrator");
#elif UNITY_IOS && !UNITY_EDITOR
        setVibratorIOS();
#endif
    }

    //IOS把DeviceShakeIOS放在Plugins/iOS下
    //Android在AndroidStudio工程UnityPlayerActivity里面加
    //import android.os.Vibrator;
    //public void setVibrator()
    //{
    //    Vibrator vibrator = (Vibrator)getSystemService(VIBRATOR_SERVICE);
    //    ///调用android系统的震动接口，自定义时间为5毫秒
    //    vibrator.vibrate(5);
    //}
}
