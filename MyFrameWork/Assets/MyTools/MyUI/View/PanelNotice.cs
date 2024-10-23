using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class PanelNotice : PanelBase
{
    public Button btnClose;
    public Transform goNoticeList;
    public Text txtVersion;
    public Text txtDate;
    public Text txtContent;

    public override void Init(params object[] _args)
    {
        args = _args;
        CurViewPath = "MyUI/View/PanelNotice";
        layer = PanelLayer.Tips;
    }

    public override void InitComponent()
    {
        btnClose = curView.transform.Find("Bg/btnClose_Button").GetComponent<Button>();
        btnClose.onClick.AddListener(btnCloseOnClick);
        goNoticeList = curView.transform.Find("ScrollView/Viewport/goNoticeList_Transform").transform;
        txtVersion = curView.transform
            .Find("ScrollView/Viewport/goNoticeList_Transform/NoticeItem/GameObject/Version/txtVersion_Text")
            .GetComponent<Text>();
        txtDate = curView.transform
            .Find("ScrollView/Viewport/goNoticeList_Transform/NoticeItem/GameObject/Date/txtDate_Text")
            .GetComponent<Text>();
        txtContent = curView.transform.Find("ScrollView/Viewport/goNoticeList_Transform/NoticeItem/txtContent_Text")
            .GetComponent<Text>();
        CustomComponent();
    }
    //——————————上面部分自动生成，每次生成都会替换掉，不要手写东西——————————

    //——————————以下为手写部分，初始化补充方法为CustomComponent()———————————
    //@EndMark@
    public void btnCloseOnClick()
    {
        AudioMgr.Instance.PlayEffect(MyAudioName.gs_1);
        Close();
    }

    public void CustomComponent()
    {
    }

    public override void OnShow()
    {
        base.OnShow();
        GameObject item = Addressables.LoadAssetAsync<GameObject>(PathItem.NoticeItem).WaitForCompletion();
        if (item == null)
            Debug.LogError("panelMgr.OpenPanelfail,skin is null,skinPath= " + PathItem.NoticeItem);
        List<UpdateNotice> noticelist = UpdateNotice.GetList();
        noticelist.Sort((a, b) => b.Sort.CompareTo(a.Sort));
        for (int i = 0; i < noticelist.Count; i++)
        {
            GameObject gameObject = Instantiate(item, goNoticeList);
            NoticeItem Myitem = gameObject.AddComponent<NoticeItem>();
            Myitem.index = i;
            Myitem.InitComponent();
            Myitem.SetInfo(noticelist[i]);
        }
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}