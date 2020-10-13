using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class XpLevelManger : ProgressBar
{
    public static XpLevelManger I;
    [SerializeField] Text LevelText;

    void Awake()
    {
        I = this;

        Game.I.GameOvering += OnGameOvering;
    }

    void Start()
    {
        CurrentLevel = CalcCurLev();
    }

    int Score;
    int CurrentLevel;

    void OnGameOvering()
    {
        GameData.I.Xp += Score;
        GameData.Save();

        var newLevel = CalcCurLev();

        LevelUp(CurrentLevel - newLevel);//handle no level ip case
    }

    float Coff = 10f, Expo = 1.5f;
    int CalcCurLev()
    {
        return MyLib.GetBaseOfExp(Coff, Expo, GameData.I.Xp);
    }

    public event Action LevelUping;
    void LevelUp(int levelsCount)
    {
        if (levelsCount == 0)
            return;

        LevelUping();
    }

}
