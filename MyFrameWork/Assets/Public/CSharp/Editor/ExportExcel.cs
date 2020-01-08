using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using UnityEditor;
using UnityEngine;

/*
 *使用方法:Excel文件夹放在Assets同级目录
 *         Excel第一行填写注释,第二行填类型如int[],int[,]等,后面填数据
 *         打包时File/BuildSetting/PlayerSettings/ApiCompatibilityLevel设置成.NET2.0
 */
public class ExportExcel {

    //Excel工作目录
    static string excelFile = Application.dataPath.Substring(0, Application.dataPath.Length - 7) + "/Excel/";
    //输出目录
    static string csFile = Application.dataPath + "/Scripts/CSharp/Data/";

    [MenuItem("EasyCode/ExportExcel")]
    public static void ExcelToData()
    {
        if (!Directory.Exists(excelFile))
            Directory.CreateDirectory(excelFile);
        if (!Directory.Exists(csFile))
            Directory.CreateDirectory(csFile);

        DirectoryInfo direction = new DirectoryInfo(excelFile);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta")) continue;
            if (!files[i].Name.EndsWith(".xls") && !files[i].Name.EndsWith(".xlsx")) continue;

            string excelName = files[i].Name.Split('.')[0];

            string[,] data = GetExcelData(files[i].FullName);
            ToCS(excelName, data);
            //ToLua(excelName, data);
            ToPB(excelName, data);
        }

        AssetDatabase.Refresh();
    }

    //得到Excel行列
    public static string[,] GetExcelData(string excelFile)
    {
        FileStream fileStream = new FileStream(excelFile, FileMode.Open, FileAccess.Read);
        IWorkbook workbook = null;
        if (excelFile.EndsWith(".xlsx"))//2007
        {
            workbook = new XSSFWorkbook(fileStream);
        }
        else if (excelFile.EndsWith(".xls"))//2003
        {
            workbook = new HSSFWorkbook(fileStream);
        }
        ISheet sheet = workbook.GetSheetAt(0);
        string[,] cells = new string[sheet.LastRowNum + 1,sheet.GetRow(0).LastCellNum];

        for(int i=0;i<=sheet.LastRowNum;i++)
        {
            IRow row = sheet.GetRow(i);
            for (int j = 0; j < row.LastCellNum; j++)
            {
                cells[i, j] = row.Cells[j].ToString();
            }
        }

        return cells;
    }

    /// <summary>
    /// 纯C#
    /// </summary>
    static void ToCS(string sheetName, string[,] data)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public class " + sheetName);
        sb.AppendLine("{");

        for (int i=0;i< data.GetLength(1);i++)//列
        {
            //注释
            sb.AppendLine("    //" + data[0, i]);
            //类型、名称
            sb.Append("    public static " + data[1,i] + " " + data[2, i] + " = {");
            for (int j=3;j< data.GetLength(0);j++)//行
            {
                sb.Append(TypeStringCSharp(data[1,i], data[j, i]) + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("};");
            sb.AppendLine();
        }
        sb.AppendLine();
        sb.AppendLine("}");

        File.WriteAllText(csFile + sheetName + ".cs", sb.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
    }
    static string TypeStringCSharp(string type, string str)
    {
        if (type.Equals("int[]"))
        {
            return str;
        }
        if (type.Equals("int[,]"))
        {
            return "{" + str + "}";
        }
        if (type.Equals("float[]"))
        {
            return str + "f";
        }
        if (type.Equals("float[,]"))
        {
            return "{"+ str.Replace(",","f,") + "f}";
        }
        if (type.Equals("string[]"))
        {
            return '"' + str + '"';
        }
        if(type.Equals("string[,]"))
        {
            return "{" + '"' + str.Replace(",", '"' + "," + '"') + '"' + "}";
        }
        if (type.Equals("bool[]"))
        {
            return str.ToLower();
        }
        if (type.Equals("bool[,]"))
        {
            return "{" + str.ToLower() + "}";
        }
        return str;
    }

    /// <summary>
    /// Lua
    /// </summary>
    static void ToLua(string sheetName, string[,] data)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("local " + sheetName + " = ");
        sb.AppendLine("{");

        for (int j = 3; j < data.GetLength(0); j++)//行
        {
            sb.AppendLine("    " + "[" + (j - 2) + "] = ");
            sb.AppendLine("    {");
            for (int i = 0; i < data.GetLength(1); i++)//列
            {
                sb.AppendLine("        --" + data[0, i]);
                sb.AppendLine("        " + data[2, i] + " = " + TypeStringLua(data[1, i], data[j, i]) + ", ");
            }
            sb.Remove(sb.Length - 2, 1);
            sb.AppendLine("    },");
        }
        sb.Remove(sb.Length - 1, 1);
        sb.AppendLine();
        sb.AppendLine("}");

        File.WriteAllText(csFile + sheetName + ".lua.txt", sb.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh();
    }
    static string TypeStringLua(string type, string str)
    {
        if (type.Equals("int[]"))
        {
            return str;
        }
        if (type.Equals("int[,]"))
        {
            return "{" + str + "}";
        }
        if (type.Equals("float[]"))
        {
            return str;
        }
        if (type.Equals("float[,]"))
        {
            return "{" + str + "}";
        }
        if (type.Equals("string[]"))
        {
            return '"' + str + '"';
        }
        if (type.Equals("string[,]"))
        {
            return "{" + '"' + str.Replace(",", '"' + "," + '"') + '"' + "}";
        }
        if (type.Equals("bool[]"))
        {
            return str.ToLower();
        }
        if (type.Equals("bool[,]"))
        {
            return "{" + str.ToLower() + "}";
        }
        return str;
    }
    /// <summary>
    /// protoc
    /// </summary>
    static void ToPB(string sheetName, string[,] data)
    {

    }
}
