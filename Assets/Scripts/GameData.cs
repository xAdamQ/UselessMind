using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public float StartUncoverTime;
    public int UncoverdOnStart;
    public int ExtraFlipAmount;
    public float FreezeAmount;
    public float JokerSeeTime;
    public int Lifes;
    public int Xp;

    private int coins;
    public int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
            Game.I.CoinText.text = value.ToString();
            Game.I.CoinText.GetComponent<Animation>().Play();
        }
    }

    public int HighestLevel;
    public int HighestScore;

    public int CardBack;

    public string Lang;

    public int OpenedFlag, OpenedPack;
    public int[] PackLevCapacity;
    public int[][] Flags;
    public int[][] FlagScores;


    #region untility
    public static GameData I;

    static string DataPath = Application.persistentDataPath + "GD.prog";
    public static void Save()
    {
        var formatter = new BinaryFormatter();
        var stream = new FileStream(DataPath, FileMode.Create);
        formatter.Serialize(stream, I);
        stream.Close();
    }
    public static void Load()
    {
        if (File.Exists(DataPath))
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(DataPath, FileMode.Open);

            var data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            I = data;
        }
        else
        {
            FirstEnter();
        }
    }//only happen once in app

    public static void Delete()
    {
        if (File.Exists(DataPath))
        {
            File.Delete(DataPath);
            Debug.Log("data deleted successfully");
        }
        else
        {
            Debug.Log("data already deleted");
        }
    }

    static void FirstEnter()
    {
        I = new GameData();
        I.setDefaults();
        Save();
    }

    void setDefaults()
    {
        StartUncoverTime = 0;
        UncoverdOnStart = 0;

        ExtraFlipAmount = 4;
        FreezeAmount = 5;

        Lifes = 0;
        Coins = 0;

        HighestLevel = 0;
        HighestScore = 0;

        Lang = "en";

        OpenedFlag = 0;
        OpenedPack = 0;

        PackLevCapacity = new int[] { 20, 5 };

        Flags = new int[PackLevCapacity.Length][];
        Flags[0] = new int[] { 0, 8, 14, PackLevCapacity[0] - 1 };//0 have and last ind has to be flags always
        Flags[1] = new int[] { 0, 12, 27, PackLevCapacity[1] - 1 };

        FlagScores = new int[Flags.Length][];
        for (int i = 0; i < Flags.Length; i++) FlagScores[i] = new int[Flags[i].Length];

    }
    #endregion

}
