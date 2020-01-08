using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGoost : MonoBehaviour {

    public int goostNumber = 10;
    public float timeSpeed = 0.01f;
    public float alphaSpeed = 2;
    float tick = 0;
    int count = 0;
    float colorA;
    Color colorbase;
    Color colorchange;
    List<Image> goosts = new List<Image>();

	void Start () {
        Image baseIma = GetComponent<Image>();
        colorbase = baseIma.color;
        for(int i=0;i< goostNumber; i++)
        {
            Image ima = new GameObject("goost" + i).AddComponent<Image>();
            ima.sprite = baseIma.sprite;
            ima.SetNativeSize();
            ima.transform.SetParent(transform.parent);
            ima.transform.localPosition = Vector3.zero;
            goosts.Add(ima);
        }
	}
	
	void Update () {

        for(int i=0;i<goosts.Count;i++)
        {
            colorchange = goosts[i].color;
            colorA = goosts[i].color.a;
            if(colorA > 0)
            {
                colorA -= Time.deltaTime * alphaSpeed;
                if(colorA < 0)
                {
                    colorA = 0;
                }
                colorchange.a = colorA;
                goosts[i].color = colorchange;
            }
        }


        tick += Time.deltaTime;
        if(tick > timeSpeed)
        {
            goosts[count].transform.position = transform.position;
            goosts[count].color = colorbase;
            tick = 0;
            count++;
            if(count == goosts.Count)
            {
                count = 0;
            }
        }
	}
}
