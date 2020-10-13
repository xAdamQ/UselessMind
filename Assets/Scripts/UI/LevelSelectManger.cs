using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManger : MonoBehaviour
{
    public static LevelSelectManger I;

    [SerializeField]
    GameObject
        OpenedLevelGatePrefab,
        LockedLevelGatePrefab,
        OpenedFlagLevelGatePrefab,
        LockedFlagLevelGatePrefab,
        PackGatePrefab;

    public GameObject
        PackPanel,
        LevelPanel;

    [SerializeField]
    Transform
        PackPanelContent,
        LevelPanelContent;

    void Awake()
    {
        I = this;
    }
    void Start()
    {
        LoadPackGatePanel();
    }

    public void LoadLevelGatePanel(int packInd)
    {
        var flagCounter = 0;

        LevelPanel.SetActive(true);

        int openedLevCount;
        if (packInd < GameData.I.OpenedPack)
            openedLevCount = GameData.I.PackLevCapacity[packInd];
        else if (packInd > GameData.I.OpenedPack)
            openedLevCount = 0;
        else
            openedLevCount = GameData.I.Flags[packInd][GameData.I.OpenedFlag] + 1;

        //load opened
        for (int i = 0; i < openedLevCount; i++)
        {

            GameObject newGate = null;
            if (flagCounter != GameData.I.Flags[packInd].Length && GameData.I.Flags[packInd][flagCounter] == i)
            {
                newGate = Instantiate(OpenedFlagLevelGatePrefab, LevelPanelContent);
                newGate.transform.GetChild(1).GetComponent<Text>().text = GameData.I.FlagScores[packInd][flagCounter].ToString();
                newGate.GetComponent<LevelGate>().FlagInd = flagCounter;
                newGate.GetComponent<LevelGate>().PackInd = packInd;

                flagCounter++;
            }//flag
            else
            {
                newGate = Instantiate(OpenedLevelGatePrefab, LevelPanelContent);
            }

            newGate.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
        }

        //load locked
        for (int i = openedLevCount; i < GameData.I.PackLevCapacity[packInd]; i++)
        {
            GameObject newGatePref = null;
            if (flagCounter != GameData.I.Flags[packInd].Length && GameData.I.Flags[packInd][flagCounter] == i)
            {
                newGatePref = LockedFlagLevelGatePrefab;
                flagCounter++;
            }
            else
            {
                newGatePref = LockedLevelGatePrefab;
            }

            var newGate = Instantiate(newGatePref, LevelPanelContent);

            newGate.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
        }



    }

    public void LoadPackGatePanel()
    {
        for (int i = 0; i < GameData.I.PackLevCapacity.Length; i++)
        {
            var newGate = Instantiate(PackGatePrefab, PackPanelContent);
            newGate.GetComponent<PackGate>().packInd = i;
            newGate.transform.GetChild(0)/*text*/.GetComponent<Text>().text = "Pack " + Game.I.PackNames[i];
        }
    }

    //public void DestroyLevelPanel()
    //{
    //    Destroy(LevelPanel);
    //}

    public void ClearLevelPanel()
    {
        foreach (Transform item in LevelPanelContent)
        {
            Destroy(item.gameObject);
        }
    }

}
