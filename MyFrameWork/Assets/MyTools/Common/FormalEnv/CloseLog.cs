using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseLog : MonoBehaviour {
	// Use this for initialization
	void Start () {
        Debug.unityLogger.logEnabled = false;
	}
}
