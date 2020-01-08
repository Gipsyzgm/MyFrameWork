using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if Do_Tween
using DG.Tweening;
#endif
using UnityEngine.SceneManagement;


public class Lauch : MonoBehaviour {

    Image load;
	void Start () {
        AsyncOperation scene = SceneManager.LoadSceneAsync("UI");
        scene.allowSceneActivation = false;

        load = GetComponent<Image>();
        Color c = load.color;
#if Do_Tween
        DOTween.To(() => c.a, x => c.a = x, 0, 1).OnUpdate(()=>
        {
            load.color = new Color(c.r, c.g, c.b, c.a);
        }).OnComplete(()=>
        {
            scene.allowSceneActivation = true;
        });
#endif
    }
	
	// Update is called once per frame
	void Update () {
      
	}
}
