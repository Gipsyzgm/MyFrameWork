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

public class CreateConfigData : MonoBehaviour
{
    public static AllConfigInfo configData;
    public static HotFixAllConfigInfo HotFixconfigData;

    static string _path = "";
    /// <summary>
    /// 从第几行开始有数据
    /// </summary>
    const int startRow = 3;
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
    /// 不需要热更AllConfigInfo的类名
    /// </summary>
    static string AllConfigName = "AllConfigInfo";
    /// <summary>
    /// 需要热更的类名
    /// </summary>
    static string HotFixConfigName = "HotFixAllConfigInfo";
    /// <summary>
    /// 不需要热更读取读Excel的类名
    /// </summary>
    static string ReadExcelName = "AllNeedReadExcel";
    /// <summary>
    /// 需要热更读取读Excel的类名
    /// </summary>
    static string HotFixReadExcelName = "HotFixAllNeedReadExcel";

    [MenuItem("我的工具/其他/配置Excel表格/读取配置表格", false, 2)]
    public static void ReadConfigData()
    {
        WriteAllConfigInfo();
        WriteHotFixAllConfigInfo();
        WriteReadExcelInfo();
        WriteHotFixReadExcelInfo();
        AssetDatabase.DeleteAsset(assetDir+"ConfigAsset.asset");
        AssetDatabase.DeleteAsset(HotFixassetDir + "HotFixConfigAsset.asset");
        configData = ScriptableObject.CreateInstance<AllConfigInfo>();
        HotFixconfigData = ScriptableObject.CreateInstance<HotFixAllConfigInfo>();
        AutoReadExcelInfo();
        AutoReadHotFixExcelInfo();
        AssetDatabase.CreateAsset(configData, assetDir + "ConfigAsset.asset");
        AssetDatabase.CreateAsset(HotFixconfigData, HotFixassetDir + "HotFixConfigAsset.asset");
        Debug.LogError("读取配置完成,"+ assetDir+"ConfigAsset.asset");
        Debug.LogError("读取配置完成," + HotFixassetDir + "HotFixConfigAsset.asset");
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
        StringBuilder sbPath = new StringBuilder(); 
        sbPath.AppendLine("using UnityEngine;");
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");
        sbPath.AppendLine("public class " + AllConfigName+ ": ScriptableObject");
        sbPath.AppendLine("{");
        DirectoryInfo direction = new DirectoryInfo(scriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        string scriptFilePath = AllConfigInfoDir + AllConfigName + ".cs";
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string prefabName = files[i].Name.Split('.')[0];
            sbPath.AppendLine("    public " + prefabName + "[] "+ prefabName+";");
        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.LogError("把GameData下的配置文件写入AllConfigInfo脚本");       
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
        StringBuilder sbPath = new StringBuilder();
        sbPath.AppendLine("using UnityEngine;");
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");
        sbPath.AppendLine("public class " + HotFixConfigName + ": ScriptableObject");
        sbPath.AppendLine("{");
        DirectoryInfo direction = new DirectoryInfo(HotFixscriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        string scriptFilePath = AllConfigInfoDir + HotFixConfigName + ".cs";
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string prefabName = files[i].Name.Split('.')[0];
            sbPath.AppendLine("    public " + prefabName + "[] " + prefabName + ";");
        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.LogError("先把HotFixGameData下的配置文件写入HotFixAllConfigInfo脚本");
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
            bool IsDicExcel = false;
            Type tempClass = Assembly.Load("Assembly-CSharp").GetType(prefabName);
            FieldInfo[] fields = tempClass.GetFields();
            foreach (var item in fields)
            {
                if (item.FieldType == typeof(List<String>))
                {
                    IsDicExcel = true;
                }
            }
           
            sbPath.AppendLine("     public static void " + prefabName + "()");
            sbPath.AppendLine("     { ");
            sbPath.AppendLine("         Debug.LogError("+'"'+"读取表格:"+prefabName +'"' +"); ");
            if (IsDicExcel)
            {               
                sbPath.AppendLine("         Dictionary<string, List<string>> dic = CreateConfigData.ReadDictionaryFromExcel(" + '"' + prefabName + '"' + ", 0);");
                sbPath.AppendLine("         CreateConfigData.configData." + prefabName + "= new " + prefabName + "[dic.Count];");
                sbPath.AppendLine("         int Count = 0;");
                sbPath.AppendLine("         foreach (var item in dic.Keys)");
                sbPath.AppendLine("         { ");
                sbPath.AppendLine("             CreateConfigData.configData." + prefabName + "[Count] = new " + prefabName + "(); ");
                sbPath.AppendLine("             CreateConfigData.configData." + prefabName + "[Count]." + fields[0].Name + " = item; ");
                sbPath.AppendLine("             CreateConfigData.configData." + prefabName + "[Count]." + fields[1].Name + " = dic[item];");
                sbPath.AppendLine("             Count ++;");
                sbPath.AppendLine("         } ");           
            }
            else
            {
                sbPath.AppendLine("         string json = CreateConfigData.ReadExcel2Json(" + '"' + prefabName + '"' + ", 0);");
                sbPath.AppendLine("         JsonData jd = JsonMapper.ToObject(json);");
                sbPath.AppendLine("         CreateConfigData.configData." + prefabName + " = new " + prefabName + "[jd.Count];");
                sbPath.AppendLine("         for (int i = 0; i < jd.Count; i++) ");
                sbPath.AppendLine("         {  ");
                sbPath.AppendLine("             CreateConfigData.configData." + prefabName + "[i]"+" = JsonMapper.ToObject<" + prefabName + ">(jd[i].ToJson());");
                sbPath.AppendLine("         }  ");

            }
            sbPath.AppendLine("     } ");

        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.LogError("自动生成读Excel的方法脚本");
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
            bool IsDicExcel = false;
            Type tempClass = Assembly.Load("Assembly-CSharp").GetType(prefabName);
            FieldInfo[] fields = tempClass.GetFields();
            foreach (var item in fields)
            {
                if (item.FieldType == typeof(List<String>))
                {
                    IsDicExcel = true;
                }
            }

            sbPath.AppendLine("     public static void " + prefabName + "()");
            sbPath.AppendLine("     { ");
            sbPath.AppendLine("         Debug.LogError(" + '"' + "读取表格:" + prefabName + '"' + "); ");
            if (IsDicExcel)
            {
                sbPath.AppendLine("         Dictionary<string, List<string>> dic = CreateConfigData.ReadDictionaryFromExcel(" + '"' + prefabName + '"' + ", 0);");
                sbPath.AppendLine("         CreateConfigData.HotFixconfigData." + prefabName + "= new " + prefabName + "[dic.Count];");
                sbPath.AppendLine("         int Count = 0;");
                sbPath.AppendLine("         foreach (var item in dic.Keys)");
                sbPath.AppendLine("         { ");
                sbPath.AppendLine("             CreateConfigData.HotFixconfigData." + prefabName + "[Count] = new " + prefabName + "(); ");
                sbPath.AppendLine("             CreateConfigData.HotFixconfigData." + prefabName + "[Count]." + fields[0].Name + " = item; ");
                sbPath.AppendLine("             CreateConfigData.HotFixconfigData." + prefabName + "[Count]." + fields[1].Name + " = dic[item];");
                sbPath.AppendLine("             Count ++;");
                sbPath.AppendLine("         } ");
            }
            else
            {
                sbPath.AppendLine("         string json = CreateConfigData.ReadExcel2Json(" + '"' + prefabName + '"' + ", 0);");
                sbPath.AppendLine("         JsonData jd = JsonMapper.ToObject(json);");
                sbPath.AppendLine("         CreateConfigData.HotFixconfigData." + prefabName + " = new " + prefabName + "[jd.Count];");
                sbPath.AppendLine("         for (int i = 0; i < jd.Count; i++) ");
                sbPath.AppendLine("         {  ");
                sbPath.AppendLine("             CreateConfigData.HotFixconfigData." + prefabName + "[i]" + " = JsonMapper.ToObject<" + prefabName + ">(jd[i].ToJson());");
                sbPath.AppendLine("         }  ");

            }
            sbPath.AppendLine("     } ");

        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
        Debug.LogError("自动生成读HotFixExcel的方法脚本");
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
    /// 测试读表读成字典格式
    /// </summary>
    //public static void ReadTestDicExcel()
    //{
    //    Dictionary<string, List<string>> dic = ReadDictionaryFromExcel("测试字典配置表", 0);
    //    configData.TestDicExcel = new TestDicExcel[dic.Count];
    //    Debug.LogError(dic.Count);
    //    int Count = 0;
    //    foreach (var item in dic.Keys)
    //    {
    //        Count++;
    //        //因为item是Index为1，在程序里index开始是0；所以减1
    //        configData.TestDicExcel[Count] = new TestDicExcel();
    //        configData.TestDicExcel[Count].level = item;
    //        configData.TestDicExcel[Count].testDic = dic[item];
    //    }
    //}
    /// <summary>
    /// 测试读表0
    /// </summary>
    //static void ReadTestExcel0()
    //{
    //    string json = ReadExcel2Json("测试配置表", 0);
    //    JsonData jd = JsonMapper.ToObject(json);
    //    configData.TestExcel0 = new TestExcel0[jd.Count];
    //    for (int i = 0; i < jd.Count; i++)
    //    {
    //        configData.TestExcel0[i] = JsonMapper.ToObject<TestExcel0>(jd[i].ToJson());
    //    }
    //}
    /// <summary>
    /// 测试读表1
    /// </summary>
    //static void ReadTestExcel1()
    //{
    //    string json = ReadExcel2Json("测试配置表", 1);
    //    JsonData jd = JsonMapper.ToObject(json);
    //    configData.TestExcel1 = new TestExcel1[jd.Count];
    //    for (int i = 0; i < jd.Count; i++)
    //    {
    //        configData.TestExcel1[i] = JsonMapper.ToObject<TestExcel1>(jd[i].ToJson());
    //    }
    //}

    /// <summary>
    /// 参数1为表格名称，参数2为第N张表格Trim()是去两边空格的方法,excel转成json串
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="tablesID"></param>
    /// <returns></returns>
    public static string ReadExcel2Json(string filename, int tablesID = 0)
    {

        DataTable mTables = getExcelData(filename, tablesID);
        //列
        int columns = mTables.Columns.Count;
        //行
        int rows = mTables.Rows.Count;
        List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
        for (int i = startRow - 1; i < rows; i++)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            bool flag = false;
            for (int j = 0; j < columns; j++)
            {
                string field = mTables.Rows[startRow - 2][j].ToString();
                if (field == "") continue;
                string value = mTables.Rows[i][j].ToString().Trim();
                row[field] = value;
                if (!flag && !string.IsNullOrEmpty(value))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                continue;
            }
            table.Add(row);
        }
        string json = JsonMapper.ToJson(table);
        return json;
    }
    public static DataTable getExcelData(string filename, int tableID = 0)
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

    //excel 每行存为一个list,key为第一列的数据,value为每行之后的数据
    public static Dictionary<string, List<string>> ReadDictionaryFromExcel(string fileName, int tablesID)
    {
        Dictionary<string, List<string>> _dic = new Dictionary<string, List<string>>();
        DataTable mTables = getExcelData(fileName, tablesID);
        int columns = mTables.Columns.Count;
        int rows = mTables.Rows.Count;
        for (int i = 2; i < rows; i++)
        {
            if (mTables.Rows[i][0].ToString() == "")
                break;
            List<string> _list = new List<string>();
            for (int j = 1; j < columns; j++)
            {
                string value = mTables.Rows[i][j].ToString();
               
                if (!string.IsNullOrEmpty(value))
                {
                    _list.Add(value);
                }
            }
            _dic.Add(mTables.Rows[i][0].ToString(), _list);
        }
        return _dic;
    }

}

