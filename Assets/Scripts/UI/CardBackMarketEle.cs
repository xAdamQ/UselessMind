using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBackMarketEle : MonoBehaviour
{
    public int Index { set; get; }

    public void SetCardBack()
    {
        GameData.I.CardBack = Index;
        Game.I.RefreshStartCardBack();

        GameData.Save();
    }
}
