using UnityEngine;
using LitJson;
using System.Collections.Generic;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class AllNeedReadExcel
{
     public static void ArmsInfo()
     { 
         Debug.Log("读取表格:ArmsInfo"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("ArmsInfo", 0);
         CreateConfigData.configInfo.ArmsInfo= new ArmsInfo[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.ArmsInfo[i] = new ArmsInfo(); 
             CreateConfigData.configInfo.ArmsInfo[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.ArmsInfo[i].Name= (string) table[i]["Name"];
             CreateConfigData.configInfo.ArmsInfo[i].Des= (string) table[i]["Des"];
             CreateConfigData.configInfo.ArmsInfo[i].HeadImg= (string) table[i]["HeadImg"];
             CreateConfigData.configInfo.ArmsInfo[i].Duty= (string) table[i]["Duty"];
             CreateConfigData.configInfo.ArmsInfo[i].DutyImg= (string) table[i]["DutyImg"];
             CreateConfigData.configInfo.ArmsInfo[i].Skill= (int[]) table[i]["Skill"];
             CreateConfigData.configInfo.ArmsInfo[i].level= (int) table[i]["level"];
             CreateConfigData.configInfo.ArmsInfo[i].TarGetID= (int[]) table[i]["TarGetID"];
             CreateConfigData.configInfo.ArmsInfo[i].Price= (int) table[i]["Price"];
             CreateConfigData.configInfo.ArmsInfo[i].PrefabsObj= (string) table[i]["PrefabsObj"];
             CreateConfigData.configInfo.ArmsInfo[i].MixTreeID= (int[]) table[i]["MixTreeID"];
             CreateConfigData.configInfo.ArmsInfo[i].HP= (int) table[i]["HP"];
             CreateConfigData.configInfo.ArmsInfo[i].Attack= (int) table[i]["Attack"];
             CreateConfigData.configInfo.ArmsInfo[i].Defense= (int) table[i]["Defense"];
             CreateConfigData.configInfo.ArmsInfo[i].Speed= (float) table[i]["Speed"];
             CreateConfigData.configInfo.ArmsInfo[i].Miss= (float) table[i]["Miss"];
             CreateConfigData.configInfo.ArmsInfo[i].Critical= (float) table[i]["Critical"];
             CreateConfigData.configInfo.ArmsInfo[i].Race= (string) table[i]["Race"];
         } 
     } 
     public static void EquipInfo()
     { 
         Debug.Log("读取表格:EquipInfo"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("EquipInfo", 0);
         CreateConfigData.configInfo.EquipInfo= new EquipInfo[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.EquipInfo[i] = new EquipInfo(); 
             CreateConfigData.configInfo.EquipInfo[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.EquipInfo[i].Name= (string) table[i]["Name"];
             CreateConfigData.configInfo.EquipInfo[i].Des= (string) table[i]["Des"];
             CreateConfigData.configInfo.EquipInfo[i].HeadImg= (string) table[i]["HeadImg"];
             CreateConfigData.configInfo.EquipInfo[i].level= (int) table[i]["level"];
             CreateConfigData.configInfo.EquipInfo[i].attack= (int) table[i]["attack"];
             CreateConfigData.configInfo.EquipInfo[i].Price= (int) table[i]["Price"];
         } 
     } 
     public static void GameLang()
     { 
         Debug.Log("读取表格:GameLang"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("GameLang", 0);
         CreateConfigData.configInfo.GameLang= new GameLang[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.GameLang[i] = new GameLang(); 
             CreateConfigData.configInfo.GameLang[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.GameLang[i].cn= (string) table[i]["cn"];
             CreateConfigData.configInfo.GameLang[i].en= (string) table[i]["en"];
         } 
     } 
     public static void LevelInfo()
     { 
         Debug.Log("读取表格:LevelInfo"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("LevelInfo", 0);
         CreateConfigData.configInfo.LevelInfo= new LevelInfo[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.LevelInfo[i] = new LevelInfo(); 
             CreateConfigData.configInfo.LevelInfo[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.LevelInfo[i].HeroCount= (int) table[i]["HeroCount"];
             CreateConfigData.configInfo.LevelInfo[i].EnemyInfo= (string[]) table[i]["EnemyInfo"];
             CreateConfigData.configInfo.LevelInfo[i].LevelType= (int) table[i]["LevelType"];
             CreateConfigData.configInfo.LevelInfo[i].LevelSelect= (int[]) table[i]["LevelSelect"];
         } 
     } 
     public static void SkillInfo()
     { 
         Debug.Log("读取表格:SkillInfo"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("SkillInfo", 0);
         CreateConfigData.configInfo.SkillInfo= new SkillInfo[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.SkillInfo[i] = new SkillInfo(); 
             CreateConfigData.configInfo.SkillInfo[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.SkillInfo[i].Name= (string) table[i]["Name"];
             CreateConfigData.configInfo.SkillInfo[i].Des= (string) table[i]["Des"];
             CreateConfigData.configInfo.SkillInfo[i].SkillImg= (string) table[i]["SkillImg"];
         } 
     } 
     public static void UpdateNotice()
     { 
         Debug.Log("读取表格:UpdateNotice"); 
         List<Dictionary<string, object>> table = CreateConfigData.ReadExcelData("UpdateNotice", 0);
         CreateConfigData.configInfo.UpdateNotice= new UpdateNotice[table.Count];
         for (int i = 0; i < table.Count; i++)
         { 
             CreateConfigData.configInfo.UpdateNotice[i] = new UpdateNotice(); 
             CreateConfigData.configInfo.UpdateNotice[i].Id= (int) table[i]["Id"];
             CreateConfigData.configInfo.UpdateNotice[i].Name= (string) table[i]["Name"];
             CreateConfigData.configInfo.UpdateNotice[i].Content= (string) table[i]["Content"];
             CreateConfigData.configInfo.UpdateNotice[i].Date= (string) table[i]["Date"];
             CreateConfigData.configInfo.UpdateNotice[i].Version= (string) table[i]["Version"];
             CreateConfigData.configInfo.UpdateNotice[i].Sort= (int) table[i]["Sort"];
         } 
     } 
}
