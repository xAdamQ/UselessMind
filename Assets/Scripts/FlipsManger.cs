using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlipsManger : MonoBehaviour
{
    public static FlipsManger I;

    void Awake()
    {
        I = this;
        Game.I.GameOvered += OnGameOvered;
        Game.I.LevelLoaded += OnLevelLoaded;
        BaseCard.Showing += OnCardShowing;
    }

    static int Flips;
    [SerializeField] Text MaxFlipsText, VariableFlipsText;

    void OnCardShowing()
    {
        Flips++;
    }

    int maxFlips;
    public int MaxFlips
    {
        get
        {
            return maxFlips;
        }
        set
        {
            maxFlips = value;
            Visual.GetComponent<Image>().DoNotifyFeedback();
            MaxFlipsText.text = value.ToString();
        }
    }

    void OnCardShown()
    {
        VariableFlipsText.text = Flips.ToString();
        Visual.GetComponent<ProgressBar>().FillValue = 1 - ((float)Flips / MaxFlips);

        if (Flips == MaxFlips && Game.I.IsLevelDone == false) //we must check wining first
        {
            Game.I.StartCoroutine(Game.I.GameOver(delay2: 1f));
            GetComponent<RectTransform>().DoEndFeedback(BaseCard.DieTime);
        }
    }

    bool IsOfficerWoring;
    [SerializeField] GameObject Visual;
    void OnLevelLoaded()
    {
        Flips = 0;

        if (Level.Current.Flips > -1)
        {
            Visual.GetComponent<ProgressBar>().FillValue = 1;
            VariableFlipsText.text = "0";

            if (IsOfficerWoring == false)
            {
                Visual.SetActive(true);
                BaseCard.Shown += OnCardShown;
                IsOfficerWoring = true;
            }

            GetComponent<RectTransform>().DoStartFeedback();

            MaxFlips = Level.Current.Flips;
        }
        else
        {
            HideVisual();
        }

    }

    void HideVisual()
    {
        if (IsOfficerWoring == true)
        {
            Visual.SetActive(false);
            BaseCard.Shown -= OnCardShown;
            IsOfficerWoring = false;
        }
    }

    void OnGameOvered()
    {
        HideVisual();
    }
}
