using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatMgr : MonoBehaviour {

    public Material[] mat;

    public static MatMgr Instance;

	void Awake () {
        Instance = this;
	}
}
