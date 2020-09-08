using UnityEngine;
using LitJson;
using System.Collections.Generic;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class AllNeedReadExcel
{
     public static void VerCheckLang()
     { 
         Debug.LogError("读取表格:VerCheckLang"); 
         string json = CreateConfigData.ReadExcel2Json("VerCheckLang", 0);
         JsonData jd = JsonMapper.ToObject(json);
         CreateConfigData.configData.VerCheckLang = new VerCheckLang[jd.Count];
         for (int i = 0; i < jd.Count; i++) 
         {  
             CreateConfigData.configData.VerCheckLang[i] = JsonMapper.ToObject<VerCheckLang>(jd[i].ToJson());
         }  
     } 
}
