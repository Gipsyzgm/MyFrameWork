using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class DataMgr :MonoSingleton<DataMgr>
{
         private  AllConfigInfo AllConfig; 
         public void InitAllConfig() 
         {
             AllConfig = Resources.Load<AllConfigInfo>("AllConfigInfo");
             Deserialize(AllConfig);
             Resources.UnloadUnusedAssets();
         }
         
         public static void Deserialize(AllConfigInfo set)
         {
             for (int i = 0; i < set.ArmsInfo.Length; i++)
             {
                  ArmsInfo ID;
                  ArmsInfo.GetDictionary().TryGetValue(set.ArmsInfo[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","ArmsInfo", set.ArmsInfo[i].Id));
                    }
                    else
                    {
                         ArmsInfo.GetDictionary().Add(set.ArmsInfo[i].Id, set.ArmsInfo[i]);
                         ArmsInfo.GetAllKey().Add(set.ArmsInfo[i].Id);
                    }
             }
             for (int i = 0; i < set.EquipInfo.Length; i++)
             {
                  EquipInfo ID;
                  EquipInfo.GetDictionary().TryGetValue(set.EquipInfo[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","EquipInfo", set.EquipInfo[i].Id));
                    }
                    else
                    {
                         EquipInfo.GetDictionary().Add(set.EquipInfo[i].Id, set.EquipInfo[i]);
                         EquipInfo.GetAllKey().Add(set.EquipInfo[i].Id);
                    }
             }
             for (int i = 0; i < set.GameLang.Length; i++)
             {
                  GameLang ID;
                  GameLang.GetDictionary().TryGetValue(set.GameLang[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","GameLang", set.GameLang[i].Id));
                    }
                    else
                    {
                         GameLang.GetDictionary().Add(set.GameLang[i].Id, set.GameLang[i]);
                         GameLang.GetAllKey().Add(set.GameLang[i].Id);
                    }
             }
             for (int i = 0; i < set.LevelInfo.Length; i++)
             {
                  LevelInfo ID;
                  LevelInfo.GetDictionary().TryGetValue(set.LevelInfo[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","LevelInfo", set.LevelInfo[i].Id));
                    }
                    else
                    {
                         LevelInfo.GetDictionary().Add(set.LevelInfo[i].Id, set.LevelInfo[i]);
                         LevelInfo.GetAllKey().Add(set.LevelInfo[i].Id);
                    }
             }
             for (int i = 0; i < set.SkillInfo.Length; i++)
             {
                  SkillInfo ID;
                  SkillInfo.GetDictionary().TryGetValue(set.SkillInfo[i].Id, out ID);
                    if (ID!=null)
                    {
                         Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!","SkillInfo", set.SkillInfo[i].Id));
                    }
                    else
                    {
                         SkillInfo.GetDictionary().Add(set.SkillInfo[i].Id, set.SkillInfo[i]);
                         SkillInfo.GetAllKey().Add(set.SkillInfo[i].Id);
                    }
             }
         }
}
