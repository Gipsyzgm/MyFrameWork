using UnityEngine;
using LitJson;
using System.Collections.Generic;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class AllReadExcel
{
     public static void TestDicExcel()
     { 
         Debug.LogError("读取表格:TestDicExcel"); 
         Dictionary<string, List<string>> dic = CreateConfigData.ReadDictionaryFromExcel("TestDicExcel", 0);
         CreateConfigData.configData.TestDicExcel= new TestDicExcel[dic.Count];
         int Count = 0;
         foreach (var item in dic.Keys)
         { 
             CreateConfigData.configData.TestDicExcel[Count] = new TestDicExcel(); 
             CreateConfigData.configData.TestDicExcel[Count].level = item; 
             CreateConfigData.configData.TestDicExcel[Count].testDic = dic[item];
             Count ++;
         } 
     } 
     public static void TestExcel0()
     { 
         Debug.LogError("读取表格:TestExcel0"); 
         string json = CreateConfigData.ReadExcel2Json("TestExcel0", 0);
         JsonData jd = JsonMapper.ToObject(json);
         CreateConfigData.configData.TestExcel0 = new TestExcel0[jd.Count];
         for (int i = 0; i < jd.Count; i++) 
         {  
             CreateConfigData.configData.TestExcel0[i] = JsonMapper.ToObject<TestExcel0>(jd[i].ToJson());
         }  
     } 
     public static void TestExcel1()
     { 
         Debug.LogError("读取表格:TestExcel1"); 
         string json = CreateConfigData.ReadExcel2Json("TestExcel1", 0);
         JsonData jd = JsonMapper.ToObject(json);
         CreateConfigData.configData.TestExcel1 = new TestExcel1[jd.Count];
         for (int i = 0; i < jd.Count; i++) 
         {  
             CreateConfigData.configData.TestExcel1[i] = JsonMapper.ToObject<TestExcel1>(jd[i].ToJson());
         }  
     } 
     public static void TestLanguage()
     { 
         Debug.LogError("读取表格:TestLanguage"); 
         string json = CreateConfigData.ReadExcel2Json("TestLanguage", 0);
         JsonData jd = JsonMapper.ToObject(json);
         CreateConfigData.configData.TestLanguage = new TestLanguage[jd.Count];
         for (int i = 0; i < jd.Count; i++) 
         {  
             CreateConfigData.configData.TestLanguage[i] = JsonMapper.ToObject<TestLanguage>(jd[i].ToJson());
         }  
     } 
}
