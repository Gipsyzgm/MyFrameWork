using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class EditSpritAtlas : EditorWindow
{

    public static bool _SpritAtlasIsInBuild = true;

    public static bool SpritAtlasIsInBuild
    {
        get
        {
            SpritAtlasIsInBuild = EditorPrefs.GetBool("SpritAtlasIsInBuild", false);
            return _SpritAtlasIsInBuild;
        }
        set
        {
            _SpritAtlasIsInBuild = value;
            EditorPrefs.SetBool("SpritAtlasIsInBuild", value);
        }
    }
    //[MenuItem("Assets/设置UIAtlas 编辑模式可显示", priority = 0)]
    //static void AutoSetASTC()
    //{
    //    SetUIAtlas(true);
    //}

    const string SpritAtlasPlatformSettings = @"
    - serializedVersion: 3
      m_BuildTarget: Android
      m_MaxTextureSize: 2048
      m_ResizeAlgorithm: 0
      m_TextureFormat: 47
      m_TextureCompression: 1
      m_CompressionQuality: 50
      m_CrunchedCompression: 0
      m_AllowsAlphaSplitting: 0
      m_Overridden: 1
      m_AndroidETC2FallbackOverride: 0
      m_ForceMaximumCompressionQuality_BC6H_BC7: 1
    - serializedVersion: 3
      m_BuildTarget: iPhone
      m_MaxTextureSize: 2048
      m_ResizeAlgorithm: 0
      m_TextureFormat: 48
      m_TextureCompression: 1
      m_CompressionQuality: 50
      m_CrunchedCompression: 0
      m_AllowsAlphaSplitting: 0
      m_Overridden: 1
      m_AndroidETC2FallbackOverride: 0
      m_ForceMaximumCompressionQuality_BC6H_BC7: 1
    packingSettings:
      serializedVersion: 2
      padding: 4
      blockOffset: 1
      allowAlphaSplitting: 0
      enableRotation: 1
      enableTightPacking: 0
    variantMultiplier: 1
    packables:";

    /// <summary>
    /// 设置图集属性
    /// </summary>
    public static void SetUIAtlas(bool isInBuild = false)
    {
        string uiAtlasDir = AppSetting.BundleResDir + AppSetting.UIAtlasDir;
        DirectoryInfo dir = new DirectoryInfo(uiAtlasDir);
        string s = "bindAsDefault: " + (isInBuild ? "0" : "1");
        string r = "bindAsDefault: " + (isInBuild ? "1" : "0");
        FileInfo[] files = dir.GetFiles("*.spriteatlas", SearchOption.AllDirectories);

        string startFlag = "platformSettings:";
        string endFlag = "packables:";
        foreach (FileInfo file in files)
        {
            string text = string.Empty;
            using (StreamReader stream = file.OpenText())
            {
                text = stream.ReadToEnd();
                int startIndex = text.IndexOf(startFlag) + startFlag.Length;
                int endIndex = text.IndexOf(endFlag) + endFlag.Length;
                text = text.Remove(startIndex, endIndex - startIndex);
                text = text.Replace(s, r);
                text = text.Insert(startIndex, SpritAtlasPlatformSettings);
            }
            using (StreamWriter write = file.CreateText())
            {
                write.Write(text);
                write.Close();
                write.Dispose();
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(uiAtlasDir);
        AssetDatabase.Refresh();
        Debug.Log("UIAtlas 属性设置完成");
    }

    /// <summary>
    /// 设置单个SpriteAtlas属性
    /// </summary>
    public static void SetSpriteAtlas(string filePath)
    {
        bool isInBuild = SpritAtlasIsInBuild;
        string s = "bindAsDefault: " + (isInBuild ? "0" : "1");
        string r = "bindAsDefault: " + (isInBuild ? "1" : "0");
        string startFlag = "platformSettings:";
        string endFlag = "packables:";
        FileInfo file = new FileInfo(filePath);
        string text = string.Empty;
        using (StreamReader stream = file.OpenText())
        {
            text = stream.ReadToEnd();
            int startIndex = text.IndexOf(startFlag) + startFlag.Length;
            int endIndex = text.IndexOf(endFlag) + endFlag.Length;
            text = text.Remove(startIndex, endIndex - startIndex);
            text = text.Replace(s, r);
            text = text.Insert(startIndex, SpritAtlasPlatformSettings);
        }
        using (StreamWriter write = file.CreateText())
        {
            write.Write(text);
            write.Close();
            write.Dispose();
        }
        //AssetDatabase.SaveAssets();
        //AssetDatabase.ImportAsset(filePath);
        AssetDatabase.Refresh();
    }
}