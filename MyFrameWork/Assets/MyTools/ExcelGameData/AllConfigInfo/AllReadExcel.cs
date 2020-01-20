using UnityEngine;
using LitJson;
using System.Collections.Generic;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class AllReadExcel
{
     public static void TestDicExcel()
     { 
             Debug.LogError("TestDicExcel：执行了吗？"); 
         Dictionary<string, List<string>> dic = CreateConfigData.ReadDictionaryFromExcel("TestDicExcel", 0);
         CreateConfigData.configData.TestDicExcel= new TestDicExcel[dic.Count];
         foreach (var item in dic.Keys)
         { 
             CreateConfigData.configData.TestDicExcel[int.Parse(item) - 1] = new TestDicExcel(); 
             CreateConfigData.configData.TestDicExcel[int.Parse(item) - 1].level = item; 
             CreateConfigData.configData.TestDicExcel[int.Parse(item) - 1].testDic = dic[item];
         } 
     } 
     public static void TestExcel0()
     { 
             Debug.LogError("TestExcel0：执行了吗？"); 
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
             Debug.LogError("TestExcel1：执行了吗？"); 
         string json = CreateConfigData.ReadExcel2Json("TestExcel1", 0);
         JsonData jd = JsonMapper.ToObject(json);
         CreateConfigData.configData.TestExcel1 = new TestExcel1[jd.Count];
         for (int i = 0; i < jd.Count; i++) 
         {  
             CreateConfigData.configData.TestExcel1[i] = JsonMapper.ToObject<TestExcel1>(jd[i].ToJson());
         }  
     } 
}
