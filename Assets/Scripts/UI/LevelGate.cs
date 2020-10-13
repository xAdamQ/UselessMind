using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGate : MonoBehaviour
{
    [HideInInspector] public int FlagInd;
    [HideInInspector] public int PackInd;

    public void OnClick()
    {
        LevelSelectManger.I.LevelPanel.SetActive(false);
        Game.I.StartNewGame(PackInd, FlagInd);
    }

}
