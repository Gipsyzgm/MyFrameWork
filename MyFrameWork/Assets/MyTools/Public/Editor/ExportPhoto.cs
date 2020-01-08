using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * 截图在桌面UnityCaptrue文件夹
 * 感觉没什么用，作为参考代码。
 */
public class ExportPhoto : MonoBehaviour {

    static string dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + "\\UnityCaptrue";

    void Start()
    {
        //单个截透明图
        //StartCoroutine(PngStart());
    }

    //截游戏图
    [UnityEditor.MenuItem("我的工具/截图")]
    public static void Capture()
    {
        string file = GetFileName();
        ScreenCapture.CaptureScreenshot(file);
    }
    //截透明图,可以把模型、场景、特效啥的截成Icon,这个模式相机镜头调成Solid Color
    IEnumerator PngStart()
    {
        FindObjectOfType<Camera>().clearFlags = CameraClearFlags.SolidColor;
        string file = GetFileName();
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        yield return new WaitForEndOfFrame();
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot = GetCutTexture(screenShot);
        screenShot.Apply();
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(file, bytes);
    }

    #region
    //文件路径
    static string GetFileName()
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        Debug.LogError("截图完成："+dir);
        int name = 0;
        string file = dir + "/" + name + ".png";
        while (File.Exists(file))
        {
            name++;
            file = dir + "/" + name + ".png";
        }
        return file;
    }
    // 裁剪边缘多余的图块
    static Texture2D GetCutTexture(Texture2D tex)
    {
        int left = 0;
        int right = 0;
        int bottom = 0;
        int top = 0;

        bool complete = false;
        for (int i = 0; i < Screen.width; i++)
        {
            if (complete) break;
            for (int j = 0; j < Screen.height; j++)
            {
                Color c = tex.GetPixel(i, j);
                if (c.a != 0)
                {
                    left = i;
                    complete = true;
                    break;
                }
            }
        }
        complete = false;
        for (int i = Screen.width; i >= 0; i--)
        {
            if (complete) break;
            for (int j = 0; j < Screen.height; j++)
            {
                Color c = tex.GetPixel(i, j);
                if (c.a != 0)
                {
                    right = i;
                    complete = true;
                    break;
                }
            }
        }
        complete = false;
        for (int i = 0; i < Screen.height; i++)
        {
            if (complete) break;
            for (int j = 0; j < Screen.width; j++)
            {
                Color c = tex.GetPixel(j, i);
                if (c.a != 0)
                {
                    bottom = i;
                    complete = true;
                    break;
                }
            }
        }
        complete = false;
        for (int i = Screen.height; i >= 0; i--)
        {
            if (complete) break;
            for (int j = 0; j < Screen.width; j++)
            {
                Color c = tex.GetPixel(j, i);
                if (c.a != 0)
                {
                    top = i;
                    complete = true;
                    break;
                }
            }
        }

        int texWidth = right - left;
        int texHeight = top - bottom;

        Texture2D newTex = new Texture2D(texWidth, texHeight);

        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                bool x = i >= left && i <= right;
                bool y = j >= bottom && j <= top;

                if (x && y)
                {
                    int m = i - left;
                    int n = j - bottom;
                    newTex.SetPixel(m, n, tex.GetPixel(i, j));
                }

            }
        }

        return newTex;
    }
    #endregion

}
