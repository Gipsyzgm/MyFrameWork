using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System.Data;
using System.IO;
using Excel;
using System.Text;
using System.Reflection;
using System;

public class CreateConfigData  {

    static string _path = "";
    static AllConfigInfo configData;
    /// <summary>
    /// 写入资源的生成资源的路径,必须在Resources路径下
    /// </summary>
    static string assetDir = "Assets/MyTools/ExcelGameData/Resources/";
    /// <summary>
    /// 需要写入AllConfigInfo的文件夹地址
    /// </summary>
    static string scriptDir = "Assets/MyTools/ExcelGameData/GameData/";
    /// <summary>
    /// AllConfigInfo的文件夹存放地址
    /// </summary>
    static string AllConfigInfoDir = "Assets/MyTools/ExcelGameData/AllConfigInfo/";
    /// <summary>
    /// 从第几行开始有数据
    /// </summary>
    const int startRow = 3;

    [MenuItem("我的工具/配置Excel表格/读取配置表格", false, 2)]
    public static void ReadConfigData()
    {
        WriteAllConfigInfo();
        AssetDatabase.DeleteAsset(assetDir+"ConfigAsset.asset");
        configData = ScriptableObject.CreateInstance<AllConfigInfo>();
        
   
        ReadTestExcel0();
        ReadTestExcel1();
        ReadTestDicExcel();
        ReadExcel();

        AssetDatabase.CreateAsset(configData, assetDir + "ConfigAsset.asset");
        Debug.LogError("读取配置完成,"+ assetDir+"ConfigAsset.asset");     
    }
    /// <summary>
    /// 自动写AllConfigInfo脚本
    /// </summary>
    public static void WriteAllConfigInfo()
    {
        if (!Directory.Exists(AllConfigInfoDir))
            Directory.CreateDirectory(AllConfigInfoDir);
        if (!Directory.Exists(scriptDir))
            Directory.CreateDirectory(scriptDir);
        StringBuilder sbPath = new StringBuilder();
        string csName = "AllConfigInfo";
        sbPath.AppendLine("using UnityEngine;");
        sbPath.AppendLine("//每次都会重新生成的脚本，不要删，覆盖就行了");
        sbPath.AppendLine("public class " + csName+ ": ScriptableObject");
        sbPath.AppendLine("{");
        DirectoryInfo direction = new DirectoryInfo(scriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        string scriptFilePath = AllConfigInfoDir + csName + ".cs";
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string prefabName = files[i].Name.Split('.')[0];
            sbPath.AppendLine("    public " + prefabName + "[] "+ prefabName+";");
        }
        sbPath.AppendLine("}");
        File.WriteAllText(scriptFilePath, sbPath.ToString(), Encoding.UTF8);
        Debug.LogError("先把GameData下的配置文件写入AllConfigInfo脚本");
    }


    static void ReadExcel()
    {
        DirectoryInfo direction = new DirectoryInfo(scriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);    
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string prefabName = files[i].Name.Split('.')[0];
            Debug.LogError("prefabName"+prefabName);
            bool IsDicExcel = false;
            Type tempClass = Assembly.Load("Assembly-CSharp").GetType(prefabName);
            FieldInfo[] fields = tempClass.GetFields();
            foreach (var item in fields)
            {
                if (item.FieldType == typeof(List<String>))
                {
                    Debug.LogError(item.FieldType + ":" + item.Name);
                    IsDicExcel = true;
                }             
            }
            if (IsDicExcel)
            {
                Dictionary<string, List<string>> dic = ReadDictionaryFromExcel(prefabName, 0);
                //需要通过类名获得属性，  需要通过string实例化

                //configData.GetType().GetField(prefabName) = new TestDicExcel[dic.Count];
                configData.TestDicExcel = new TestDicExcel[dic.Count];
                Debug.LogError(dic.Count);
                foreach (var item in dic.Keys)
                {
                    //因为item是Index为1，在程序里index开始是0；所以减1
                    configData.TestDicExcel[int.Parse(item) - 1] = new TestDicExcel();
                    configData.TestDicExcel[int.Parse(item) - 1].level = item;
                    configData.TestDicExcel[int.Parse(item) - 1].testDic = dic[item];
                }
            }
            else
            {

            }

        }



    }


   
    /// <summary>
    /// 测试读表读成字典格式
    /// </summary>
    static void ReadTestDicExcel()
    {
        Dictionary<string, List<string>> dic = ReadDictionaryFromExcel("测试字典配置表", 0);
        configData.TestDicExcel = new TestDicExcel[dic.Count];
        Debug.LogError(dic.Count);
        foreach (var item in dic.Keys)
        {
            //因为item是Index为1，在程序里index开始是0；所以减1
            configData.TestDicExcel[int.Parse(item) - 1] = new TestDicExcel();
            configData.TestDicExcel[int.Parse(item) - 1].level = item;
            configData.TestDicExcel[int.Parse(item) - 1].testDic = dic[item];
        }
    }
    /// <summary>
    /// 测试读表0
    /// </summary>
    static void ReadTestExcel0()
    {
        string json = ReadExcel2Json("测试配置表", 0);
        JsonData jd = JsonMapper.ToObject(json);
        configData.TestExcel0 = new TestExcel0[jd.Count];
        for (int i = 0; i < jd.Count; i++)
        {
            configData.TestExcel0[i] = JsonMapper.ToObject<TestExcel0>(jd[i].ToJson());
        }
    }
    /// <summary>
    /// 测试读表1
    /// </summary>
    static void ReadTestExcel1()
    {
        string json = ReadExcel2Json("测试配置表", 1);
        JsonData jd = JsonMapper.ToObject(json);
        configData.TestExcel1 = new TestExcel1[jd.Count];
        for (int i = 0; i < jd.Count; i++)
        {
            configData.TestExcel1[i] = JsonMapper.ToObject<TestExcel1>(jd[i].ToJson());
        }
    }
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
        for (int i = startRow-1 ; i < rows; i++)
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
            //把第一行第一列存为key,后面的存成list
            _dic.Add(mTables.Rows[i][0].ToString(), _list);
        }
        return _dic;
    }
}

