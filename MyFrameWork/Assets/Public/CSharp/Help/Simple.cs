using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simple : MonoBehaviour {

    //帧动画
    public enum SimpleType
    {
        SpriteChange,
        Null,
    }
    public SimpleType type;
    Image ima;
    SpriteRenderer spriteRender;
    public Sprite[] sprite;
    public float timeSpace = 0.2f;
    float timeSpaceNow;
    int count;
    public bool startChange;

    void Start () {
        timeSpaceNow = timeSpace;
        ima = GetComponent<Image>();
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
        count = 0;
    }
	
	void Update () {
		if(type == SimpleType.SpriteChange)
        {
            if(startChange)
            {
                timeSpaceNow -= Time.deltaTime;
                if (timeSpaceNow < 0)
                {
                    timeSpaceNow = timeSpace;
                    count++;
                    if (count > sprite.Length - 1)
                    {
                        count = 0;
                    }
                    if(ima != null)
                    {
                        ima.sprite = sprite[count];
                        ima.SetNativeSize();
                    }
                    else
                    {
                        spriteRender.sprite = sprite[count];
                    }
                }
            }
        }
	}
}
