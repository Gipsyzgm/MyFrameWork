using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMgr : MonoBehaviour {

    public static EditorMgr Instance;
 
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
    }
}
