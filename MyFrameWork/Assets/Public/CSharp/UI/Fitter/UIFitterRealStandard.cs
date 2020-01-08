using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 想要保持原样不缩放,就挂在不想缩放的物体上
 */
public class UIFitterRealStandard : MonoBehaviour {

	void Start () {
        transform.localScale = new Vector3(transform.localScale.x, 1 / UIFitter.realStandard, transform.localScale.z);
    }	
}
