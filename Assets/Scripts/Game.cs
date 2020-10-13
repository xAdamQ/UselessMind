using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using DG.Tweening;

public class Game : MonoBehaviour
{

    #region genral props
    public List<BaseCard>[] AllCards;

    public Text CoinText;

    public static Game I;

    Hashtable TextColorHT;

    [SerializeField] GameObject[] StartCardBacks;

    [SerializeField] RectTransform CanvasRect;

    public readonly string[] PackNames = { "A", "F", "X", "Z", };
    #endregion

    #region ini

    void Awake()
    {
        I = this;

        GameData.Load();
        //GameData.I.Lang = "ar";
        //GameData.I.Coins = 5000;
        //GameData.Save();

        TextColorHT = new Hashtable();
        TextColorHT.Add("from", 0);
        TextColorHT.Add("to", 1);
        TextColorHT.Add("time", .5f);
        TextColorHT.Add("onupdate", "SetTextColor");
        TextColorHT.Add("onupdatetarget", gameObject);

        TransletedText.Translete();

        //eg: if 3 so {0,.5,1}
        CoinThreshold = new float[CoinsPref.Length];
        var increaseUnit = 1f / (CoinThreshold.Length - 1);
        for (int i = 0; i < CoinThreshold.Length; i++) CoinThreshold[i] = i * increaseUnit;

        DOTween.Init();
        MyLib.Ini(CanvasRect);

    }

    void Start()
    {
        OrdinaryCard.OneTimeIni();
        BaseCard.OneTimeIni();

        OnLevDone = new Action(() => { });
        GameData.I.Coins += 0;

        RefreshStartCardBack();
        //CurrentXpLevel = CalcCurXpLev();
        Level.DebugLevelsReport();
    }

    #endregion

    #region new game
    [SerializeField] int TSTJumpToLevel = -1;
    public event Action GameStatring;
    public void StartNewGame(int packInd, int flagInd)
    {
        IsOver = false;

        GameStatring();

        NewGameCards.SetActive(false);
        StartUI.SetActive(false);
        InGameUI.SetActive(true);

        CurrentLevelIndex = GameData.I.Flags[packInd][flagInd];
        CurrentPackIndex = packInd;

        //set every game props
        Lifes = GameData.I.Lifes;
        Score = GameData.I.FlagScores[packInd][flagInd];
        if (TSTJumpToLevel != -1)
            CurrentLevelIndex = TSTJumpToLevel;

        ConsecutiveLevels = 1;
        var levPath = System.IO.Path.Combine("Levels", CurrentPackIndex.ToString(), CurrentLevelIndex.ToString());
        Level.Current = Resources.Load<Level>(levPath);
        StartCoroutine(LoadLevel());

        if (Lifes > 0)
            LifesGo.SetActive(true);

    }
    #endregion

    #region load level
    float[] CoinThreshold;
    /// <summary>
    /// retun coin index based on level variables
    /// description inside
    /// </summary>
    int CalcCoin()
    {
        //we get 2 poss the fisrt is no coin based on the consecutive levels you pass
        //the sec is coin poss which is 1 minus no coin poss
        //there's types of coins, each one has threshold eg{0,.5,1}
        //we get percent from current level over all levels
        //we use that percent to get the distnace from level to each threshold
        //then we invert each one to get a weight
        //then we calc a percent from each weight to all weights
        //we multiply these percents by the the whole sec poss (1 minus no coin poss)
        //to get a 1 devided with no coin poss, no 1 coin poss, no 2 coin poss, etc...

        var zeroPercent = .9f / ConsecutiveLevels;

        var currentLevelPercent = int.Parse(Level.Current.name) / (float)Level.AllCount;

        var coinWeights = new float[CoinsPref.Length];
        var weightSum = 0f;
        for (int i = 0; i < coinWeights.Length; i++)
        {
            var distance = Mathf.Abs(currentLevelPercent - CoinThreshold[i]);

            if (distance != 0)
                coinWeights[i] = 1 / distance;
            else
                coinWeights[i] = float.MaxValue;

            weightSum += coinWeights[i];
        }//calc coin wieghts

        var allCoinPercent = 1 - zeroPercent;
        var eachCoinPrecent = new float[CoinsPref.Length];
        for (int i = 0; i < eachCoinPrecent.Length; i++)
        {
            eachCoinPrecent[i] = coinWeights[i] / weightSum;
        }//calc each coin percent

        var allPoss = new float[CoinsPref.Length + 1]; //including zero
        var debug = "";
        allPoss[0] = zeroPercent;
        debug += allPoss[0] + "___";
        for (int i = 1; i < allPoss.Length; i++)
        {
            allPoss[i] = eachCoinPrecent[i - 1] * allCoinPercent;
            debug += allPoss[i] + "___";
        }//calc the whole 1 percents (including zero)
        Debug.Log(debug);
        return MyLib.SlicedRandom(eachCoinPrecent) - 1; //if returned -1 so no coin
    }
    public void RefreshStartCardBack()
    {
        StartCardBacks[0].GetComponent<SpriteRenderer>().sprite = CardBackMarket.I.Grid.GetChild(GameData.I.CardBack).GetComponent<Image>().sprite;
        StartCardBacks[1].GetComponent<SpriteRenderer>().sprite = CardBackMarket.I.Grid.GetChild(GameData.I.CardBack).GetComponent<Image>().sprite;
    }
    [SerializeField] GameObject OrdinaryCardPref, WhiteCardPref, JokerCardPref, MonsterCardPref, MeteorCardPref, ExtraFlipsCardPref, FreezeCardPref;
    [SerializeField] GameObject[] CoinsPref;
    static int[] JackIndices = new int[] { 10, 23, 36, 49 }, QueenIndecis = new int[] { 12, 25, 38, 51 };
    public event Action LevelLoaded, LevelLoading;
    public void SetTextColor(float alpha)
    {
        {
            var newAlpha = CenterMessage.GetComponent<Text>().color; newAlpha.a = alpha;
            CenterMessage.GetComponent<Text>().color = newAlpha;
        }
        {
            var shadows = CenterMessage.GetComponents<Shadow>();
            var newAlpha = shadows[0].effectColor; newAlpha.a = alpha;
            shadows[0].effectColor = newAlpha;
        }
    }
    void SetCardBackInResources()
    {
        WhiteCardPref.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        OrdinaryCardPref.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        JokerCardPref.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        MonsterCardPref.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        MeteorCardPref.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        ExtraFlipsCardPref.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        FreezeCardPref.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        CoinsPref[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        CoinsPref[1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        CoinsPref[2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
        =
        CardBackMarket.I.Grid.GetChild(GameData.I.CardBack).GetComponent<Image>().sprite;
    }
    [SerializeField] Text LevelText, PackText, ScoreText;
    IEnumerator LoadLevel()
    {
        LevelLoading();
        SetCardBackInResources();

        IsLevelDone = false;

        if (Level.Current.StartMessage != null && Level.Current.StartMessage != "")
        {
            iTween.ValueTo(gameObject, TextColorHT);
            CenterMessage.text = Level.Current.StartMessage;
            CenterMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(Level.MessageReadTime);
            CenterMessage.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(.3f);

        var linearSize = Level.Current.GridSize.x * Level.Current.GridSize.y;
        var allCards = new BaseCard[linearSize];

        var currentInd = 0;

        if (Level.Current.NoMoney == false)
        {
            var coinInd = CalcCoin();
            if (coinInd != -1) createCard(CoinsPref[coinInd]);
        }

        //createCard(CoinsPref[0]);

        for (int i = 0; i < Level.Current.ExtraFlipsCount; i++)
            createCard(ExtraFlipsCardPref);
        for (int i = 0; i < Level.Current.FreezeCount; i++)
            createCard(FreezeCardPref);
        for (int i = 0; i < Level.Current.MeteorCount; i++)
            createCard(MeteorCardPref);
        for (int i = 0; i < Level.Current.jokerCount; i++)
            createCard(JokerCardPref);
        for (int i = 0; i < Level.Current.monsterCount; i++)
            createCard(MonsterCardPref);

        if (Level.Current.whiteCount != -1)
        {
            for (int i = 0; i < Level.Current.whiteCount; i++)
            {
                createCard(WhiteCardPref);
            }
        }
        else
        {
            var remaining2 = linearSize - currentInd;
            for (int i = 0; i < remaining2; i++)
            {
                createCard(WhiteCardPref);
            }
        }//fill with white
        if (Level.Current.coupleCount != -1)
        {
            for (int i = 0; i < Level.Current.coupleCount; i++)
            {
                createCouple();
            }
        }
        else
        {
            var remaining2 = linearSize - currentInd;
            var coupleCount = (remaining2 - (remaining2 % 2)) / 2;
            for (int i = 0; i < coupleCount; i++)
            {
                createCouple();
            }
        }//fill with couples

        void createCouple()
        {
            var jack = JackIndices[UnityEngine.Random.Range(0, JackIndices.Length)];
            createCard(OrdinaryCardPref).GetComponent<OrdinaryCard>().SetData(jack);
            var queen = jack + 2; // * this guarntee both same color and shape
            createCard(OrdinaryCardPref).GetComponent<OrdinaryCard>().SetData(queen);

        }

        var remaining = linearSize - currentInd;
        var remainder = remaining % Level.Current.CloneCount;

        if (remainder != 0)
        {
            for (int i = 0; i < remainder; i++)
            {
                createCard(WhiteCardPref);
            }
        }

        var shuffledIds = new List<int>();
        for (int i = 0; i < 52; i++)
            shuffledIds.Add(i);
        shuffledIds.Shuffle();

        for (int i = 0; i < (remaining - remainder) / Level.Current.CloneCount; i++)
        {
            for (int c = 0; c < Level.Current.CloneCount; c++)
            {
                createCard(OrdinaryCardPref).GetComponent<OrdinaryCard>().SetData(shuffledIds[i]);
            }
        }

        GameObject createCard(GameObject prefab)
        {
            var newCard = Instantiate(prefab);
            newCard.transform.position = Camera.main.transform.position + Vector3.forward * 10;
            allCards[currentInd] = newCard.GetComponent<BaseCard>();
            currentInd++;
            return newCard;
        }

        allCards.Shuffle();

        //2d visual organize
        AllCards = new List<BaseCard>[Level.Current.GridSize.x];
        for (int i = 0; i < AllCards.Length; i++) AllCards[i] = new List<BaseCard>(); //ini all cards
        var linInd = 0;
        for (int y = 0; y < Level.Current.GridSize.y; y++)
        {
            for (int x = 0; x < Level.Current.GridSize.x; x++)
            {
                allCards[linInd].SetAndAnimateIndex(new Vector2Int(x, y));
                AllCards[x].Add(allCards[linInd]);
                linInd++;
            }
        }

        if (GameData.I.UncoverdOnStart != 0)
        {
            AddBlockTime(GameData.I.StartUncoverTime + BaseCard.FlipTime);
            allCards.Shuffle();
            var uncoverdCount = allCards.Length > GameData.I.UncoverdOnStart ? GameData.I.UncoverdOnStart : allCards.Length;
            for (int i = 0; i < uncoverdCount; i++)
            {
                allCards[i].ShowAnim(BaseCard.FlipTime);
                allCards[i].HideAnim(GameData.I.StartUncoverTime + BaseCard.FlipTime);
            }
        }

        LevelLoaded();
        PauseButton.SetActive(true);
    }
    #endregion

    #region block click
    [HideInInspector] public bool ClickAvai = true;
    float BlockTime;
    public void AddBlockTime(float value)
    {
        BlockTime += value;
        if (ClickAvai) StartCoroutine(BlockClick());
    }

    IEnumerator BlockClick()
    {
        ClickAvai = false;

        do
        {
            var consumedBlockTime = BlockTime;
            BlockTime = 0;
            yield return new WaitForSeconds(consumedBlockTime);
        }
        while (BlockTime != 0);

        ClickAvai = true;

    }
    #endregion

    #region game over
    public event Action GettingHiScore;
    void SetNewHighScore()
    {
        if (Score > GameData.I.HighestScore)
        {
            GettingHiScore();
            GameData.I.HighestScore = Score;
            GameData.Save();
        }
    }
    [SerializeField] GameObject LifesGo, NewGameCards, StartUI, GameOverUI, GatesUI, InGameUI, PauseButton;
    [SerializeField] Text LifesText;
    [HideInInspector] public bool IsOver;
    public event Action GameOvering, GameOvered, StartScreening;
    /// <summary>
    /// delay: before cards killed
    /// thenDelay: after
    /// </summary>
    public IEnumerator GameOver(float delay = 0, float delay2 = 0, bool quit = false)
    {
        GameOvering();
        IsOver = true;
        AddBlockTime(delay + delay2 + BaseCard.DieTime);

        yield return new WaitForSeconds(delay);

        BaseCard.DestroyAllCards();
        yield return new WaitForSeconds(BaseCard.DieTime + delay2);


        if (quit == false && Lifes > 0)
        {
            Lifes--;
            IsOver = false;
            StartCoroutine(LoadLevel());
            GameOvered();
            yield break;
        }//my lose a life only
        else
        {
            InGameUI.SetActive(false);
            GameOverUI.SetActive(true);
            PauseButton.SetActive(false);
            GameOvered();

            while (Input.GetMouseButton(0) == false)
                yield return new WaitForEndOfFrame();

            StartScreening();

            Camera.main.transform.position = Vector3.back * 10;

            GameOverUI.SetActive(false);
            StartUI.SetActive(true);
            GatesUI.SetActive(true);
            NewGameCards.SetActive(true);

            SetNewHighScore();
        } //total lose

    }

    public void QuitGame()
    {
        StartCoroutine(GameOver(quit: true));
    }
    #endregion

    #region level done
    void AddLevelDoneScore()
    {
        Score += MyLib.GetExpResult(.5f, 2.5f, currentLevelIndex + 1);
    }
    [SerializeField] public Text CenterMessage;
    public static event Action OnLevDone;
    [HideInInspector] public bool IsLevelDone;
    public IEnumerator LevelDone()
    {
        PauseButton.SetActive(false);
        IsLevelDone = true;

        ConsecutiveLevels++;
        AddLevelDoneScore();

        yield return new WaitForSeconds(1f);

        OnLevDone();

        for (int i = 0; i < GameData.I.Flags[CurrentPackIndex].Length; i++)
        {
            if (GameData.I.Flags[CurrentPackIndex][i] == CurrentLevelIndex)//flag level
            {
                if (CurrentLevelIndex == GameData.I.PackLevCapacity[CurrentPackIndex] - 1)//last lev in pack
                {
                    if (GameData.I.OpenedPack != GameData.I.PackLevCapacity.Length - 1)//not last pack in game
                    {
                        GameData.I.OpenedPack++;
                        GameData.I.OpenedFlag = 0;
                    }
                }
                else
                {
                    if (GameData.I.OpenedPack == CurrentPackIndex && i > GameData.I.OpenedFlag)
                        GameData.I.OpenedFlag = i;
                }

                if (Score > GameData.I.FlagScores[CurrentPackIndex][i])
                {
                    GameData.I.FlagScores[CurrentPackIndex][i] = Score;
                }//hi score

                GameData.Save();
                break;
            }
        }

        BaseCard.DestroyAllCards();

        CurrentLevelIndex++;
        var levPath = System.IO.Path.Combine("Levels", CurrentPackIndex.ToString(), CurrentLevelIndex.ToString());
        Level.Current = Resources.Load<Level>(levPath);

        if (Level.Current == null)
        {
            TheHolyGameIsDone();
            yield break;
        }

        StartCoroutine(LoadLevel());
    }
    #endregion

    #region every game props
    int lifes;
    int Lifes
    {
        get
        {
            return lifes;
        }
        set
        {
            lifes = value;
            LifesText.text = value.ToString();
        }
    }

    int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            ScoreText.text = value.ToString();
        }
    }

    int currentLevelIndex;
    int CurrentLevelIndex
    {
        get => currentLevelIndex;
        set
        {
            currentLevelIndex = value;
            LevelText.text = (value + 1).ToString();
        }
    }

    int currentPackIndex;
    int CurrentPackIndex
    {
        get => currentPackIndex;
        set
        {
            currentPackIndex = value;
            PackText.text = PackNames[value];
        }
    }


    int ConsecutiveLevels;
    #endregion

    public void SetTimeScale(int val)
    {
        Time.timeScale = val;
    }

    void TheHolyGameIsDone()
    {
        Debug.Log("Game Is Done Thank YOU");
    }

}
