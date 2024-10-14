/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.12.28
 *  描述信息：一些简单的游戏功能扩展，编辑器环境可用。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class GameEditor : MonoBehaviour {

    [MenuItem("我的工具/游戏/删除缓存(PlayerPrefs)")]
    public static void Delete()
    {
        PlayerPrefs.DeleteAll();
    }
    [MenuItem("我的工具/游戏/加钱")]
    public static void AddCoin()
    {

        Debug.LogError("需要自己根据游戏逻辑扩展");
    }
    [MenuItem("我的工具/游戏/加钻石")]
    public static void AddDiamond()
    {
        Debug.LogError("需要自己根据游戏逻辑扩展");
    }
    [MenuItem("我的工具/游戏/加速度")]
    public static void Speed()
    {
        Time.timeScale = Time.timeScale == 5 ? 1 : 5;
        Debug.LogError("切换速度"+Time.timeScale.ToString());
    }
    [MenuItem("我的工具/环境/测试模式")]
    public static void InEditorEnv()
    {
        IsInRealEnv = !IsInRealEnv;
        while (true)
        {
            if (GameObject.Find("Env_Mgr") != null)
            {
                DestroyImmediate(GameObject.Find("Env_Mgr"));
            }
            else
            {
                break;
            }
        }
        GameObject go = new GameObject();
        go.name = "Env_Mgr";
        go.transform.position = Vector3.zero;

        ToolsHelper.AddFileComment(go, MyDefaultPath.PublicEnvPath, ".cs");
        ToolsHelper.AddFileComment(go, MyDefaultPath.EditorEnvPath, ".cs");

        Debug.LogError("切换至测试模式");
    }
    [MenuItem("我的工具/环境/正式模式")]
    public static void InRealEnv()
    {
        IsInRealEnv = !IsInRealEnv;
        while (true)
        {
            if (GameObject.Find("Env_Mgr") != null)
            {
                DestroyImmediate(GameObject.Find("Env_Mgr"));
            }
            else
            {
                break;
            }
        }
        GameObject go = new GameObject();
        go.name = "Env_Mgr";
        go.transform.position = Vector3.zero;
        ToolsHelper.AddFileComment(go, MyDefaultPath.PublicEnvPath, ".cs");
        ToolsHelper.AddFileComment(go, MyDefaultPath.FormalEnvPath, ".cs");
        Debug.LogError("切换至正式模式");
    }
    [MenuItem("我的工具/环境/正式模式", true)]
    public static bool InRealEnvValidate()
    {
        Menu.SetChecked("我的工具/环境/正式模式", IsInRealEnv);
        Menu.SetChecked("我的工具/环境/测试模式", !IsInRealEnv);
        return true;
    }
    [MenuItem("我的工具/快捷键/PlayAndClose _F1")]
    static void PlayAndClose()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = true;
        }
        else if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
    }
    [MenuItem("我的工具/快捷键/PauseAndPlay _F3")]
    static void PauseAndPlay()
    {
        if (!EditorApplication.isPaused)
        {
            EditorApplication.isPaused = true;
        }
        else if (EditorApplication.isPaused)
        {
            EditorApplication.isPaused = false;
        }
    }

    public static bool IsInRealEnv 
    {
        get 
        {
            return EditorPrefs.GetBool("IsInRealEnv", false);
        }
        set {

            EditorPrefs.SetBool("IsInRealEnv", value);
        }

    }
    /// <summary>
    /// 宏改变，有则删除，没有则增加
    /// </summary>
    /// <param name="name"></param>
    public static void ChangeDefineSymbols(string name)
    {
        string define;
        BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            define = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        else
        {
            define = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
            buildTargetGroup = BuildTargetGroup.iOS;
        }
        string[] defineArr = define.Split(';');
        bool isAdd = !defineArr.Contains(name);
        string newDefine = string.Empty;
        if (isAdd)
            newDefine = define + ";" + name;
        else
        {
            newDefine = define.Replace(name, string.Empty);
            newDefine = newDefine.Replace(";;", ";");
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, newDefine);
        Debug.Log($"已经{(isAdd ? "添加" : "移除")}宏{name}");
    }


    [MenuItem("我的工具/其他/重新生成UiPAth信息(移除UI页面时可用)")]
    public static void RefreshUiInfo()
    {
        ExportUI.RefreshUiInfo();
    }
}
