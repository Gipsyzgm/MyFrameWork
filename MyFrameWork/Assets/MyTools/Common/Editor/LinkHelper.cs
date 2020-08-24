using System;
using System.IO;
using UnityEditor;
using UnityEngine;

    /// <summary>
    /// 硬链接目录工具。。。支持win+mac, 需要win 7以上才有mklink命令
    /// </summary>
    public class LinkHelper
    {
        /// <summary>
        /// 删除硬链接目录
        /// </summary>
        /// <param name="linkPath"></param>
        public static void DeleteLink(string linkPath)
        {
            var os = Environment.OSVersion;
            if (os.ToString().Contains("Windows"))
            {
                MyEditorTools.ExecuteCommand(String.Format("rmdir \"{0}\"", linkPath));
            }
            else if (os.ToString().Contains("Unix"))
            {
                MyEditorTools.ExecuteCommand(String.Format("rm -Rf \"{0}\"", linkPath));
            }
            else
            {
                Debug.LogError(String.Format("[SymbolLinkFolder]Error on OS: {0}", os.ToString()));
            }
        }

        public static void SymbolLinkFolder(string srcFolderPath, string targetPath)
        {
            var os = Environment.OSVersion;
            if (os.ToString().Contains("Windows"))
            {
                MyEditorTools.ExecuteCommand(String.Format("mklink /J \"{0}\" \"{1}\"", targetPath, srcFolderPath));
            }
            else if (os.ToString().Contains("Unix"))
            {
                var fullPath = Path.GetFullPath(targetPath);
                if (fullPath.EndsWith("/"))
                {
                    fullPath = fullPath.Substring(0, fullPath.Length - 1);
                    fullPath = Path.GetDirectoryName(fullPath);
                }
                MyEditorTools.ExecuteCommand(String.Format("ln -s {0} {1}", Path.GetFullPath(srcFolderPath), fullPath));
            }
            else
            {
                Debug.LogError(String.Format("[SymbolLinkFolder]Error on OS: {0}", os.ToString()));
            }
        }

        /// <summary>
        /// 删除指定目录所有硬链接
        /// </summary>
        /// <param name="assetBundlesLinkPath"></param>
        public static void DeleteAllLinks(string assetBundlesLinkPath)
        {
            if (Directory.Exists(assetBundlesLinkPath))
            {
                foreach (var dirPath in Directory.GetDirectories(assetBundlesLinkPath))
                {
                    DeleteLink(dirPath);
                }
            }

        }
        /// <summary>
        /// 创建StreamingAssets链接
        /// </summary>
        public static void MkLinkStreamingAssets()
        {
            string linkPath = Application.streamingAssetsPath + "/" + Utility.GetPlatformName();
            if (IsLinkStreamingAssets)
            {
                MyEditorTools.CreateDir(Application.streamingAssetsPath);
                var exportPath = AppSetting.ExportResBaseDir + Utility.GetPlatformName();
                MyEditorTools.DeleteDir(linkPath); //删除复制过来的文件夹
                SymbolLinkFolder(exportPath, linkPath);
            }
            else
            {
                DeleteLink(linkPath);
            }
            AssetDatabase.Refresh();
        }
        public static void DeleteLinkStreamingAssets()
        {
            string linkPath = Application.streamingAssetsPath + "/" + Utility.GetPlatformName();
            if (IsLinkStreamingAssets)
            {
                DeleteLink(linkPath);
                IsLinkStreamingAssets = false;
            }
            else
                MyEditorTools.DeleteDir(linkPath);
        }

        private static bool _IsLinkStreamingAssets = false;
        /// <summary>
        /// 是否连接资源StreamingAssets
        /// </summary>
        public static bool IsLinkStreamingAssets {
            get {
                _IsLinkStreamingAssets = EditorPrefs.GetBool("IsLinkStreamingAssets", false);
                return _IsLinkStreamingAssets;
            }
            set {
                _IsLinkStreamingAssets = value;
                EditorPrefs.SetBool("IsLinkStreamingAssets", value);
            }
        }
    }