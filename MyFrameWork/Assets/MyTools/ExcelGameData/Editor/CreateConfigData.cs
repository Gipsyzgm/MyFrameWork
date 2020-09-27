using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Reflection;
using System;
using LitJson;
using System.Data;
using Excel;
using OfficeOpenXml;
using ILRuntime.Runtime;
using UObject = UnityEngine.Object;

public class CreateConfigData : MonoBehaviour
{
    public static AllConfigInfo configInfo;
    public static AllHotConfigInfo hotConfigInfo;

    static string _path = "";
    /// <summary>
    /// 从第几行开始有数据
    /// </summary>
    static int startRow = 4;
    /// <summary>
    /// 写入资源的生成资源的路径,不需要热更的资源放在Resources下
    /// </summary>
    static string assetDir = "Assets/MyTools/ExcelGameData/Resources/";
    /// <summary>
    /// 写入资源的生成资源的路径,需要热更的资源放在BundleRes热更目录下
    /// </summary>
    static string HotFixassetDir = "Assets/GameRes/BundleRes/GameData/";  
    /// <summary>
    /// 不需要热更的excel写入AllConfigInfo的文件夹地址
    /// </summary>
    static string scriptDir = "Assets/MyTools/ExcelGameData/GameData/";
    /// <summary>
    /// 需要热更的excel写入AllConfigInfo的文件夹地址
    /// </summary>
    static string HotFixscriptDir = "Assets/MyTools/ExcelGameData/HotFixGameData/";
    /// <summary>
    /// AllConfigInfo的文件存放地址
    /// </summary>
    static string AllConfigInfoDir = "Assets/MyTools/ExcelGameData/AllConfigInfo/";
    /// <summary>
    /// ReadExcelInfo的文件存放地址,必须editor下
    /// </summary>
    static string ReadExcelInfoDir = "Assets/MyTools/ExcelGameData/Editor/";
    /// <summary>
    /// 不需要热更读取读Excel的类名
    /// </summary>
    static string ReadExcelName = "AllNeedReadExcel";
    /// <summary>
    /// 需要热更读取读Excel的类名
    /// </summary>
    static string HotFixReadExcelName = "HotFixAllNeedReadExcel";
    /// <summary>
    /// AllConfigInfo的类名/序列化数据的名称
    /// </summary>
    static string AllConfigName = "AllConfigInfo";
    /// <summary>
    /// AllHotConfigInfo的类名/序列化数据的名称
    /// </summary>
    static string AllHotConfigName = "AllHotConfigInfo";
    /// <summary>
    /// 生成初始化Data的方法
    /// </summary>
    static string DataInitInfo = "DataMgr";
    /// <summary>
    /// DataMgr生成的路径
    /// </summary>
    static string DataInitInfoDir = "Assets/MyTools/ExcelGameData/AllConfigInfo/";


    [MenuItem("我的工具/其他/配置Excel表格/读取配置表格", false, 2)]
    public static void ReadConfigData()
    {
        AssetDatabase.DeleteAsset(assetDir + AllConfigName + ".asset");
        AssetDatabase.DeleteAsset(HotFixassetDir + AllHotConfigName + ".asset");
        configInfo = ScriptableObject.CreateInstance<AllConfigInfo>();
        hotConfigInfo = ScriptableObject.CreateInstance<AllHotConfigInfo>();
        WriteAllConfigInfo();
        WriteHotFixAllConfigInfo();
        WriteReadExcelInfo();
        WriteHotFixReadExcelInfo();
        AutoReadExcelInfo();
        AutoReadHotFixExcelInfo();
        AssetDatabase.CreateAsset(configInfo, assetDir + AllConfigName + ".asset");
        AssetDatabase.CreateAsset(hotConfigInfo, HotFixassetDir + AllHotConfigName + ".asset");
        Debug.Log("读取配置完成,"+ assetDir + AllConfigName + ".asset");
        Debug.Log("读取配置完成," + HotFixassetDir + AllHotConfigName + ".asset");
        WriteDataInitInfo();

    }
    /// <summary>
    /// 自动写AllConfigInfo脚本
    /// </summary>
    public static void WriteAllConfigInfo()
    {
        if (!Directory.Exists(AllConfigInfoDir))
            Directory.CreateDirectory(AllConfigInfoDir);
        if (!Directory.Exists(scriptDir))
        {
            Debug.LogError("文件夹" + scriptDir + "不存在，请检查路径是否正确");
            return;
        }
        DirectoryInfo direction = new DirectoryInfo(scriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        StringBuilder sbPath = new StringBuilder();
        sbPath.AppendLine("using UnityEngine;");
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");
        sbPath.AppendLine("public class " + AllConfigName + ": ScriptableObject");
        sbPath.AppendLine("{");
        string scriptFilePath = AllConfigInfoDir + AllConfigName + ".cs";
        for (int x = 0; x < files.Length; x++)
        {
            if (files[x].Name.EndsWith(".meta")) continue;
            string prefabName = files[x].Name.Split('.')[0];
            sbPath.AppendLine("    public " + prefabName + "[] " + prefabName + ";");
        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.Log("把GameData下的配置文件写入AllConfigInfo脚本");
    }

    public static void WriteHotFixAllConfigInfo()
    {
        if (!Directory.Exists(AllConfigInfoDir))
            Directory.CreateDirectory(AllConfigInfoDir);
        if (!Directory.Exists(HotFixscriptDir))
        {
            Debug.LogError("文件夹" + HotFixscriptDir + "不存在，请检查路径是否正确");
            return;
        }
        DirectoryInfo direction = new DirectoryInfo(HotFixscriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        StringBuilder sbPath = new StringBuilder();
        sbPath.AppendLine("using UnityEngine;");
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");
        sbPath.AppendLine("public class " + AllHotConfigName + ": ScriptableObject");
        sbPath.AppendLine("{");
        string scriptFilePath = AllConfigInfoDir + AllHotConfigName + ".cs";
        for (int x = 0; x < files.Length; x++)
        {
            if (files[x].Name.EndsWith(".meta")) continue;
            string prefabName = files[x].Name.Split('.')[0];
            sbPath.AppendLine("    public " + prefabName + "[] " + prefabName + ";");
        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.Log("把HotFixGameData下的配置文件写入HotFixAllConfigInfo脚本");
    }
    /// <summary>
    /// 自动生成读取方法
    /// </summary>
    public static void WriteReadExcelInfo()
    {
        if (!Directory.Exists(AllConfigInfoDir))
            Directory.CreateDirectory(AllConfigInfoDir);
        if (!Directory.Exists(scriptDir))
            Directory.CreateDirectory(scriptDir);
        StringBuilder sbPath = new StringBuilder();
        DirectoryInfo direction = new DirectoryInfo(scriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        string scriptFilePath = ReadExcelInfoDir + ReadExcelName + ".cs";

        sbPath.AppendLine("using UnityEngine;");
        sbPath.AppendLine("using LitJson;");
        sbPath.AppendLine("using System.Collections.Generic;");
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");       
        sbPath.AppendLine("public class " + ReadExcelName);
        sbPath.AppendLine("{");
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string prefabName = files[i].Name.Split('.')[0];           
            sbPath.AppendLine("     public static void " + prefabName + "()");
            sbPath.AppendLine("     { ");
            sbPath.AppendLine("         Debug.Log("+'"'+"读取表格:"+prefabName +'"' +"); ");          
            sbPath.AppendLine("         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData(" + '"' + prefabName + '"' + ", 0);");
            sbPath.AppendLine("         CreateConfigData.configInfo." + prefabName + "= new " + prefabName + "[table.Count];");
            sbPath.AppendLine("         for (int i = 0; i < table.Count; i++)");
            sbPath.AppendLine("         { ");
            sbPath.AppendLine("             CreateConfigData.configInfo." + prefabName + "[i] = new " + prefabName + "(); ");
            Type tempClass = Assembly.Load("Assembly-CSharp").GetType(prefabName);
            FieldInfo[] fields = tempClass.GetFields();
            for (int x = 0; x < fields.Length; x++)
            {
                string type = CreateExcel.GetDataBaseType(fields[x].FieldType.ToString());
                if (type == "error") return;
                sbPath.AppendLine("             CreateConfigData.configInfo." + prefabName + "[i]." + fields[x].Name + "= (" + type + ") table[i][" + '"' + fields[x].Name + '"' + "];");
            }
            sbPath.AppendLine("         } ");
            sbPath.AppendLine("     } ");
        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.Log("自动生成读Excel的方法脚本");
    }
    public static void WriteHotFixReadExcelInfo()
    {
        if (!Directory.Exists(AllConfigInfoDir))
            Directory.CreateDirectory(AllConfigInfoDir);
        if (!Directory.Exists(HotFixscriptDir))
            Directory.CreateDirectory(HotFixscriptDir);
        StringBuilder sbPath = new StringBuilder();
        DirectoryInfo direction = new DirectoryInfo(HotFixscriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        string scriptFilePath = ReadExcelInfoDir + HotFixReadExcelName + ".cs";

        sbPath.AppendLine("using UnityEngine;");
        sbPath.AppendLine("using LitJson;");
        sbPath.AppendLine("using System.Collections.Generic;");
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");
        sbPath.AppendLine("public class " + HotFixReadExcelName);
        sbPath.AppendLine("{");
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string prefabName = files[i].Name.Split('.')[0];
            sbPath.AppendLine("     public static void " + prefabName + "()");
            sbPath.AppendLine("     { ");
            sbPath.AppendLine("         Debug.Log(" + '"' + "读取表格:" + prefabName + '"' + "); ");
            sbPath.AppendLine("         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData(" + '"'+prefabName+'"'+", 0);");
            sbPath.AppendLine("         CreateConfigData.hotConfigInfo." + prefabName + "= new " + prefabName + "[table.Count];");
            sbPath.AppendLine("         for (int i = 0; i < table.Count; i++)");
            sbPath.AppendLine("         { ");
            sbPath.AppendLine("             CreateConfigData.hotConfigInfo." + prefabName + "[i] = new " + prefabName + "(); ");       
            Type tempClass = Assembly.Load("Assembly-CSharp").GetType(prefabName);
            FieldInfo[] fields = tempClass.GetFields();   
            for (int x = 0; x < fields.Length; x++)
            {
                string type = CreateExcel.GetDataBaseType(fields[x].FieldType.ToString());
                if (type == "error") return;
                sbPath.AppendLine("             CreateConfigData.hotConfigInfo." + prefabName + "[i]." + fields[x].Name+"= ("+ type + ") table[i]["+'"'+ fields[x].Name + '"'+"];");
            }
            sbPath.AppendLine("         } ");
            sbPath.AppendLine("     } ");

        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.Log("自动生成读HotFixExcel的方法脚本");
    }

    /// <summary>
    /// 数据初始化脚本
    /// </summary>
    public static void WriteDataInitInfo()
    {
        StringBuilder sbPath = new StringBuilder();
        DirectoryInfo direction = new DirectoryInfo(HotFixscriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        string scriptFilePath = DataInitInfoDir + DataInitInfo + ".cs";

        sbPath.AppendLine("using UnityEngine;");
        sbPath.AppendLine("using System.Collections;");
        sbPath.AppendLine("using System.Collections.Generic;");
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");
        sbPath.AppendLine("public class " + DataInitInfo + " :MonoSingleton<" + DataInitInfo + ">");
        sbPath.AppendLine("{");
        sbPath.AppendLine("         private  AllConfigInfo AllConfig; ");
        sbPath.AppendLine("         private  AllHotConfigInfo AllHotConfig; ");
        sbPath.AppendLine("         public void InitAllConfig() ");
        sbPath.AppendLine("         {");
        sbPath.AppendLine("             AllConfig = Resources.Load<AllConfigInfo>(" +'"'+ AllConfigName +'"'+ ");");
        sbPath.AppendLine("             Deserialize(AllConfig);");
        sbPath.AppendLine("             Resources.UnloadUnusedAssets();");
        sbPath.AppendLine("         }");
        sbPath.AppendLine("         ");
        sbPath.AppendLine("         public void InitAllHotConfig() ");
        sbPath.AppendLine("         {");
        sbPath.AppendLine("             AllHotConfig = ABMgr.Instance.LoadConfigInfo("+ '"'+ "GameData/"+ AllHotConfigName + '"' +");");
        sbPath.AppendLine("             Deserialize(AllHotConfig);");
        sbPath.AppendLine("         }");
        sbPath.AppendLine("         public static void Deserialize(AllConfigInfo set)");
        sbPath.AppendLine("         {");
        Type tempClass = Assembly.Load("Assembly-CSharp").GetType(AllConfigName);
        FieldInfo[] fields = tempClass.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            sbPath.AppendLine("             for (int i = 0; i < set."+ fields[i].Name + ".Length; i++)");
            sbPath.AppendLine("             {");
            sbPath.AppendLine("                  " + fields[i].FieldType.ToString().Replace("[]", "") + ".GetDictionary().Add(set." + fields[i].Name + "[i].Id, set."+ fields[i].Name + "[i]);");
            sbPath.AppendLine("             }");
        }    
        sbPath.AppendLine("         }");
        sbPath.AppendLine("         public static void Deserialize(AllHotConfigInfo set)");
        sbPath.AppendLine("         {");
        Type temp1Class = Assembly.Load("Assembly-CSharp").GetType(AllHotConfigName);
        FieldInfo[] fields1 = temp1Class.GetFields();
        for (int i = 0; i < fields1.Length; i++)
        {
            sbPath.AppendLine("             for (int i = 0; i < set." + fields1[i].Name + ".Length; i++)");
            sbPath.AppendLine("             {");
            sbPath.AppendLine("                  " + fields1[i].FieldType.ToString().Replace("[]", "") + ".GetDictionary().Add(set." + fields1[i].Name + "[i].Id, set." + fields1[i].Name + "[i]);");
            sbPath.AppendLine("             }");
        }
        sbPath.AppendLine("         }");
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.Log("自动生成数据初始化脚本");
    }

    /// <summary>
    /// 自动读取自动生成读取方法
    /// </summary>
    public static void AutoReadExcelInfo()
    {
        Type t = Type.GetType(ReadExcelName);      

        MethodInfo[] mt = t.GetMethods(BindingFlags.Public | BindingFlags.Static);
        if (mt != null)
        {
            for (int i = 0; i < mt.Length; i++)
            {
                mt[i].Invoke(null, null);
            }
        }
    }
    public static void AutoReadHotFixExcelInfo()
    {
        Type t = Type.GetType(HotFixReadExcelName);
        MethodInfo[] mt = t.GetMethods(BindingFlags.Public | BindingFlags.Static);
        if (mt != null)
        {
            for (int i = 0; i < mt.Length; i++)
            {
                mt[i].Invoke(null, null);
            }
        }
    }
    /// <summary>
    /// 测试读表0
    /// </summary>
    public static void ReadTestExcel0()
    {
        Debug.LogError("读表");
        //string json = ReadExcel2Json("TestDicExcel", 0);
        //JsonData jd = JsonMapper.ToObject(json);
        //hotConfigInfo.TestDicExcel = new TestDicExcel[jd.Count];
        //for (int i = 0; i < jd.Count; i++)
        //{
        //    hotConfigInfo.TestDicExcel[i] = new TestDicExcel();
        //    Debug.LogError("json:"+jd[i].ToJson());
        //    hotConfigInfo.TestDicExcel[i] = JsonMapper.ToObject<TestDicExcel>(jd[i].ToJson());
        //}
        //List<Dictionary<string, object>> table = ReadExcelData("TestDicExcel", 0);
        //hotConfigInfo.TestDicExcel = new TestDicExcel[table.Count];
        //for (int i = 0; i < table.Count; i++)
        //{
        //    hotConfigInfo.TestDicExcel[i] = new TestDicExcel();
        //    Debug.LogError("id" + table[i]["Id"]);
        //    hotConfigInfo.TestDicExcel[i].Id = (int)table[i]["Id"];
        //    hotConfigInfo.TestDicExcel[i].level = (string)table[i]["level"];
        //    hotConfigInfo.TestDicExcel[i].testDic2 = (int[])table[i]["testDic2"];
        //    hotConfigInfo.TestDicExcel[i].testDic3 = (string[])table[i]["testDic3"];
        //    hotConfigInfo.TestDicExcel[i].testDic4 = (float)table[i]["testDic4"];
        //    hotConfigInfo.TestDicExcel[i].testDic5 = (bool)table[i]["testDic5"];
        //}

    }
    /// <summary>
    /// 表格数据存入List<Dictionary<string, object>>
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="tablesID"></param>
    /// <returns></returns>
    public static List<Dictionary<string, object>> ReadExcelData(string filename, int tablesID = 0)
    {
        DataTable mTables = GetExcelData(filename, tablesID);
        //列
        int columns = mTables.Columns.Count;   
        //行
        int rows = mTables.Rows.Count;
        List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
        bool flag = false;
        for (int i = startRow - 1; i < rows; i++)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            for (int j = 0; j < columns; j++)
            {
                string field = mTables.Rows[startRow - 2][j].ToString();
                if (field == "") continue;
                string type = mTables.Rows[startRow - 3][j].ToString();
                string value = mTables.Rows[i][j].ToString().Trim();
                //如果某行有任意元素的话标记一下计入数据表,整行没有元素的话该行不计入数据表
                if (!flag && !string.IsNullOrEmpty(value))
                {
                    flag = true;
                }
                row[field] = GetRealData(type, value);
            }
            if (flag)
            {
                flag = false;
                table.Add(row);
            }
        }        
        return table;
    }
    /// <summary>
    /// 获取表格
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="tableID"></param>
    /// <returns></returns>
    public static DataTable GetExcelData(string filename, int tableID = 0)
    {
        string currentPath = Application.dataPath;
        DirectoryInfo di = new DirectoryInfo(currentPath);
        _path = di.Parent.ToString() + "/ConfigExcels/";
        FileStream fs = File.Open(_path + filename + ".xlsx", FileMode.Open, FileAccess.Read);
        IExcelDataReader er = ExcelReaderFactory.CreateOpenXmlReader(fs);
        DataSet ds = er.AsDataSet();
        fs.Close();
        if (ds.Tables.Count < 1)
        {
            return null;
        }
        return ds.Tables[tableID];
    }
    //数据转成对应的类型
    //list<int[]>这种类型无法序列化,不放入可选类型
    //把数据转成object类型返回，装箱操作。
    public static object GetRealData(string tempType, string obj)
    {
        object result;
        int[] intAryData;
        string[] stringAryData;

        switch (tempType)
        {
            case "int":               
                result = obj==""? 0 : obj.ToInt32();
                break;
            case "string":
                result = obj;
                break;
            case "float":
                result = obj == "" ? 0f : Convert.ToSingle(obj);
                break;
            case "bool":
                if (obj == "")
                {
                    result = false;
                }
                else
                {
                    if (obj.ToLower() == "false")
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    };
                }
               
                break;
            case "int[]":
                if (obj == "")
                {
                    intAryData = new int[2];
                }
                else 
                {
                    intAryData = Array.ConvertAll(obj.Split(','), s => int.Parse(s));
                }
               
                result = intAryData;
                break;
            case "string[]":
                if (obj == "")
                {
                    stringAryData = new string[2];
                }
                else
                {
                    stringAryData = obj.Split(',');
                }     
                result = stringAryData;
                break;
            default:
                result = obj;
                break;
        }
        return result;
    }

}
