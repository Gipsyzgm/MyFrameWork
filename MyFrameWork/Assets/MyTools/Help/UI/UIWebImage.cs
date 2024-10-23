using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIWebImage : MonoBehaviour
{
    private static Dictionary<string, Texture2D> cacheTexture = new Dictionary<string, Texture2D>();
    private static Dictionary<string, List<Action>> requests = new Dictionary<string, List<Action>>();
    public RawImage rawImage;
    public string url;

    private void Awake()
    {
        if (rawImage == null)
        {
            rawImage = GetComponent<RawImage>();
        }
    }

    public void LoadImage(string url, bool resetSize = false)
    {
        this.url = url;
        if (cacheTexture.ContainsKey(url))
        {
            try
            {
                if (rawImage != null && cacheTexture[url] != null)
                {
                    rawImage.texture = cacheTexture[url];
                    if (resetSize)
                    {
                        rawImage.SetNativeSize();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            List<Action> list;
            bool isRequest = false;
            if (!requests.ContainsKey(url))
            {
                list = new List<Action>();
                requests[url] = list;
                isRequest = true;
            }
            else
            {
                list = requests[url];
            }

            list.Add(() => { LoadImage(url); });

            if (isRequest)
            {
                StartCoroutine(DownloadTexture(url));
            }
        }
    }

    private IEnumerator DownloadTexture(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        DownloadHandlerTexture handler = new DownloadHandlerTexture(true);
        uwr.downloadHandler = handler;

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || !string.IsNullOrEmpty(uwr.error))
        {
            Debug.LogError("Download texture error: " + uwr.error);
        }
        else if (uwr.isDone)
        {
            if (handler.texture != null)
            {
                cacheTexture[url] = handler.texture;
            }

            if (requests.ContainsKey(url))
            {
                List<Action> list = requests[url];
                if (handler.texture != null)
                {
                    foreach (Action callback in list)
                    {
                        callback();
                    }
                }

                list.Clear();
            }
        }
    }
}