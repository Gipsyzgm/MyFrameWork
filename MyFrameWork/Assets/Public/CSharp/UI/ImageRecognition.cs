using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRecognition : MonoBehaviour {

    public Sprite baseTex;
    public Sprite search;

    void Start()
    {
        Recognition();
    }

    public void Recognition()
    {
        //把中间一行像素拿出来识别
        int xMax = (int)search.rect.xMax;
        int yMax = (int)search.rect.yMax;
        int yCenter = yMax / 2;
        List<Color> color = new List<Color>();
        for (int i=0;i<xMax;i++)
        {
            Color c = search.texture.GetPixel(i, yCenter);
            if(c.a == 0)
            {
                if(color.Count > 0)
                {
                    break;
                }
            }
            else
            {
                color.Add(c);
            }
        }

        xMax = (int)baseTex.rect.xMax;
        yMax = (int)baseTex.rect.yMax;
        print(color.Count);

        for(int y=0;y<yMax;y++)
        {
            for (int x=0; x<xMax; x++)
            {
                int index = 0;
                for (int m = 0; m < color.Count; m++)
                {
                    Color c = color[m];
                    Color cc = baseTex.texture.GetPixel(x + m, y);
                    bool compare = CompareColor(c, cc);
                    if(m == 0)
                        if (compare == false)
                            break;
                    if (compare)
                    {
                        index++;
                        if (index >= color.Count - 5)
                        {
                            print("OK:" + (x) + "_" + y);
                            return;
                        }
                    }
                }
            }
        }
    }

    public bool CompareColor(Color a,Color b)
    {
        if (a.r - b.r > 0.01f || a.r - b.r < -0.01f) return false;
        if (a.g - b.g > 0.01f || a.g - b.g < -0.01f) return false;
        if (a.b - b.b > 0.01f || a.g - b.g < -0.01f) return false;
        if (a.a - b.a > 0.01f || a.a - b.a < -0.01f) return false;

        print("Yes");
        return true;
    }

    //Transform uiRoot = FindObjectOfType<CanvasScaler>().transform;
    //Color[] screenColor = new Color[Screen.height * Screen.width];
}
