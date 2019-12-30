using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Text;
using System.IO;

/*
 * 
 * 界面拼好后，把需要记下来的控件___点击/右键/自动生成UI/标记类型/XXX___标记好需要的控件,在主节点上右键___自动生成UI/生成UI脚本__就可以把界面导出来了
 */

public class ExportUI {
    //结束标志
    static string replace = "@EndMark@";
    //UI代码存储位置
    static string uiScriptDir = Application.dataPath + "/Scripts/MyUI/View/";
    //Item代码存储位置
    static string itemScriptDir = Application.dataPath + "/Scripts/MyUI/Item/";
    //UI预制体存储位置
    static string uiDir = "Assets/Resources/MyUI/View/";
    //Item预制体存储位置
    static string itemDir = "Assets/Resources/MyUI/Item/";
    //路径名
    static string uiPathName = "PathUI";
    static string itemPathName = "PathItem";
    //继承脚本
    static string overrideUI = "UIBase";
    static string overrideItem = "MonoBehaviour";
    //导出类型
    static int exportType = 0;

    static string scriptDir = "";
    static string prefabDir = "";
    static string overrideScript = "";
    static string pathName = "";

    [MenuItem("GameObject/自动生成UI/生成UI脚本", priority = 1)]
    public static void Export_UI()
    {
        exportType = 0;
        scriptDir = uiScriptDir;
        prefabDir = uiDir;
        pathName = uiPathName;
        overrideScript = overrideUI;
        Export();
    }
    [MenuItem("GameObject/自动生成UI/生成Item脚本", priority = 2)]
    public static void Export_Item()
    {
        exportType = 1;
        scriptDir = itemScriptDir;
        prefabDir = itemDir;
        pathName = itemPathName;
        overrideScript = overrideItem;
        Export();
    }
    public static void Export()
    {
        //选择是不是单个物体
        if (Selection.gameObjects.Length != 1) return;
        Transform transSelect = Selection.gameObjects[0].transform;
        Transform[] objs = transSelect.GetComponentsInChildren<Transform>(true);
        Dictionary<string, Type> path_type_dic = new Dictionary<string, Type>();
        Dictionary<string, string> path_name_dic = new Dictionary<string, string>();
        //存储物体信息。
        for (int i = 0; i < objs.Length; i++)
        {
            foreach (var key in dict.Keys)
            {
                if (objs[i].name.Contains(key))
                {
                    string path = TransformPath(objs[i].transform, transSelect, objs[i].name);
                    if (path == null)
                    {
                        Debug.LogError("生成失败，作为页面的父物体不可被标记。");
                        return;
                    }
                    if (path_type_dic.ContainsKey(path))
                    {
                        Debug.Log("生成失败，重名了，检查一下:" + objs[i].name);
                        return;
                    }
                    path_type_dic.Add(path, dict[key]);
                    path_name_dic.Add(path, objs[i].name.Split('_')[0]);
                }
            }
        }
        //创建文件夹
        if (!Directory.Exists(scriptDir))
            Directory.CreateDirectory(scriptDir);
        if (!Directory.Exists(prefabDir))
            Directory.CreateDirectory(prefabDir);
        //创建Prefab
        UnityEngine.Object prefabObj = null;
        string dirObj = prefabDir + transSelect.gameObject.name + ".prefab";

        if (!File.Exists(dirObj))
            PrefabUtility.CreateEmptyPrefab(dirObj);
        prefabObj = AssetDatabase.LoadAssetAtPath(dirObj, typeof(UnityEngine.Object));
        //使用PrefabUtility.ReplacePrefab将场景中选中的Prefab实例制作成一个Prefab并覆盖到之前的空prefab上
        PrefabUtility.ReplacePrefab(transSelect.gameObject, prefabObj);
        //ToCS
        ToCS(transSelect.name, path_type_dic, path_name_dic);
        //ToLua
        ToLua(transSelect.name, path_type_dic, path_name_dic);

        AssetDatabase.Refresh();
    }
    //拼接C#脚本
    static void ToCS(string className, Dictionary<string, Type> path_type_dic, Dictionary<string, string> path_name_dic)
    {
        StringBuilder sb = new StringBuilder();
        //Append是不加回车的拼接；AppendLine是加回车的拼接。
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using UnityEngine.UI;");
        sb.AppendLine("using System.Collections;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine();
        sb.AppendLine("public class " + className + " : "+ overrideScript +" {");
        sb.AppendLine();
        //定义Ui变量名
        foreach (var key in path_type_dic.Keys)
        {
            Type type = path_type_dic[key];
            string name = path_name_dic[key];
            sb.AppendLine("    public " + type.Name + " " + name + ";");
        }
        if(exportType == 1)
        {
            sb.AppendLine("    public int index = 0;");
        }
        sb.AppendLine("    void Awake()");
        sb.AppendLine("    {");
        //查找Ui变量名
        foreach (var key in path_type_dic.Keys)
        {
            sb.Append("        ");
            string path = key;
            Type type = path_type_dic[key];
            string name = path_name_dic[key];
            if (type == typeof(GameObject))
                sb.AppendLine(name + " = " + "transform.Find(" + '"' + path + '"' + ").gameObject" + ";");
            else if (type == typeof(Transform))
                sb.AppendLine(name + " = " + "transform.Find(" + '"' + path + '"' + ").transform" + ";");
            else
                sb.AppendLine(name + " = " + "transform.Find(" + '"' + path + '"' + ").GetComponent<" + type.Name + ">()" + ";");
            if (type == typeof(Button))
            {
                string methodName = name + "OnClick";
                sb.AppendLine("        " + name + ".onClick.AddListener(" + methodName + ");");
            }
        }
        sb.AppendLine("        CustomComponent();");
        sb.AppendLine("    }");
        sb.AppendLine("    //=======================上面部分自动生成，每次生成都会替换掉,不要手写东西==================");
        sb.AppendLine();
        sb.AppendLine("    //=====================================手写部分=============================================");
        sb.AppendLine("    //" + replace);


        string scriptFile = scriptDir + className + ".cs";
        //如果存在脚本（替换操作replace前的代码。）
        if (File.Exists(scriptFile))
        {
            string csString = File.ReadAllText(scriptFile);
            if (!csString.Contains(replace))
            {
                Debug.Log(replace + "没找到" + replace + "在Awake方法后一行补上//" + replace);
                return;
            }            
            int splitIndex = csString.IndexOf(replace) + replace.Length;
            //substring（int beginIndex） 意思为返回下标从beginIndex开始（包括），到字符串结尾的子字符串。
            //存起来replace标记后面的代码
            csString = csString.Substring(splitIndex);

            //判断按钮的回调是否存在
            foreach (var key in path_type_dic.Keys)
            {
                Type type = path_type_dic[key];
                string name = path_name_dic[key];
                if (type == typeof(Button))
                {
                    string methodName = name + "OnClick";
                    if (!csString.Contains(methodName + "()"))
                    {
                        sb.AppendLine("    public void " + methodName + "()");
                        sb.AppendLine("    {");
                        sb.AppendLine("        ");
                        sb.AppendLine("    }");
                    }
                }
            }
            if (!csString.Contains("CustomComponent()"))
            {
                sb.AppendLine("    public void CustomComponent()");
                sb.AppendLine("    {");
                sb.AppendLine("        ");
                sb.AppendLine("    }");
            }
            csString = "    " + csString.TrimStart();
            sb.Append(csString);
        }
        else
        {
            foreach(var key in path_type_dic.Keys)
            {
                Type type = path_type_dic[key];
                string name = path_name_dic[key];
                if (type == typeof(Button))
                {
                    string methodName = name + "OnClick";
                    sb.AppendLine("    public void " + methodName + "()");
                    sb.AppendLine("    {");
                    sb.AppendLine("        ");
                    sb.AppendLine("    }");
                }
            }
            sb.AppendLine("    public void CustomComponent()");
            sb.AppendLine("    {");
            sb.AppendLine("        ");
            sb.AppendLine("    }");
            if(exportType == 0)
            {
                sb.AppendLine("    public override void Open()");
                sb.AppendLine("    {");
                sb.AppendLine("        ");
                sb.AppendLine("    }");
                sb.AppendLine("    public override void Close()");
                sb.AppendLine("    {");
                sb.AppendLine("        ");
                sb.AppendLine("    }");
            }
            if(exportType == 1)
            {
                sb.AppendLine("    public void UpdateItem()");
                sb.AppendLine("    {");
                sb.AppendLine("        ");
                sb.AppendLine("    }");
            }
            sb.AppendLine("}");
        }
        //参数1为文件绝对路径，创建一个新文件，使用指定的编码在其中写入指定的字符串数组，然后关闭文件。如果目标文件已存在，则改写该文件。
        File.WriteAllText(scriptFile, sb.ToString(), Encoding.UTF8);

        StringBuilder sbPath = new StringBuilder();
        sbPath.AppendLine("public class " + pathName);
        sbPath.AppendLine("{");
        string resPath = "";
        string[] str = prefabDir.Split('/');
        for (int i = 2; i < str.Length; i++)
            resPath += str[i] + "/";
        //Substring（int beginIndex，int endIndex） 意思为返回下标从beginIndex开始（包括），到endIndex（不包括）结束的子字符串。
        resPath = resPath.Substring(0, resPath.Length - 2); //剔除后缀名
        DirectoryInfo direction = new DirectoryInfo(prefabDir);
        //DirectoryInfo.GetFiles返回当前目录的文件列表
        //DirectoryInfo 的 GetFiles 返回的是 FileInfo[]，而 Directory.GetFiles 返回的是 string[]
        //通配符是一种特殊语句，主要有星号(*)和问号(?)
        //SearchOption是设置文件夹的。TopDirectoryOnly值检索当前文件夹。AllDirectories检索当前文件夹及子文件夹
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].Name.EndsWith(".prefab")) continue;
            string prefabName = files[i].Name.Split('.')[0];
            sbPath.AppendLine("    public static string " + prefabName + " = " + '"' + resPath + "/" + prefabName + '"' + ";");
        }
        sbPath.AppendLine("}");
        string scriptFilePath = scriptDir + pathName + ".cs";
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        Debug.Log("成功生成脚本");

    }
    static void ToLua(string className, Dictionary<string, Type> path_type_dic, Dictionary<string, string> path_name_dic)
    {

    }
    /// <summary>
    /// 递归找拼接子物体位置信息//递归找父物体
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="root"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    static string TransformPath(Transform trans, Transform root, string path)
    {
        //直到找到父物体，返回path

        if (trans == root)
        {          
            return null;
        }
        if (trans.parent == root) return path;
        Transform parent = trans.parent;      
        path = parent.name + "/" + path;
        return TransformPath(parent, root, path);
    }
    static Dictionary<string, Type> dict = new Dictionary<string, Type>()
    {
        { "_GameObject",     typeof(GameObject) },
        { "_Transform",      typeof(Transform) },
        { "_Image",          typeof(Image) },
        { "_Text",           typeof(Text) },
        { "_Button",         typeof(Button) },
        { "_ScrollRect",     typeof(ScrollRect) },
        { "_GridLayoutGroup",typeof(GridLayoutGroup) },
        { "_Slider",         typeof(Slider) },
        { "_DropDown",       typeof(Dropdown) },
        { "_RawImage",       typeof(RawImage) },
        { "_InputField",     typeof(InputField) },
        //还需要什么就在这里加
    };
    //=================================加后缀===================================

    [MenuItem("GameObject/自动生成UI/标记类型/GameObject", priority = 0)]
    public static void TypeGameObject()
    {
        SetName(typeof(GameObject));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/Transform", priority = 1)]
    public static void TypeTransform()
    {
        SetName(typeof(Transform));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/Image", priority = 2)]
    public static void TypeImage()
    {
        SetName(typeof(Image));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/Text", priority = 3)]
    public static void TypeText()
    {
        SetName(typeof(Text));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/Button", priority = 4)]
    public static void TypeButton()
    {
        SetName(typeof(Button));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/ScrollRect", priority = 5)]
    public static void TypeScrollRect()
    {
        SetName(typeof(ScrollRect));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/GridLayoutGroup", priority = 6)]
    public static void TypeGridLayoutGroup()
    {
        SetName(typeof(GridLayoutGroup));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/Slider", priority = 7)]
    public static void TypeSlider()
    {
        SetName(typeof(Slider));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/Dropdown", priority = 8)]
    public static void TypeDropdown()
    {
        SetName(typeof(Dropdown));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/RawImage", priority = 9)]
    public static void TypeRawImage()
    {
        SetName(typeof(RawImage));
    }
    [MenuItem("GameObject/自动生成UI/标记类型/InputField", priority = 10)]
    public static void TypeInputField()
    {
        SetName(typeof(InputField));
    }

    static void SetName(Type type)
    {
        for(int i=0;i<Selection.gameObjects.Length;i++)
        {
            GameObject obj = Selection.gameObjects[i];

            bool down = false;
            foreach (var key in dict.Keys)
            {
                if (down) break;
                //如果名字里包含key
                if (obj.name.Contains(key))
                {
                    foreach (var key0 in dict.Keys)
                    {
                        if (type.Name.Equals(dict[key0].Name))
                        {
                            obj.name = obj.name.Replace(key, key0);
                            down = true;
                            break;
                        }
                    }
                }
            }
            if (down) continue;

            foreach (var key in dict.Keys)
            {
                if (type.Name.Equals(dict[key].Name))
                {
                    obj.name += key;
                    break;
                }
            }
        }
    }
}
