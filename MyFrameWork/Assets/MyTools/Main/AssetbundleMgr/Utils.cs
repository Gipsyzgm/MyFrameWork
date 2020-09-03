using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Utils
{
    public static void ResetShader(UnityEngine.Object obj)
    {
#if UNITY_EDITOR     
        List<Material> listMat = new List<Material>();
        if (obj == null)
        {
            Renderer[] rends = Transform.FindObjectsOfType<Renderer>();
            if (null != rends)
            {
                foreach (Renderer item in rends)
                {
                    Material[] materialsArr = item.sharedMaterials;
                    foreach (Material m in materialsArr)
                        listMat.Add(m);
                }
            }
        }
        else if (obj is Material)
        {
            Material m = obj as Material;
            listMat.Add(m);
        }
        else if (obj is GameObject)
        {
            GameObject go = obj as GameObject;
            Renderer[] rends = go.GetComponentsInChildren<Renderer>();
            if (null != rends)
            {
                foreach (Renderer item in rends)
                {
                    Material[] materialsArr = item.sharedMaterials;
                    foreach (Material m in materialsArr)
                        listMat.Add(m);
                }
            }
        }
        for (int i = 0; i < listMat.Count; i++)
        {
            Material m = listMat[i];
            if (null == m)
                continue;
            var shaderName = m.shader.name;
            var newShader = Shader.Find(shaderName);
            if (newShader != null)
                m.shader = newShader;
        }
#endif
    }


}
