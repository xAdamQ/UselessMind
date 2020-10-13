using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Linq;

public class LanguageManger : MonoBehaviour
{
    public static LanguageManger I;

    public XDocument LangFile;

    public enum LanguageType { English, Arabic }
    public LanguageType Language;

    void Awake()
    {
        I = this;

        LangFile = XDocument.Load("Assets/Other/" + Language.ToString() + ".xml");

        if (Language == LanguageType.Arabic)
        {
            GetValues = GetArabicValuesInArray;
        }
        else
        {
            GetValues = GetValuesInArray;
        }

    }

    public delegate string[] del(string tag);
    public del GetValues;

    string[] GetArabicValuesInArray(string tag)
    {
        var arr = GetValuesInArray(tag);

        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = ArabicSupport.ArabicFixer.Fix(arr[i]);
        }

        return arr;
    }
    string[] GetValuesInArray(string tag)
    {
        var tagEles = LangFile.Descendants(tag);
        return tagEles.Select(n => n.Value).ToArray();
    }

}
