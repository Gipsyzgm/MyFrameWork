using UnityEngine;
//每次都会重新生成的脚本，不要删，覆盖就行了
public class AllConfigInfo: ScriptableObject
{
    public ArmsInfo[] ArmsInfo;
    public EquipInfo[] EquipInfo;
    public GameLang[] GameLang;
    public LevelInfo[] LevelInfo;
    public SkillInfo[] SkillInfo;
    public UpdateNotice[] UpdateNotice;
}
