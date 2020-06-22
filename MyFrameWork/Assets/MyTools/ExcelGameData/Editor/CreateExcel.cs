/*
 *  项目名字：MyFrameWork
 *  创建时间：2020.1.21
 *  描述信息：Excel数据处理。
 *  使用说明：
 *  1：写自己需要转换Excel表格的C#代码。可以参考Assets/MyTools/ExcelGameData/GameData/下的代码。
 *  C#脚本也放在该路径下，该路径下只能放需要转换Excel表格
 *  2：我的工具-配置Excel表格-生成默认表格。下面ExcelFileDir即为生成的表格路径。
 *  3：表格填数据。
 *  4：我的工具-配置Excel表格-读取配置表格。
 *  5：MyGameData.InitGameData();调用初始化代码。
 *  6：通过MyGameData.config.可以点出所有的数据。
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
    /// <summary>
    /// 1对多数据 最大数据数
    /// </summary>
    static int maxList = 10;

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
            bool IsDicExcel = false;
            foreach (var item in fieldInfos)
            {
                if (item.FieldType==typeof(List<string>))
                {
                    IsDicExcel = true;
                }
            }
            if (IsDicExcel)
            {
                if (fieldInfos.Length!=2)
                {
                    Debug.LogError("表格不合要求，请核对格式");
                }
                for (int i = 0; i < maxList+1; i++)
                {
                    if (i == 0)
                    {
                        worksheet.Cells[1, i + 1].Value = "Key";
                        worksheet.Cells[2, i + 1].Value = fieldInfos[i].Name;
                    }
                    else if (i == 1)
                    {
                        worksheet.Cells[1, i + 1].Value = "Value";
                        worksheet.Cells[2, i + 1].Value = "List[" + (i - 1) + "]";
                    }
                    else
                    {
                        worksheet.Cells[1, i + 1].Value = "";
                        worksheet.Cells[2, i + 1].Value = "List[" + (i - 1) + "]";
                    }     
                }
            }
            else
            {
                for (int i = 0; i < fieldInfos.Length; i++)
                {

                    worksheet.Cells[1, i + 1].Value = "描述（可替换）";
                    worksheet.Cells[2, i + 1].Value = fieldInfos[i].Name;
                }
            }          
            //save our new workbook and we are done!        
            package.Save();
        }
    }

}



