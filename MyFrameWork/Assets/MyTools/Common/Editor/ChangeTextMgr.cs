using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextMgr
{
    //目标路径
    public static string TargetPath = Application.dataPath + "/GameRes/BundleRes/Prefabs/MyUI/";
    //修改倍数
    public static float NewSize = 1.8f;
    // Start is called before the first frame update
    [MenuItem("我的工具/其他/批量修改预制体文字（慎用，效果叠加）")]
    public static void ChangePrefabs()
    {
        Debug.Log(TargetPath);
        //获得指定路径下面的所有资源文件
        //if (Directory.Exists(TargetPath))
        //{
        //    DirectoryInfo dirInfo = new DirectoryInfo(TargetPath);
        //    FileInfo[] files = dirInfo.GetFiles("*", SearchOption.AllDirectories); //包括子目录
        //    Debug.Log(files.Length);
        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        //所有预制体
        //        if (files[i].Name.EndsWith(".prefab"))
        //        {                      
        //            string tempPath = files[i].FullName.Replace(@"\", "/");
        //            string[] ary = tempPath.Split('/');
        //            int tempint = 0;
        //            string path = "Assets";
        //            //拼接资源路径
        //            for (int n = 0; n < ary.Length; n++)
        //            {
        //                if (ary[n] == "Assets")
        //                {
        //                    tempint = n;
        //                }
        //            }
        //            for (int x = tempint + 1; x < ary.Length; x++)
        //            {
        //                path += "/" + ary[x];
        //            }
        //            Debug.Log("路径："+path);
        //            GameObject obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        //            //获取所有包含text子物体
        //            Text[] AllChild = obj.GetComponentsInChildren<Text>(true);
        //            //按规则修改子物体
        //            foreach (Text child in AllChild)
        //            {
        //                Debug.Log(child.name);
        //                child.fontStyle = FontStyle.BoldAndItalic;
        //                if (child.rectTransform.anchorMin == Vector2.zero&& child.rectTransform.anchorMax == Vector2.one)
        //                {
        //                    Debug.LogError("自定义的锚点，跳过");
        //                    continue;
        //                }
        //                float height = child.rectTransform.rect.height * NewSize;
        //                Debug.LogError("开始" + height + "初始高：" + child.rectTransform.rect.height + "倍率：" + NewSize);
        //                if (child.rectTransform.rect.height * NewSize > child.transform.parent.GetComponent<RectTransform>().rect.height)
        //                {
        //                    height = child.transform.parent.GetComponent<RectTransform>().rect.height;
        //                    if (height == 0)
        //                    {
        //                        height = child.rectTransform.rect.height * NewSize;
        //                    }
        //                }
        //                else
        //                {
        //                    height = child.rectTransform.rect.height * NewSize;
        //                }
        //                float move = height - child.rectTransform.rect.height;
        //                child.rectTransform.sizeDelta = new Vector2(child.rectTransform.rect.width, height);
        //            }
        //            //通知你的编辑器 obj 改变了
        //            EditorUtility.SetDirty(obj);
        //            //保存修改
        //            AssetDatabase.SaveAssets();
        //            AssetDatabase.Refresh();
        //        }
        //    }
        //    Debug.LogError("修改完成");
        //}
        //else
        //{
        //    Debug.LogError("资源路径不存在");
        //}

    }

}


