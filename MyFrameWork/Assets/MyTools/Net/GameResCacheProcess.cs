// using System;
// using UnityEngine;
// using UnityEngine.UI;
//
//
// public class GameResCacheProcess : ResCacheProcess
// {
//     [SerializeField] public ResReference resref;
//
//     [NonSerialized] private GameObject panel;
//     [NonSerialized] private Text m_Title;
//     [NonSerialized] private string[] texts0;
//     [NonSerialized] private string[] texts1;
//
//     public override void display()
//     {
//         var canvas = GameObject.Find("Canvas");
//         var pref = GameLoader.loadSync<GameObject>(resref.combinAssetPath);
//         panel = Instantiate(pref, canvas.transform);
//         m_Title = panel.transform.Find("m_Title").GetComponent<Text>();
//         texts0 = new string[] { "初始化...", "资源更新中...", "资源验证中..." };
//         texts1 = new string[] { "B", "KB", "MB" };
//     }
//
//     public override void cacheError(int step)
//     {
//         endCache();
//     }
//
//     public override void startCache(int step, long byteTotal)
//     {
//         if (m_Title != null)
//             m_Title.gameObject.SetActive(true);
//     }
//
//     public override void cacheProgress(int step, long byteLoaded, long byteTotal)
//     {
//         if (m_Title != null)
//         {
//             m_Title.text =
//                 $"{texts0[step]}{Utils.getByteDescript(byteLoaded, out var bType)}{texts1[bType]}/{Utils.getByteDescript(byteTotal, out bType)}{texts1[bType]}";
//         }
//     }
//
//     public override void endCache()
//     {
//         if (panel != null)
//             DestroyImmediate(panel);
//     }
// }