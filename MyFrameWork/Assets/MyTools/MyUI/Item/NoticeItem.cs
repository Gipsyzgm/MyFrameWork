﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Events;

public class NoticeItem : MonoBehaviour
{
    public Text txtVersion;
    public Text txtDate;
    public Text txtContent;
    public int index = 0;
    public object[] args;

    public void InitComponent(params object[] _args)
    {
        args = _args;
        txtVersion = transform.Find("GameObject/Version/txtVersion_Text").GetComponent<Text>();
        txtDate = transform.Find("GameObject/Date/txtDate_Text").GetComponent<Text>();
        txtContent = transform.Find("txtContent_Text").GetComponent<Text>();
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————

    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void CustomComponent()
    {
    }

    public void SetInfo(UpdateNotice updateNotice)
    {
        txtVersion.text = updateNotice.Version;
        txtDate.text = TimeHelper.TimeFormatToString(updateNotice.Date);
        string content = string.Format("<size=40>【{0}】</size>\n{1}", updateNotice.Name, updateNotice.Content);
        txtContent.text = content.Replace("\\n", "\n");
    }


    public void UpdateItem()
    {
    }
}