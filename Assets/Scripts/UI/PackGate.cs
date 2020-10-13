using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackGate : MonoBehaviour
{

    [HideInInspector] public int packInd;

    public void OnCLick()
    {
        LevelSelectManger.I.LoadLevelGatePanel(packInd);
        LevelSelectManger.I.PackPanel.SetActive(false);
    }

}
