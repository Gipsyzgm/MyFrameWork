using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

//每次都会重新生成的脚本，不要删，覆盖就行了
public class DataMgr : MonoSingleton<DataMgr>
{
    public void InitAllConfig()
    {
        var allConfig = LoaderMgr.Instance.LoadAssetSync<AllConfigInfo>("GameData/AllConfigInfo");
        Deserialize(allConfig);
        LoaderMgr.Instance.Release("GameData/AllConfigInfo");
    }

    public static void Deserialize(AllConfigInfo set)
    {
        for (int i = 0; i < set.ArmsInfo.Length; i++)
        {
            ArmsInfo ID;
            ArmsInfo.GetDictionary().TryGetValue(set.ArmsInfo[i].Id, out ID);
            if (ID != null)
            {
                Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!", "ArmsInfo",
                    set.ArmsInfo[i].Id));
            }
            else
            {
                ArmsInfo.GetDictionary().Add(set.ArmsInfo[i].Id, set.ArmsInfo[i]);
                ArmsInfo.GetList().Add(set.ArmsInfo[i]);
            }
        }

        for (int i = 0; i < set.EquipInfo.Length; i++)
        {
            EquipInfo ID;
            EquipInfo.GetDictionary().TryGetValue(set.EquipInfo[i].Id, out ID);
            if (ID != null)
            {
                Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!", "EquipInfo",
                    set.EquipInfo[i].Id));
            }
            else
            {
                EquipInfo.GetDictionary().Add(set.EquipInfo[i].Id, set.EquipInfo[i]);
                EquipInfo.GetList().Add(set.EquipInfo[i]);
            }
        }

        for (int i = 0; i < set.GameLang.Length; i++)
        {
            GameLang ID;
            GameLang.GetDictionary().TryGetValue(set.GameLang[i].Id, out ID);
            if (ID != null)
            {
                Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!", "GameLang",
                    set.GameLang[i].Id));
            }
            else
            {
                GameLang.GetDictionary().Add(set.GameLang[i].Id, set.GameLang[i]);
                GameLang.GetList().Add(set.GameLang[i]);
            }
        }

        for (int i = 0; i < set.GiftEffectConfig.Length; i++)
        {
            GiftEffectConfig ID;
            GiftEffectConfig.GetDictionary().TryGetValue(set.GiftEffectConfig[i].Id, out ID);
            if (ID != null)
            {
                Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!", "GiftEffectConfig",
                    set.GiftEffectConfig[i].Id));
            }
            else
            {
                GiftEffectConfig.GetDictionary().Add(set.GiftEffectConfig[i].Id, set.GiftEffectConfig[i]);
                GiftEffectConfig.GetList().Add(set.GiftEffectConfig[i]);
            }
        }

        for (int i = 0; i < set.GradeInnerConfig.Length; i++)
        {
            GradeInnerConfig ID;
            GradeInnerConfig.GetDictionary().TryGetValue(set.GradeInnerConfig[i].Id, out ID);
            if (ID != null)
            {
                Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!", "GradeInnerConfig",
                    set.GradeInnerConfig[i].Id));
            }
            else
            {
                GradeInnerConfig.GetDictionary().Add(set.GradeInnerConfig[i].Id, set.GradeInnerConfig[i]);
                GradeInnerConfig.GetList().Add(set.GradeInnerConfig[i]);
            }
        }

        for (int i = 0; i < set.LevelInfo.Length; i++)
        {
            LevelInfo ID;
            LevelInfo.GetDictionary().TryGetValue(set.LevelInfo[i].Id, out ID);
            if (ID != null)
            {
                Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!", "LevelInfo",
                    set.LevelInfo[i].Id));
            }
            else
            {
                LevelInfo.GetDictionary().Add(set.LevelInfo[i].Id, set.LevelInfo[i]);
                LevelInfo.GetList().Add(set.LevelInfo[i]);
            }
        }

        for (int i = 0; i < set.SkillInfo.Length; i++)
        {
            SkillInfo ID;
            SkillInfo.GetDictionary().TryGetValue(set.SkillInfo[i].Id, out ID);
            if (ID != null)
            {
                Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!", "SkillInfo",
                    set.SkillInfo[i].Id));
            }
            else
            {
                SkillInfo.GetDictionary().Add(set.SkillInfo[i].Id, set.SkillInfo[i]);
                SkillInfo.GetList().Add(set.SkillInfo[i]);
            }
        }

        for (int i = 0; i < set.UpdateNotice.Length; i++)
        {
            UpdateNotice ID;
            UpdateNotice.GetDictionary().TryGetValue(set.UpdateNotice[i].Id, out ID);
            if (ID != null)
            {
                Debug.LogError(string.Format("{0}数据唯一ID{1}重复,数据覆盖,数据不支持重复ID,请核实修正避免Bug!", "UpdateNotice",
                    set.UpdateNotice[i].Id));
            }
            else
            {
                UpdateNotice.GetDictionary().Add(set.UpdateNotice[i].Id, set.UpdateNotice[i]);
                UpdateNotice.GetList().Add(set.UpdateNotice[i]);
            }
        }
    }
}