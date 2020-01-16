using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageMgr : MonoBehaviour {

    public enum LanguageType
    {
        cn,
        en,
    }
    public static LanguageType type = LanguageType.cn;

	void Awake () {
        if(Application.systemLanguage == SystemLanguage.Chinese)
        {
            type = LanguageType.cn;
        }
        else
        {
            type = LanguageType.en;
        }

        string defaul = PlayerPrefs.GetString("Language", string.Empty);

        string[] language = System.Enum.GetNames(typeof(LanguageType));
        for (int i = 0; i < language.Length; i++)
        {
            if (defaul.Equals(language[i]))
            {
                type = (LanguageType)System.Enum.Parse(typeof(LanguageType), defaul);
                break;
            }
        }
    }

    //设置语言
    public static void ChangeLanguage(LanguageType _type)
    {
        type = _type;

        UpdateLanguage();

        PlayerPrefs.SetString("Language", (_type.ToString()));
    }
    static void UpdateLanguage()
    {
        LabelLocal[] locals = FindObjectsOfType<LabelLocal>();

        for(int i=0;i<locals.Length;i++)
        {
            Text t = locals[i].GetComponent<Text>();
            if(t != null)
            {
                t.text = GetById(locals[i].languageId);
            }
        }
    }

    //用Id取一个语言
    public static string GetById(int id)
    {
        if (type == LanguageType.cn)
        {
            return Language.cn[id];
        }
        else if (type == LanguageType.en)
        {
            return Language.en[id];
        }

        return Language.en[id];
    }


}
public class Language
{

   public static string[] cn = { };
   public static string[] en = { };

}
