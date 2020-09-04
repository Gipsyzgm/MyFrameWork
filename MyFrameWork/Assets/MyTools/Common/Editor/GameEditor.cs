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
        MyEditorTools.OpenEXE("C:/Program Files/Mozilla Firefox/firefox.exe");
        Debug.LogError("删档成功，限PlayerPrefs数据");
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
        Menu.SetChecked("我的工具/环境/测试模式", true);
        Menu.SetChecked("我的工具/环境/正式模式", false);
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
        Menu.SetChecked("我的工具/环境/正式模式", true);
        Menu.SetChecked("我的工具/环境/测试模式", false);
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
    //[MenuItem("我的工具/使用说明",false,0)]
    static void Help()
    {
        Debug.LogError("使用说明1：Common文件夹为通用资源。谨慎操作，其他脚本可能引用");
        Debug.LogError("使用说明2：Zhelp文件内为一些辅助功能脚本");
        Debug.LogError("使用说明3：功能遵循独立原则，通过删除对应功能文件夹以删除其功能");
        Debug.LogError("使用说明4: 由划水爱好者Gipsy整理");    
    }

}
