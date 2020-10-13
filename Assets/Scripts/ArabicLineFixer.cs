using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;

public class ArabicLineFixer : MonoBehaviour
{
    public static void Fix(Text textComp)
    {

        textComp.text = ArabicFixer.Fix(textComp.text, false, false);
        Canvas.ForceUpdateCanvases();

        // cached text generator must be get by text comp so we use it
        var fixedText = new string[textComp.cachedTextGenerator.lineCount];
        for (int line = 0; line < textComp.cachedTextGenerator.lines.Count; line++)
        {
            var startIndex = textComp.cachedTextGenerator.lines[line].startCharIdx;
            var endIndex = (line == textComp.cachedTextGenerator.lines.Count - 1) ? textComp.text.Length
                 : textComp.cachedTextGenerator.lines[line + 1].startCharIdx;

            var length = endIndex - startIndex;
            fixedText[line] = textComp.text.Substring(startIndex, length);
        }// saperete all rlines in an array

        textComp.text = "";
        for (int rline = fixedText.Length - 1; rline >= 0; rline--)
        {
            if (fixedText[rline] != "" && fixedText[rline] != "\n" && fixedText[rline] != null)
                textComp.text += fixedText[rline] + "\n";
        }//invert this array

        Debug.Log(textComp.text);
    }
    public static void Fix(Text textComp, string newString)
    {
        textComp.text = newString;
        Fix(textComp);
    }
}