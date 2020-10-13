using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransletedText : MonoBehaviour
{

    [SerializeField] string Arabic;

    public static void Translete()
    {
        if (GameData.I.Lang == "ar")
        {
            var all = FindObjectsOfType<TransletedText>();
            for (int i = 0; i < all.Length; i++)
            {

                //ArabicLineFixer.Fix(all[i].GetComponent<Text>(), all[i].GetComponent<TransletedText>().Arabic);
                all[i].GetComponent<Text>().text = ArabicSupport.ArabicFixer.Fix(all[i].GetComponent<TransletedText>().Arabic);
            }
        }
    }

}
