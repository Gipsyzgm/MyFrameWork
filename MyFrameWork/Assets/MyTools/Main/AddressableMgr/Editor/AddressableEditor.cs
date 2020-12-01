using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 所有资源全用远程构建远程加载非静态的方式来做。
/// 通过DefaultBuild来做首次构建。
/// 关于crc建议：静态资源不要勾选,非静态资源勾不勾选都可以
/// 安卓会直接缓存.待测试.
/// </summary>
public class AddressableEditor
{
    //加载目录地址资源路径
    static string resPath = AppSetting.BundleResDir;
    [MenuItem("AddressableEditor/自动分组",false,0)]
    public static void AutoGroup()
    {
        //是否启用简单命名方式
        bool simplied = false;
        string assetPath;
        string[] dirs = Directory.GetDirectories(resPath);
        foreach (var item in dirs)
        {
            DirectoryInfo dir = new DirectoryInfo(item);
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetGroup Group = CreateOrGetNonStaticGroup(settings, dir.Name);        
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            foreach (var file in files)
            {
                if (file.FullName.EndsWith(".meta")) continue;
                assetPath = "Assets" + file.FullName.Substring(Application.dataPath.Length);
                var guid = AssetDatabase.AssetPathToGUID(assetPath);
                var entry = settings.CreateOrMoveEntry(guid, Group);
                Debug.Log(assetPath);
                entry.address = assetPath;
                if (simplied)
                {
                    entry.address = Path.GetFileNameWithoutExtension(assetPath);
                }
                //设置资源标签
                //entry.SetLabel("labelname",true,true);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    //创建组
    private static AddressableAssetGroup CreateOrGetNonStaticGroup(AddressableAssetSettings settings, string groupName)
    {
        var group = settings.FindGroup(groupName);
        if (group == null)
            group = settings.CreateGroup(groupName, false, false, false, null, typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
        group.GetSchema<ContentUpdateGroupSchema>().StaticContent = false;
        BundledAssetGroupSchema groupSchema = group.GetSchema<BundledAssetGroupSchema>();
        //groupSchema.UseAssetBundleCrc = false;
        //groupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.OnlyHash;
        groupSchema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;
        groupSchema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kRemoteBuildPath);
        groupSchema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kRemoteLoadPath);
        return group;    
    }

    //打包
    [MenuItem("AddressableEditor/DefaultBuild",false, 1)]
    public static void BuildContent()
    {
        Debug.LogError("首次打包使用,会总把bundle资源克隆到对应的StreamingAsset目录");
        AddressableAssetSettings.BuildPlayerContent();
        string linkPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/" + AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings);
        Debug.LogError(linkPath);
        var exportPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/" + Addressables.BuildPath+"/"+UnityEditor.EditorUserBuildSettings.activeBuildTarget;
        Debug.LogError(exportPath);
        MyEditorTools.CopyDirectory(linkPath,exportPath,true); 
        AssetDatabase.Refresh();

    }

    //更新
    [MenuItem("AddressableEditor/打静态资源更新包", false, 2)]
    public static void CheckForUpdateContent()
    {
        //与上次打包做资源对比
        string buildPath = ContentUpdateScript.GetContentStateDataPath(false);
        var m_Settings = AddressableAssetSettingsDefaultObject.Settings;
        List<AddressableAssetEntry> entrys = ContentUpdateScript.GatherModifiedEntries(m_Settings, buildPath);
        if (entrys.Count == 0)
        {
            Debug.Log("没有资源变更");
            return;
        }
        StringBuilder sbuider = new StringBuilder();
        sbuider.AppendLine("Need Update Assets:");
        foreach (var _ in entrys)
        {
            sbuider.AppendLine(_.address);
        }
        Debug.Log(sbuider.ToString());
        //将被修改过的资源单独分组---可以自定义组名
        var groupName = string.Format("UpdateGroup_{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
        ContentUpdateScript.CreateContentUpdateGroup(m_Settings, entrys, groupName);
    }

    [MenuItem("AddressableEditor/清除所有标签",false,3)]
    public static void ClearLabel()
    {
        AddressableAssetSettings assetSettings = AddressableAssetSettingsDefaultObject.GetSettings(false);
        var labelList = assetSettings.GetLabels();
        foreach (var item in labelList)
        {
            assetSettings.RemoveLabel(item);
        }
    }

    [MenuItem("AddressableEditor/添加已定义的标签",false,4)]
    public static void ClearAllLabel()
    {
        //需要添加标签
        AddressableAssetSettings assetSettings = AddressableAssetSettingsDefaultObject.GetSettings(false);
        for (int i = assetSettings.groups.Count - 1; i >= 0; --i)
        {
            AddressableAssetGroup assetGroup = assetSettings.groups[i];
            foreach (var item in assetGroup.entries)
            {
               // item.SetLabel("已定义的标签名",true);
            }       
        }
    }

    [MenuItem("AddressableEditor/打开远程Build目录",false,5)]
    public static void BuildUpdate()
    {
        var m_Settings = AddressableAssetSettingsDefaultObject.Settings;
        string path = (Application.dataPath.Substring(0,Application.dataPath.LastIndexOf('/')) +"/"+m_Settings.RemoteCatalogBuildPath.GetValue(m_Settings)) ;
        MyEditorTools.ShowExplorer(path.Substring(0, path.LastIndexOf('/')));
    }

    [MenuItem("AddressableEditor/打开本地构建目录", false, 6)]
    public static void OpenLocalBuild()
    {
        MyEditorTools.ShowExplorer(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) +"/"+Addressables.BuildPath);
    }


    [MenuItem("AddressableEditor/打开缓存目录", false, 7)]
    public static void OpenPersist()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
}