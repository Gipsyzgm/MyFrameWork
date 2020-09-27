using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class DataMgr :MonoSingleton<DataMgr>
{
         private  AllConfigInfo AllConfig; 
         private  AllHotConfigInfo AllHotConfig; 
         public void InitAllConfig() 
         {
             AllConfig = Resources.Load<AllConfigInfo>("AllConfigInfo");
             Deserialize(AllConfig);
             Resources.UnloadUnusedAssets();
         }
         
         public void InitAllHotConfig() 
         {
             AllHotConfig = ABMgr.Instance.LoadConfigInfo("GameData/AllHotConfigInfo");
             Deserialize(AllHotConfig);
         }
         public static void Deserialize(AllConfigInfo set)
         {
             for (int i = 0; i < set.TextPanel1.Length; i++)
             {
                  TextPanel1.GetDictionary().Add(set.TextPanel1[i].Id, set.TextPanel1[i]);
             }
             for (int i = 0; i < set.TextPanel2.Length; i++)
             {
                  TextPanel2.GetDictionary().Add(set.TextPanel2[i].Id, set.TextPanel2[i]);
             }
             for (int i = 0; i < set.VerCheckLang.Length; i++)
             {
                  VerCheckLang.GetDictionary().Add(set.VerCheckLang[i].Id, set.VerCheckLang[i]);
             }
         }
         public static void Deserialize(AllHotConfigInfo set)
         {
             for (int i = 0; i < set.TestDicExcel.Length; i++)
             {
                  TestDicExcel.GetDictionary().Add(set.TestDicExcel[i].Id, set.TestDicExcel[i]);
             }
             for (int i = 0; i < set.TestExcel0.Length; i++)
             {
                  TestExcel0.GetDictionary().Add(set.TestExcel0[i].Id, set.TestExcel0[i]);
             }
             for (int i = 0; i < set.TestExcel1.Length; i++)
             {
                  TestExcel1.GetDictionary().Add(set.TestExcel1[i].Id, set.TestExcel1[i]);
             }
             for (int i = 0; i < set.TestLanguage.Length; i++)
             {
                  TestLanguage.GetDictionary().Add(set.TestLanguage[i].Id, set.TestLanguage[i]);
             }
         }
}
