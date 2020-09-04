using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    void Start()
    {
        GameObject go = new GameObject();
        go.name = "hahaha";
        go.transform.position = Vector3.zero;
        string ANY = "ShowFPS";
        Type T = Type.GetType(ANY);

        go.AddComponent(T);


    }
 
}
public class newTest
{
    static bool isOk = false;
    public static bool canDo => isOk || Input.GetMouseButton(0);
    // Start is called before the first frame update

}