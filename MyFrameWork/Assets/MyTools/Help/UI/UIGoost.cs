using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///  挂载UI上 使UI移动具有重影效果
/// </summary>
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
            Image ima = Instantiate(gameObject).GetComponent<Image>();
            Destroy(ima.GetComponent<UIGoost>());
            ima.gameObject.name = "Goost" + i;
            ima.transform.SetParent(transform.parent);  
            goosts.Add(ima);
        }
       
    }
	
    void Update () {

        for (int i = 0; i < goosts.Count; i++)
        {
            colorchange = goosts[i].color;
            colorA = goosts[i].color.a;     
            if (colorA > 0)
            {
                colorA -= Time.fixedDeltaTime * alphaSpeed;
                if (colorA < 0)
                {
                    colorA = 0;
                }
                colorchange.a = colorA;
                goosts[i].color = colorchange;
            }
        }

        tick += Time.fixedDeltaTime;
        if (tick > timeSpeed)
        {
            goosts[count].transform.position = transform.position;
            goosts[count].color = colorbase;
            tick = 0;
            count++;
            if (count == goosts.Count)
            {
                count = 0;
            }
        }
    }
}
