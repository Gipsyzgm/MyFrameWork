using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用以记录一些基本不会变动的路径
/// </summary>
public class MyDefaultPath{

    /// <summary>
    /// 编辑器环境下所有需要挂在的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public static readonly string EditorEnvPath = "Assets/MyTools/Common/EditorEnv";
    /// <summary>
    /// 正式环境下所有需要挂在的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public static readonly string FormalEnvPath = "Assets/MyTools/Common/FormalEnv";
    /// <summary>
    /// 公共文件所有需要挂在的不区分环境的通用脚本都可以放在这个文件夹路径下，可以通过我的工具/环境来意见挂载所有的文件夹下的脚本
    /// </summary>
    public static readonly string PublicEnvPath = "Assets/MyTools/Common/PublicEnv";



}
