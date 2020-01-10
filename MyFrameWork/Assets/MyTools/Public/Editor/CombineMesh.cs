/*
 *  项目名字：MyFrameWork
 *  创建时间：2019.1.8
 *  描述信息：合并mesh，作为优化项使用，可以批量合并mesh，需要注意材质和碰撞体的变更。 
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class CombineMesh : MonoBehaviour {

    //mesh目录
    static string meshDir = "Assets/MargeMesh/Mesh/";
    static string meshObjDir = "Assets/MargeMesh/ObjMesh/";
   /// <summary>
   /// 使用需创建物体，然后添加mesh和其他组件
   /// </summary>
    [MenuItem("GameObject/合并网格/合并网格", priority = 0)]
    static void Combine()
    {
        if (!Directory.Exists(meshDir))
            Directory.CreateDirectory(meshDir);
        if (!Directory.Exists(meshObjDir))
            Directory.CreateDirectory(meshObjDir);
        GameObject[] objs = Selection.gameObjects;

        for (int j = 0; j < objs.Length; j++)
        {
            MeshFilter[] meshfilters = objs[j].GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshfilters.Length];
            Matrix4x4 matrix = objs[j].transform.worldToLocalMatrix;
            for (int i = 0; i < meshfilters.Length; i++)
            {           
                MeshFilter mf = meshfilters[i];
                MeshRenderer mr = mf.GetComponent<MeshRenderer>();
                if (mr == null) continue;                      
                combine[i].mesh = mf.sharedMesh;
                //矩阵(Matrix)自身空间坐标的点转换成世界空间坐标的点   
                //combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                //变换矩阵的问题，要保持相对位置不变，要转换为父节点的本地坐标，
                combine[i].transform = matrix * mf.transform.localToWorldMatrix;           
            }
            Mesh mesh = new Mesh();
            mesh.name = objs[j].name;         
            mesh.CombineMeshes(combine, true);           
            string path = meshDir + mesh.name + ".asset";
            AssetDatabase.CreateAsset(mesh, path);
            Debug.LogError("合并网格成功，使用需添加对应的碰撞和材质。");
        }
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 快速构建，直接合并成新物体，增删对应组件即可
    /// </summary>
    [MenuItem("GameObject/合并网格/合并网格及物体", priority = 1)]
    static void CombineMeshAndObj()
    {
        if (!Directory.Exists(meshDir))
            Directory.CreateDirectory(meshDir);
        if (!Directory.Exists(meshObjDir))
            Directory.CreateDirectory(meshObjDir);
        GameObject[] objs = Selection.gameObjects;

        for (int j = 0; j < objs.Length; j++)
        {
            MeshFilter[] meshfilters = objs[j].GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshfilters.Length];
            //MeshRenderer[] Renderer = objs[j].GetComponentsInChildren<MeshRenderer>();  //获取自身和所有子物体中所有MeshRenderer组件
            //Material[] mats = new Material[Renderer.Length];            
            //if (meshfilters.Length!= Renderer.Length)
            //{
            //    Debug.LogError("合并失败,子物体是MeshRenderer或MeshFilter组件需同时存在或者不存在，请检查是否有误");
            //    return;
            //}          
            Matrix4x4 matrix = objs[j].transform.worldToLocalMatrix;
            for (int i = 0; i < meshfilters.Length; i++)
            {
                //mats[i] = Renderer[i].sharedMaterial;
                MeshFilter mf = meshfilters[i];
                MeshRenderer mr = mf.GetComponent<MeshRenderer>();
                if (mr == null) continue;
                combine[i].mesh = mf.sharedMesh;
                //矩阵(Matrix)自身空间坐标的点转换成世界空间坐标的点   
                //combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                //变换矩阵的问题，要保持相对位置不变，要转换为父节点的本地坐标，
                combine[i].transform = matrix * mf.transform.localToWorldMatrix;
                if (i != 0)
                {
                    DestroyImmediate(mf.gameObject);
                }

            }
            Mesh mesh = new Mesh();
            mesh.name = objs[j].name;
            //为mesh.CombineMeshes添加一个false参数，表示并不是合并为一个网格，而是一个子网格列表，可以让拥有多个材质球
            //如果要合并的网格,用的是同一材质，false改为true，同时将上面的获取Material的代码去掉
            mesh.CombineMeshes(combine, true);
            //objParent.GetComponent<MeshRenderer>().sharedMaterials = mats;
            objs[j].GetComponent<MeshFilter>().mesh = mesh;
            string path = meshObjDir + mesh.name + ".asset";
            AssetDatabase.CreateAsset(mesh, path);
            Debug.LogError("合并网格和物体成功，使用需重新添加对应的碰撞和材质。");
        }
        AssetDatabase.Refresh();
    }
}
