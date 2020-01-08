using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {

    public int effectId;
    public int poolNumber;
    public float time = 4;
    public float timeMax = 4;

    public bool isPlaying;


	void Update () {

        if (isPlaying) return;

        time -= Time.deltaTime;
        if (time > 0) return;

        isPlaying = false;
    }


    public void Play()
    {
        time = timeMax;
        isPlaying = true;
    }

    public void RePlay()
    {
        time = timeMax;
        isPlaying = false;
    }

}
