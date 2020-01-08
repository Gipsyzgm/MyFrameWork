using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIncome : MonoBehaviour {

    //text
    public Text textNumber;
    //Transform
    Transform tran;
    //持续时间
    float times;
    //手动修正的值
    Vector3 tinker = new Vector3(0, 0, 0);

    void Update()
    {
        //初始的地方
        Vector3 ui = EasyCode.PosToUiPos(tran.position,tinker,ComponentMgr.Instance.uiCanvas);
        //上浮
        transform.localPosition = ui + Vector3.up * times;
        times += 1;
        if (times > 50)
        {
            gameObject.SetActive(false);
        }
    }

    public void Init(Transform t,long number)
    {
        tran = t;
        textNumber.text = EasyCode.GetLongString(number);
        transform.localPosition = EasyCode.PosToUiPos(tran.position, tinker, ComponentMgr.Instance.uiCanvas);
        times = 0;
        gameObject.SetActive(true);
    }
}
