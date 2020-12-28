/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.12.28
 *  描述信息：作为不销毁物体存在，也可能后续会添加一些其他信息。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_Mgr : MonoBehaviour {

    public static Env_Mgr Instance;
 
    // Use this for initialization
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void Init()
    {
        Debug.LogError("环境组件正常");
    }
}
