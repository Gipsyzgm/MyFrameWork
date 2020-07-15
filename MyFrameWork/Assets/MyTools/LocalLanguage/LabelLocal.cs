using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelLocal : MonoBehaviour {

    //挂载在需要转换的文字组件上，填写对应ID。
    public int languageId;

    void OnEnable()
    {
        Text t = GetComponent<Text>();

        if (t != null)
        {
            t.text = LanguageMgr.GetById(languageId);
        }
    }

}
