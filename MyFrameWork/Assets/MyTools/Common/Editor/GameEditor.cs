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

public class GameEditor : MonoBehaviour {

    [MenuItem("我的工具/游戏/删档")]
    public static void Delete()
    {
        PlayerPrefs.DeleteAll();
        MyLog.LogWithColor("删档成功，限PlayerPrefs数据",Color.red);
    }
    [MenuItem("我的工具/游戏/加钱")]
    public static void AddCoin()
    {
        MyLog.LogWithColor("需要自己根据游戏逻辑扩展", Color.red);
    }
    [MenuItem("我的工具/游戏/加钻石")]
    public static void AddDiamond()
    {
        MyLog.LogWithColor("需要自己根据游戏逻辑扩展", Color.red);
    }
    [MenuItem("我的工具/游戏/加速度")]
    public static void Speed()
    {
        Time.timeScale = Time.timeScale == 5 ? 1 : 5;
        MyLog.LogWithColor("切换速度"+Time.timeScale.ToString(), Color.red);
    }
    [MenuItem("我的工具/环境/测试模式")]
    public static void InEditorEnv()
    {
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
        go.AddComponent<Env_Mgr>();
        MyEditorTools.AddFileComment(go, MyDefaultPath.EditorEnvPath, ".cs");

        MyLog.LogWithColor("切换至测试模式", Color.red);
    }
    [MenuItem("我的工具/环境/正式模式")]
    public static void InRealEnv()
    {
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
        go.AddComponent<Env_Mgr>();
        MyEditorTools.AddFileComment(go, MyDefaultPath.FormalEnvPath, ".cs");
        MyLog.LogWithColor("切换至正式模式", Color.red);
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
}
