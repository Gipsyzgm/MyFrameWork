/*
 *  项目名字：MyFrameWork
 *  创建时间：2020.1.21
 *  描述信息：Excel数据处理。
 *  使用说明：
 *  1：写自己需要转换Excel表格的C#代码。可以参考Assets/MyTools/ExcelGameData/GameData/下的代码。
 *  需要热更的脚本放在Assets/MyTools/ExcelGameData/HotFixGameData/
 *  C#脚本也放在该路径下，该路径下只能放需要转换Excel表格
 *  2：我的工具-配置Excel表格-生成默认表格。下面ExcelFileDir即为生成的表格路径。
 *  3：表格填数据。
 *  4：我的工具-配置Excel表格-读取配置表格,生成对应的序列化数据。
 */
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Text.RegularExpressions;

public class CreateExcel : MonoBehaviour {
    
    /// <summary>
    /// 存放Excel的路径
    /// </summary>
    static string ExcelFileDir = Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/ConfigExcels/";
    /// <summary>
    /// 需要写成Excel的文件夹地址
    /// </summary>
    static string scriptDir = "Assets/MyTools/ExcelGameData/GameData/";
    /// <summary>
    /// 需要写成Excel的文件夹地址
    /// </summary>
    static string HotFixscriptDir = "Assets/MyTools/ExcelGameData/HotFixGameData/";
    /// <summary>
    /// 文件格式 
    /// 1：.xls
    /// 2：.xlsx
    /// </summary>
    static string FileFormat = ".xlsx";

    [MenuItem("我的工具/其他/配置Excel表格/生成默认表格", false, 1)]
    public static void CreateDefaultExcel()
    {
        if (!Directory.Exists(ExcelFileDir))
            Directory.CreateDirectory(ExcelFileDir);
        if (!Directory.Exists(scriptDir))
        {
            Debug.LogError("需要写入表格的文件路径不存在：" + scriptDir);
            return;
        }
        DirectoryInfo direction = new DirectoryInfo(scriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string className = files[i].Name.Split('.')[0];
            string excelDir = ExcelFileDir + className + FileFormat;    
            if (File.Exists(excelDir))
            {
                Debug.LogWarning(className + "表格已存在，跳过，如需替换，需手动删除。"+ excelDir);
                continue;
            }
            Type type = Assembly.Load("Assembly-CSharp").GetType(className);
            FieldInfo[] fields = type.GetFields();
            WriteExcel(excelDir, fields, className);
        }
        CreateHotFixDefaultExcel();
    }
    public static void CreateHotFixDefaultExcel()
    {
        if (!Directory.Exists(ExcelFileDir))
            Directory.CreateDirectory(ExcelFileDir);
        if (!Directory.Exists(HotFixscriptDir))
        {
            Debug.LogError("需要写入表格的文件路径不存在：" + HotFixscriptDir);
            return;
        }
        DirectoryInfo direction = new DirectoryInfo(HotFixscriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int x = 0; x < files.Length; x++)
        {
            if (files[x].Name.EndsWith(".meta")) continue;
            string className = files[x].Name.Split('.')[0];
            string excelDir = ExcelFileDir + className + FileFormat;   
            if (File.Exists(excelDir))
            {
                Debug.LogWarning(className + "表格已存在，跳过，如需替换，需手动删除。" + excelDir);
                continue;
            }
            Type type = Assembly.Load("Assembly-CSharp").GetType(className);
            FieldInfo[] fields = type.GetFields();
            WriteExcel(excelDir, fields, className);
        }
    }

    public static void WriteExcel(string outputDir, FieldInfo[] fieldInfos, string sheetName)
    {
        FileInfo newFile = new FileInfo(outputDir);
        if (!newFile.Exists)
        {
            newFile = new FileInfo(outputDir);
        }
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                if (i == 0)
                {
                    worksheet.Cells[1, i + 1].Value = "唯一ID,不可重复";

                }
                else
                {
                    worksheet.Cells[1, i + 1].Value = "描述（可替换）";

                }
               
                string type = GetDataBaseType(fieldInfos[i].FieldType.ToString(), sheetName);
                if (type == "error") return;
                worksheet.Cells[2, i + 1].Value = type;
                worksheet.Cells[3, i + 1].Value = fieldInfos[i].Name;
            }      
            package.Save();
            Debug.Log(sheetName + "表格生成成功");
        }
    }

    //现支持类型: int,string,float,bool,int[],string[]
    //读表时需要和这里的类型对应
    public static string GetDataBaseType(string TypeData,string DataName = null)
    {
        string baseType = "string";
        //包含多少个[]
        int Count = Regex.Matches(TypeData, @"\[").Count;
        //数组类型
        if (Count > 0)
        {
            //[]
            string[] tempAry = TypeData.Split('.');
            string tempType = tempAry[tempAry.Length - 1];
            switch (tempType)
            {
                case "Int32[]":
                    baseType = "int[]";
                    break;
                case "String[]":
                    baseType = "string[]";
                    break;
                default:
                    Debug.LogError(string.Format("{0}中数据类型{1}暂不支持,请检查!", DataName,TypeData));
                    baseType = "error";
                    break;
            }
        }
        else
        {
            //普通类型
            string[] tempAry = TypeData.Split('.');
            string tempType = tempAry[tempAry.Length - 1];
            switch (tempType)
            {
                case "Int32":
                    baseType = "int";
                    break;
                case "String":
                    baseType = "string";
                    break;
                case "Single":
                    baseType = "float";
                    break;
                case "Boolean":
                    baseType = "bool";
                    break;
                default:
                    Debug.LogError(string.Format("{0}中数据类型{1}暂不支持,请检查!", DataName, TypeData));
                    baseType = "error";
                    break;
            }
        }
        return baseType;

    }

}



