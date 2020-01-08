using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeMgr : MonoBehaviour {


    public static IncomeMgr Instance;
    public Transform node;
    public GameObject item;
    public List<ItemIncome> incomes = new List<ItemIncome>();
    public int usingId;

	void Awake () {

        Instance = this;

	}
	void Start()
    {
        for(int i=0;i<50;i++)
        {
            ItemIncome tex = Instantiate(item.gameObject).GetComponent<ItemIncome>();
            tex.transform.SetParent(node);
            tex.gameObject.SetActive(false);
            tex.transform.localScale = Vector3.one;
            tex.transform.localPosition = Vector3.zero;
            incomes.Add(tex);
        }
    }

    //参考的3D物体，text值
    public void Begin(Transform t,long number)
    {
        usingId++;
        if(usingId == incomes.Count)
        {
            usingId = 0;
        }
        incomes[usingId].Init(t, number);
    }
}
