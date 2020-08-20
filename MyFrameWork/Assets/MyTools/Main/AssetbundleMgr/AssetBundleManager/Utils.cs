using AssetBundles;
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
        if (AssetBundleManager.SimulateAssetBundleInEditor)
            return;
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


//    public static void ResetShader2(GameObject obj)
//    {
//#if UNITY_EDITOR
//        SkinnedMeshRenderer[] skinnedMeshRenderList = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
//        for (int i = 0; i < skinnedMeshRenderList.Length; i++)
//        {
//            Material[] mats = skinnedMeshRenderList[i].sharedMaterials;
//            for (int j = 0; j < mats.Length; j++)
//            {
//                //Debuger.Log(skinnedMeshRenderList[i].materials[j].shader.name);
//                mats[j].shader = Shader.Find(mats[j].shader.name);

//            }
//        }
//        MeshRenderer[] MeshRenderList = obj.GetComponentsInChildren<MeshRenderer>(true);
//        for (int i = 0; i < MeshRenderList.Length; i++)
//        {
//            Material[] mats = MeshRenderList[i].sharedMaterials;
//            for (int j = 0; j < mats.Length; j++)
//            {
//                if (mats[j] != null)
//                {
//                    mats[j].shader = Shader.Find(mats[j].shader.name);
//                }
//            }
//        }
//        TrailRenderer[] trailMeshRenderList = obj.GetComponentsInChildren<TrailRenderer>(true);
//        for (int i = 0; i < trailMeshRenderList.Length; i++)
//        {
//            Material[] mats = trailMeshRenderList[i].sharedMaterials;
//            for (int j = 0; j < mats.Length; j++)
//            {
//                mats[j].shader = Shader.Find(mats[j].shader.name);
//            }
//        }
//        //ParticleSystem[] ParticleList = obj.GetComponentsInChildren<ParticleSystem>(true);
//        //for (int i = 0; i < ParticleList.Length; i++)
//        //{
//        //    Material[] mats = ParticleList[i].renderer.sharedMaterials;
//        //    for (int j = 0; j < mats.Length; j++)
//        //    {
//        //        mats[j].shader = Shader.Find(mats[j].shader.name);
//        //    }
//        //}
//#endif
//    }
}
