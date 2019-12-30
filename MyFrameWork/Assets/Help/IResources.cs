using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 如果是做单机用Resources.Load就行了
 * 做网游都要用Lua热更新这个类又显得多余
 */

public class IResources : MonoBehaviour {

    public static Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

    public static AssetBundleManifest abManifest = null;
    public static string[] abList;

    //弄成两个参数是因为方便bundle读取
    //resPath是最终路径，或者是bundle路径,resName是bundle里面资源名,如果bundle里面没有其他资源这个不填也可以
    public static T LoadRes<T>(string resPath,string resName = "") where T : Object
    {
#if UNITY_EDITOR
        return Resources.Load<T>(resPath + resName);
#else

        string resRoot = Application.streamingAssetsPath + "/" + IFile.bundleDir + "/";

        if (abManifest == null)
        {
            string abRoot = resRoot + IFile.bundleDir;
            AssetBundle ab = AssetBundle.LoadFromFile(abRoot);
            abManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            abList = abManifest.GetAllAssetBundles();
            ab.Unload(false);
        }

        AssetBundle bundle;

        if (bundles.ContainsKey(resPath))
        {
            bundle = bundles[resPath];
        }
        else
        {
            string[] abd = abManifest.GetAllDependencies(resPath);
            for (int i = 0; i < abd.Length; i++)
            {
                //先加载依赖
                if(!bundles.ContainsKey(abd[i]))
                {
                    AssetBundle ab = AssetBundle.LoadFromFile(resRoot + abd[i]);
                    bundles.Add(abd[i], ab);
                }
            }
            if(resName == string.Empty)
            {
                string[] assetsName = resPath.Split('/');
                resName = assetsName[assetsName.Length - 1];
            }
            bundle = AssetBundle.LoadFromFile(resRoot + resPath);
        }
        T little = bundle.LoadAsset<T>(resName);
        return little;
#endif
    }
    static void Unload()
    {
        foreach(var dic in bundles)
        {
            if (dic.Value != null)
                dic.Value.Unload(false);//true代表正在使用的也会清除
            bundles.Remove(dic.Key);
        }
    }
    //每隔一段时间卸载不用的Bundle
    public static void UnloadByTick()
    {
        IUpdate.Instance.AddUpdateHandheld(Unload, 60);
    }

    //Lua不支持直接调用泛型,供Lua调用
    public static GameObject LoadResGameObject(string resPath, string resName)
    {
        return LoadRes<GameObject>(resPath, resName);
    }
    public static Sprite LoadResSprite(string resPath, string resName)
    {
        return LoadRes<Sprite>(resPath, resName);
    }
    public static Texture LoadResTexture(string resPath, string resName)
    {
        return LoadRes<Texture>(resPath, resName);
    }
    public static Shader LoadResShader(string resPath, string resName)
    {
        return LoadRes<Shader>(resPath, resName);
    }
    public static TextAsset LoadResTextAsset(string resPath, string resName)
    {
        return LoadRes<TextAsset>(resPath, resName);
    }
    public static Material LoadResMaterial(string resPath, string resName)
    {
        return LoadRes<Material>(resPath, resName);
    }
    public static AudioClip LoadResAudioClip(string resPath, string resName)
    {
        return LoadRes<AudioClip>(resPath, resName);
    }
    public static Mesh LoadResMesh(string resPath, string resName)
    {
        return LoadRes<Mesh>(resPath, resName);
    }
}
