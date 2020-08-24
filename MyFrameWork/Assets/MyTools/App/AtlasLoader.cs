using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// 当任何Sprite绑定到SpriteAtlas但在运行时找不到Atlas资源时触发。
/// 这通常意味着将精灵打包到构建中未包含的地图集。
/// 此回调不希望用户立即做出响应。相反，它将传递System.Action。用户可以稍后加载地图集对象，并使用此System.Action传回已加载的地图集。
/// 图集热更不会包含在AssetBundle的依赖项里面。需要单独处理。
/// </summary>

public class AtlasLoader : MonoBehaviour
{
    void OnEnable()
    {
        SpriteAtlasManager.atlasRequested += RequestAtlas;
    }
    void OnDisable()
    {
        SpriteAtlasManager.atlasRequested -= RequestAtlas;
    }

    void RequestAtlas(string tag, System.Action<SpriteAtlas> callback)
    {
        SpriteAtlas objs = ABMgr.Instance.LoadSpriteAtlas(tag);
        callback(objs);  
    }
}
