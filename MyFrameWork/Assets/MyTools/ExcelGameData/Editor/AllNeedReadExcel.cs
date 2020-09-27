using UnityEngine;
using LitJson;
using System.Collections.Generic;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class AllNeedReadExcel
{
     public static void TextPanel1()
     { 
         Debug.Log("读取表格:TextPanel1"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("TextPanel1", 0);
         CreateConfigData.configInfo.TextPanel1= new TextPanel1[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.TextPanel1[i] = new TextPanel1(); 
             CreateConfigData.configInfo.TextPanel1[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.TextPanel1[i].cn= (string) table[i]["cn"];
             CreateConfigData.configInfo.TextPanel1[i].en= (string) table[i]["en"];
         } 
     } 
     public static void TextPanel2()
     { 
         Debug.Log("读取表格:TextPanel2"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("TextPanel2", 0);
         CreateConfigData.configInfo.TextPanel2= new TextPanel2[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.TextPanel2[i] = new TextPanel2(); 
             CreateConfigData.configInfo.TextPanel2[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.TextPanel2[i].cn= (string) table[i]["cn"];
             CreateConfigData.configInfo.TextPanel2[i].en= (string) table[i]["en"];
         } 
     } 
     public static void VerCheckLang()
     { 
         Debug.Log("读取表格:VerCheckLang"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("VerCheckLang", 0);
         CreateConfigData.configInfo.VerCheckLang= new VerCheckLang[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.VerCheckLang[i] = new VerCheckLang(); 
             CreateConfigData.configInfo.VerCheckLang[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.VerCheckLang[i].cn= (string) table[i]["cn"];
             CreateConfigData.configInfo.VerCheckLang[i].en= (string) table[i]["en"];
         } 
     } 
}
