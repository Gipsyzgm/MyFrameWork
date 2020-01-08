using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif
using System.Runtime.InteropServices;

/*
 *GameCenter 
 */

public class IOSGameCenter : MonoBehaviour
{


    static string rankName = "shiprank";


    void Start()
    {
#if UNITY_IOS

        Social.localUser.Authenticate(HandleAuthenticated);
#endif
    }
    void HandleAuthenticated(bool success,string message)
    {
        if (success)
        {
            Debug.Log("初始化成功 用户名:" + Social.localUser.userName + " Id " + Social.localUser.id);
        }
        else
        {
            Debug.Log("初始化失败,错误信息 " + message);
        }
    }

    
    //上传分数
    public static void ScoreReported(int score)
    {
#if UNITY_IOS
        if(Social.localUser.authenticated)
            Social.ReportScore(score, rankName, HandleScoreReported);
#endif
    }
    static void HandleScoreReported(bool success)
    {
        Debug.Log("上传状态: " + success);
    }
    //打开排行榜
    public static void OpenRankBoard()
    {
#if UNITY_IOS
        if (Social.localUser.authenticated)
            Social.ShowLeaderboardUI();
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
    static extern void restoreProductData();
    //请求好评
    [DllImport("__Internal")]
    static extern void RequestNice();

#endif

    public static void BuySomething(string url)
    {
#if UNITY_IOS
        initByUnity();
        requestProductData(url);
#endif
    }
    public static void RestoreBuy()
    {
#if UNITY_IOS
        initByUnity();
        restoreProductData();
#endif
    }




    public static void SendRequestNice()
    {
#if UNITY_IOS
        RequestNice();
#endif
    }


#region

    //void OnGUI()
    //{

    //    GUI.TextArea(new Rect(Screen.width - 200, 0, 200, 100), "GameCenter:" + GameCenterState);
    //    GUI.TextArea(new Rect(Screen.width - 200, 100, 200, 100), "userInfo:" + userInfo);

    //    if (GUI.Button(new Rect(0, 0, 110, 75), "打开成就"))
    //    {

    //        if (Social.localUser.authenticated)
    //        {
    //            Social.ShowAchievementsUI();
    //        }
    //    }

    //    if (GUI.Button(new Rect(0, 150, 110, 75), "打开排行榜"))
    //    {

    //        if (Social.localUser.authenticated)
    //        {
    //            Social.ShowLeaderboardUI();
    //        }
    //    }

    //    if (GUI.Button(new Rect(0, 500, 110, 75), "排行榜设置分数"))
    //    {

    //        if (Social.localUser.authenticated)
    //        {
    //            Social.ReportScore(66, "shiprank", HandleScoreReported);
    //        }
    //    }

    //    if (GUI.Button(new Rect(0, 300, 110, 75), "设置成就"))
    //    {

    //        if (Social.localUser.authenticated)
    //        {
    //            Social.ReportProgress("XXXX", 15, HandleProgressReported);
    //        }
    //    }

    //}

    //设置 成就
    private void HandleProgressReported(bool success)
    {
        Debug.Log("*** HandleProgressReported: success = " + success);
    }

    /// <summary>
    /// 加载好友回调
    /// </summary>
    /// <param name="success">If set to <c>true</c> success.</param>
    private void HandleFriendsLoaded(bool success)
    {
        Debug.Log("*** HandleFriendsLoaded: success = " + success);
        foreach (IUserProfile friend in Social.localUser.friends)
        {
            Debug.Log("* friend = " + friend.ToString());
        }
    }

    /// <summary>
    /// 加载成就回调
    /// </summary>
    /// <param name="achievements">Achievements.</param>
    private void HandleAchievementsLoaded(IAchievement[] achievements)
    {
        Debug.Log("* HandleAchievementsLoaded");
        foreach (IAchievement achievement in achievements)
        {
            Debug.Log("* achievement = " + achievement.ToString());
        }
    }

    /// <summary>
    /// 
    /// 成就回调描述
    /// </summary>
    /// <param name="achievementDescriptions">Achievement descriptions.</param>
    private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
    {
        Debug.Log("*** HandleAchievementDescriptionsLoaded");
        foreach (IAchievementDescription achievementDescription in achievementDescriptions)
        {
            Debug.Log("* achievementDescription = " + achievementDescription.ToString());
        }
    }

#endregion
}





