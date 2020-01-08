using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *存储游戏玩家数据 
 * 
 */
public class Data : MonoBehaviour {

    public static Data Instance; 

	void Awake () {
        Instance = this;
	}
	
	void Update () {
		
	}
}
