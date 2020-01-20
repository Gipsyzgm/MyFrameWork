using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;

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
    /// 文件格式 
    /// 1：.xls
    /// 2：.xlsx
    /// </summary>
    static string FileFormat = ".xlsx";

    [MenuItem("我的工具/配置Excel表格/生成默认表格", false, 1)]
    public static void CreateDefaultExcel()
    {

        if (!Directory.Exists(ExcelFileDir))
            Directory.CreateDirectory(ExcelFileDir);
        if (!Directory.Exists(scriptDir))
            Directory.CreateDirectory(scriptDir);
        DirectoryInfo direction = new DirectoryInfo(scriptDir);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            string className = files[i].Name.Split('.')[0];
            string excelDir = ExcelFileDir + files[i].Name.Split('.')[0] + FileFormat;
            Type type = Assembly.Load("Assembly-CSharp").GetType(className);
            if (File.Exists(excelDir))
            {
                Debug.LogError(className + "表格已存在，跳过，如需替换，需手动删除。");
                continue;
            }
            FieldInfo[] fields = type.GetFields();
            WriteExcel(excelDir, fields);
            Debug.LogError(className + "表格生成成功");
        }
    }
    public static void WriteExcel(string outputDir, FieldInfo[] fieldInfos)
    {

       //string outputDir = EditorUtility.SaveFilePanel("Save Excel", "", "New Resource", "xlsx");
       FileInfo newFile = new FileInfo(outputDir);
        if (newFile.Exists)
        {
            newFile.Delete();  // ensures we create a new workbook
            newFile = new FileInfo(outputDir);
        }
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            // add a new worksheet to the empty workbook
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            //把每个属性都放进Excel表格    
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = "参数描述（可替换）";
                worksheet.Cells[2, i + 1].Value = fieldInfos[i].Name;
            }
            //save our new workbook and we are done!        
            package.Save();
        }
    }

    [MenuItem("我的工具/配置Excel表格/读取配置表格", false, 2)]
    public static void ReadConfigData()
    {
        CreateConfigData.ReadConfigData();
    }
}



