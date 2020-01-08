using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 因为高已经适配了,如果需要单独调整宽度，就挂这个
 */
public class UIFitterWidth : MonoBehaviour {

    public float fitterWidth = 1;

	void Start () {
        transform.localScale = new Vector3(fitterWidth, transform.localScale.y, transform.localScale.z);
    }
	
}
