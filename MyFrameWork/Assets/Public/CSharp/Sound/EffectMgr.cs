using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMgr : MonoBehaviour {

    public Transform node;
    public Dictionary<int, List<EffectController>> effects = new Dictionary<int, List<EffectController>>();

	void Start () {
		
	}
    void Pool(params int[] effectIds)
    {
        for(int i=0; i<effectIds.Length; i++)
        {
            int id = effectIds[i];
            string path = "";

            GameObject o = Resources.Load<GameObject>(path);
            Pool(o);
        }
    }
    public void Pool(GameObject o)
    {
        GameObject eff = Instantiate(o);
        eff.gameObject.SetActive(false);
        eff.transform.SetParent(node);
        EffectController e = eff.GetComponent<EffectController>();
        int id = e.effectId;
        e.timeMax = o.GetComponent<EffectController>().timeMax;
        e.time = e.timeMax;
        for (int j = 0; j < e.poolNumber; j++)
        {
            if (effects.ContainsKey(id))
            {
                effects[id].Add(e);
            }
            else
            {
                effects.Add(id, new List<EffectController>() { e });
            }
        }
    }

    public void Play(int effectId)
    {
        if (!effects.ContainsKey(effectId)) Pool(effectId);

        for(int i=0;i<effects[effectId].Count;i++)
        {
            if (effects[effectId][i].isPlaying) continue;
            effects[effectId][i].Play();
            return;
        }
        effects[effectId][0].RePlay();
    }

}
