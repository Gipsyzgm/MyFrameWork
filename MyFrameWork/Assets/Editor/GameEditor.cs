using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        //PlayerPrefs.SetString(System.Enum.GetNames(typeof(MoneyType))[1], long.MaxValue.ToString());
    }
    [MenuItem("我的工具/游戏/加钻石")]
    public static void AddDiamond()
    {
        //PlayerPrefs.SetString(System.Enum.GetNames(typeof(MoneyType))[2], long.MaxValue.ToString());
    }
    [MenuItem("我的工具/游戏/加速度")]
    public static void Speed()
    {
        Time.timeScale = Time.timeScale == 5 ? 1 : 5;
        MyLog.LogWithColor("切换速度", Color.red);
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
        go.AddComponent<EditorMgr>();
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
        go.AddComponent<EditorMgr>();
        MyEditorTools.AddFileComment(go, MyDefaultPath.FormalEnvPath, ".cs");
        MyLog.LogWithColor("切换至正式模式", Color.red);
    }



}
