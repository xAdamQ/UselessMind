using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardBackMarket : MonoBehaviour
{
    static float Ceofficient => 3f;
    static float Exponent => 3f;
    static float BacksCount;

    [SerializeField] GameObject UnknownCardBackPrefab, ShownCardPrefab;
    [SerializeField] public Transform Grid;
    Sprite[] AllBackSprites;

    private void Awake()
    {
        I = this;
        Game.I.GettingHiScore += OnNewHighScore;

        var list = new List<Sprite>();
        list.AddRange(Resources.LoadAll<Sprite>("Sprites/8"));
        list.AddRange(Resources.LoadAll<Sprite>("Sprites/7"));

        AllBackSprites = list.ToArray();
        BacksCount = AllBackSprites.Length;

        RefreshGrid();
    }
    public static CardBackMarket I;


    void OnNewHighScore()
    {
        RefreshGrid();
    }

    void RefreshGrid()
    {
        for (int i = 0; i < Grid.childCount; i++)
        {
            Destroy(Grid.GetChild(i).gameObject);
        }//clear grid

        //generate market
        //using current HI get last opened card index?
        var lastOpenedCardInd = MyLib.GetBaseOfExp(Ceofficient, Exponent, GameData.I.HighestScore);

        for (int i = 0; i <= lastOpenedCardInd; i++)
        {
            var newBack = Instantiate(ShownCardPrefab, Grid);
            newBack.GetComponent<UnityEngine.UI.Image>().sprite = AllBackSprites[i];
            newBack.GetComponent<CardBackMarketEle>().Index = i;
        }//instantiate shown

        for (int i = lastOpenedCardInd + 1; i < BacksCount; i++)
        {
            var newBack = Instantiate(UnknownCardBackPrefab, Grid);
            newBack.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = MyLib.GetExpResult(Ceofficient, Exponent, i).ToString();
        }//make hidden
    }



}
