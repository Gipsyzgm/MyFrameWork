using UnityEngine;
using LitJson;
using System.Collections.Generic;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class HotFixAllNeedReadExcel
{
     public static void TestDicExcel()
     { 
         Debug.Log("读取表格:TestDicExcel"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("TestDicExcel", 0);
         CreateConfigData.hotConfigInfo.TestDicExcel= new TestDicExcel[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.hotConfigInfo.TestDicExcel[i] = new TestDicExcel(); 
             CreateConfigData.hotConfigInfo.TestDicExcel[i].Id= (int) table[i]["Id"];
             CreateConfigData.hotConfigInfo.TestDicExcel[i].level= (string) table[i]["level"];
             CreateConfigData.hotConfigInfo.TestDicExcel[i].testDic2= (int[]) table[i]["testDic2"];
             CreateConfigData.hotConfigInfo.TestDicExcel[i].testDic3= (string[]) table[i]["testDic3"];
             CreateConfigData.hotConfigInfo.TestDicExcel[i].testDic4= (float) table[i]["testDic4"];
             CreateConfigData.hotConfigInfo.TestDicExcel[i].testDic5= (bool) table[i]["testDic5"];
         } 
     } 
     public static void TestExcel0()
     { 
         Debug.Log("读取表格:TestExcel0"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("TestExcel0", 0);
         CreateConfigData.hotConfigInfo.TestExcel0= new TestExcel0[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.hotConfigInfo.TestExcel0[i] = new TestExcel0(); 
             CreateConfigData.hotConfigInfo.TestExcel0[i].Id= (int) table[i]["Id"];
             CreateConfigData.hotConfigInfo.TestExcel0[i].GroupInfo= (string) table[i]["GroupInfo"];
         } 
     } 
     public static void TestExcel1()
     { 
         Debug.Log("读取表格:TestExcel1"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("TestExcel1", 0);
         CreateConfigData.hotConfigInfo.TestExcel1= new TestExcel1[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.hotConfigInfo.TestExcel1[i] = new TestExcel1(); 
             CreateConfigData.hotConfigInfo.TestExcel1[i].Id= (int) table[i]["Id"];
             CreateConfigData.hotConfigInfo.TestExcel1[i].Stage= (string) table[i]["Stage"];
             CreateConfigData.hotConfigInfo.TestExcel1[i].ItemNum= (string) table[i]["ItemNum"];
             CreateConfigData.hotConfigInfo.TestExcel1[i].LookArea= (string) table[i]["LookArea"];
             CreateConfigData.hotConfigInfo.TestExcel1[i].Min= (string) table[i]["Min"];
             CreateConfigData.hotConfigInfo.TestExcel1[i].Max= (string) table[i]["Max"];
         } 
     } 
     public static void TestLanguage()
     { 
         Debug.Log("读取表格:TestLanguage"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("TestLanguage", 0);
         CreateConfigData.hotConfigInfo.TestLanguage= new TestLanguage[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.hotConfigInfo.TestLanguage[i] = new TestLanguage(); 
             CreateConfigData.hotConfigInfo.TestLanguage[i].Id= (int) table[i]["Id"];
             CreateConfigData.hotConfigInfo.TestLanguage[i].cn= (string) table[i]["cn"];
             CreateConfigData.hotConfigInfo.TestLanguage[i].en= (string) table[i]["en"];
         } 
     } 
}
