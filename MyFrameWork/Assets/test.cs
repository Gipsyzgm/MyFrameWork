using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    public Dictionary<GameObject, int> _dicGoSurviveTimer;
    public List<GameObject> _goList;
    // Use this for initialization
    void Start () {
        _dicGoSurviveTimer = new Dictionary<GameObject, int>();
        _goList = new List<GameObject>();
        GameObject item = Resources.Load<GameObject>(PathItem.Item);
        for (int i = 0; i < 5; i++)
        {
            GameObject go = Instantiate(item) as GameObject;
            _goList.Add(go);
            _dicGoSurviveTimer.Add(go, i);
        }
        for (int i = 0; i < 5; i++)
        {
            if (_dicGoSurviveTimer.ContainsKey(_goList[i]))
            {
                Debug.LogError("我ra1111111");
            }
        }

      

        foreach (var temp in _dicGoSurviveTimer)
        {
            if (temp.Key == _goList[0])
            {
                Debug.LogError("我ra");
            }
            Debug.LogError(temp.Key+":"+temp.Value);
        }
        foreach (var temp in _goList)
        {
            Debug.LogError("list:"+temp);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
