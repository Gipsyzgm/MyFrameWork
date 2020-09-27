/*
 *  项目名字：LanguageMgr
 *  创建时间：2020.6.23
 *  描述信息：多语言简单实现。
 *  使用说明：
 *  1：没什么好说的。GetById(int id)方法里面读取方法需要和你定义的多语言表格一致。
 *  2：增加语言的话要把需要当前脚本需要区分的位置都更改。
 */
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
    //初始化语言。
	public static void Init () {

        if(Application.systemLanguage == SystemLanguage.Chinese)
        {
            type = LanguageType.cn;
        }
        else
        {
            type = LanguageType.en;
        }
        //如果存的有，设置为默认的
        string defaul = PlayerPrefs.GetString("Language");
        //检索指定枚举中常数名称的数组。
        string[] language = System.Enum.GetNames(typeof(LanguageType));
        for (int i = 0; i < language.Length; i++)
        {
            if (defaul.Equals(language[i]))
            {
                //把将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的枚举对象。
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
        LacalText[] locals = FindObjectsOfType<LacalText>();

        for(int i=0;i<locals.Length;i++)
        {
            Text t = locals[i].GetComponent<Text>();
            if(t != null)
            {
                t.text = GetById(locals[i].languageId);
            }
        }
    }

    //用Id取一个语言。TestLanguage脚本必须和语言表格的脚本名称对应。
    public static string GetById(int id)
    {
        string temp;
        switch (type)
        {
            case LanguageType.cn:
                temp = TestLanguage.Get(id).cn;
                break;
            case LanguageType.en:
                temp = TestLanguage.Get(id).en;
                break;
            default:
                temp = TestLanguage.Get(id).en;
                break;
        }
       
        return temp;
    }

}

