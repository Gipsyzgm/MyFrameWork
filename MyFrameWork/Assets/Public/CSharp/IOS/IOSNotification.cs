using UnityEngine;
using System;

public class IOSNotification : MonoBehaviour
{
    int alertHour = 19;


    //NotificationMessage("大爷来玩嘛", System.DateTime.Now.AddSeconds(10), false);
    //NotificationMessage("4点推送", 19, true);

    //定点通知
    public static void NotificationMessage(string message, int hour, bool isRepeatDay)
    {
#if UNITY_IOS

        int year = System.DateTime.Now.Year; 
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        System.DateTime newDate = new System.DateTime(year, month, day, hour, 0, 0);


        NotificationMessage(message, newDate, isRepeatDay);
#endif
    }

    //上线的时候取消这个通知
    //比如玩家下线多久可以调这个多久后提醒玩家上线
    public static void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay)
    {
#if UNITY_IOS

        //推送时间需要大于当前时间
        if (newDate > System.DateTime.Now)
        {
            UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
            localNotification.fireDate = newDate;
            localNotification.alertBody = message;
            localNotification.applicationIconBadgeNumber = 1;
            localNotification.timeZone = "GMT+8";
            localNotification.hasAction = true;
            if (isRepeatDay)
            {
                //是否每天定期循环
                localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
                localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
            }
            localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
        }

#endif
    }

    void Awake()
    {
#if UNITY_IOS

        UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge); //| UnityEngine.iOS.NotificationType.Sound);


        //第一次进入游戏的时候清空，有可能用户自己把游戏冲后台杀死，这里强制清空
        CleanNotification();
#endif
    }

    void OnApplicationPause(bool paused)
    {
        //程序进入后台时
        if (paused)
        {
            string tex = "lets play";
            //每天中午12点推送
            NotificationMessage(tex, alertHour, true);
        }
        else
        {
            //程序从后台进入前台时
            CleanNotification();
        }
    }

    //清空所有本地消息
    void CleanNotification()
    {
#if UNITY_IOS

        UnityEngine.iOS.LocalNotification l = new UnityEngine.iOS.LocalNotification();
        l.applicationIconBadgeNumber = -1;
        UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(l);
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();

#endif
    }
}
