using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr
{
    /// <summary>资源管理器</summary>
    public static AssetbundleMgr Assetbundle;
    /// <summary>网络管理器</summary>
    //public static NetMgr Net;
    /// <summary>UI管理器</summary>
    public static PanelMgr UI;
    /// <summary>ILRuntime管理器</summary>
    //public static ILRMgr ILR;
    /// <summary>版本检测管理器</summary>
    public static VersionCheckMgr VersionCheck;

    // Start is called before the first frame update
    public static void Initialize()
    {
        Assetbundle = AssetbundleMgr.Instance;
        UI = PanelMgr.Instance;
        VersionCheck = VersionCheckMgr.Instance;
    }

  
}
