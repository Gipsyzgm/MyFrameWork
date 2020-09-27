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
                  TextPanel1 ID;
                  TextPanel1.GetDictionary().TryGetValue(set.TextPanel1[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","TextPanel1", set.TextPanel1[i].Id));
                    }
                    else
                    {
                         TextPanel1.GetDictionary().Add(set.TextPanel1[i].Id, set.TextPanel1[i]);
                    }
             }
             for (int i = 0; i < set.TextPanel2.Length; i++)
             {
                  TextPanel2 ID;
                  TextPanel2.GetDictionary().TryGetValue(set.TextPanel2[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","TextPanel2", set.TextPanel2[i].Id));
                    }
                    else
                    {
                         TextPanel2.GetDictionary().Add(set.TextPanel2[i].Id, set.TextPanel2[i]);
                    }
             }
             for (int i = 0; i < set.VerCheckLang.Length; i++)
             {
                  VerCheckLang ID;
                  VerCheckLang.GetDictionary().TryGetValue(set.VerCheckLang[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","VerCheckLang", set.VerCheckLang[i].Id));
                    }
                    else
                    {
                         VerCheckLang.GetDictionary().Add(set.VerCheckLang[i].Id, set.VerCheckLang[i]);
                    }
             }
         }
         public static void Deserialize(AllHotConfigInfo set)
         {
             for (int i = 0; i < set.TestDicExcel.Length; i++)
             {
                  TestDicExcel ID;
                  TestDicExcel.GetDictionary().TryGetValue(set.TestDicExcel[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","TestDicExcel", set.TestDicExcel[i].Id));
                    }
                    else
                    {
                         TestDicExcel.GetDictionary().Add(set.TestDicExcel[i].Id, set.TestDicExcel[i]);
                    }
             }
             for (int i = 0; i < set.TestExcel0.Length; i++)
             {
                  TestExcel0 ID;
                  TestExcel0.GetDictionary().TryGetValue(set.TestExcel0[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","TestExcel0", set.TestExcel0[i].Id));
                    }
                    else
                    {
                         TestExcel0.GetDictionary().Add(set.TestExcel0[i].Id, set.TestExcel0[i]);
                    }
             }
             for (int i = 0; i < set.TestExcel1.Length; i++)
             {
                  TestExcel1 ID;
                  TestExcel1.GetDictionary().TryGetValue(set.TestExcel1[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","TestExcel1", set.TestExcel1[i].Id));
                    }
                    else
                    {
                         TestExcel1.GetDictionary().Add(set.TestExcel1[i].Id, set.TestExcel1[i]);
                    }
             }
             for (int i = 0; i < set.TestLanguage.Length; i++)
             {
                  TestLanguage ID;
                  TestLanguage.GetDictionary().TryGetValue(set.TestLanguage[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","TestLanguage", set.TestLanguage[i].Id));
                    }
                    else
                    {
                         TestLanguage.GetDictionary().Add(set.TestLanguage[i].Id, set.TestLanguage[i]);
                    }
             }
         }
}
