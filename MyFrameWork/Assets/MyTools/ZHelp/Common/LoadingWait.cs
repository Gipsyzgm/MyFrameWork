/*
 *  项目名字：MyFrameWork
 *  创建时间：2020.1.16
 *  描述信息：显示百分比和进度条加载场景。
 *  使用：需要一个Text显示百分比，需要Slider显示进度。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingWait : MonoBehaviour {
    //进度条Slider
    public Slider m_Slider;
    //进度条显示百分比Text
    public Text m_Text;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(loadScene());
    }
    IEnumerator loadScene()
    {
        int displayProgress = 0;
        int toProgress = 0;
        //字符串"entergame"为你需要加载的场景名称
        AsyncOperation op = SceneManager.LoadSceneAsync("entergame");
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)op.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }
    public void SetLoadingPercentage(int DisplayProgress)
    {
        m_Slider.value = DisplayProgress * 0.01f;
        m_Text.text = DisplayProgress.ToString() + "%";
    }
}
