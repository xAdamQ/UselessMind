using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManger : MonoBehaviour
{
    public static TimeManger I;


    void Awake()
    {
        Game.I.GameOvered += OnGameOvered;
        Game.I.GameOvering += OnGameOvering;
    }

    void Start()
    {
        I = this;
        Game.I.LevelLoaded += EveryLevelIni;
        Game.OnLevDone += OnLevelPass;

    }

    static float TimeUnit = .1f;
    float FillUnit;
    Coroutine UpdateCo;
    IEnumerator UpdateTime()
    {
        while (Visual.GetComponent<ProgressCircle>().FillValue < 1)
        {
            Visual.GetComponent<ProgressCircle>().FillValue += FillUnit;
            yield return new WaitForSeconds(TimeUnit);
        }

        if (Game.I.IsLevelDone == false)
        {
            Game.I.StartCoroutine(Game.I.GameOver(delay2: 1f));
            GetComponent<RectTransform>().DoEndFeedback(1f);
        }
    }

    void EveryLevelIni()
    {
        if (Level.Current.Time > -1)
        {
            Visual.GetComponent<ProgressCircle>().FillValue = 0;
            FillUnit = TimeUnit / Level.Current.Time;
            UpdateCo = StartCoroutine(UpdateTime());

            Visual.SetActive(true);

            GetComponent<RectTransform>().DoStartFeedback();
        }
        else
        {
            Visual.SetActive(false);
        }
    }

    [SerializeField] UnityEngine.UI.Image FillImage;
    [SerializeField] GameObject Visual;

    Coroutine FreezeCo;
    public int shownCards;
    public void FreezeCard()
    {
        Visual.GetComponent<UnityEngine.UI.Image>().DoNotifyFeedback();
        shownCards++;
        if (shownCards == 1)
            FreezeCo = StartCoroutine(Freeze());
    }
    IEnumerator Freeze()
    {
        StopCoroutine(UpdateCo);

        var tmp2 = FillImage.color;
        FillImage.color = Color.cyan;

        while (shownCards != 0)
        {
            yield return new WaitForSeconds(GameData.I.FreezeAmount);
            shownCards--;
        }

        FillImage.color = tmp2;
        UpdateCo = StartCoroutine(UpdateTime());
    }

    void OnGameOvered()
    {
        Visual.SetActive(false);
    }

    void OnGameOvering()
    {
        if (FreezeCo != null)
            StopCoroutine(FreezeCo);
        if (UpdateCo != null)
            StopCoroutine(UpdateCo);
    }

    void OnLevelPass()
    {
        if (UpdateCo != null)
        {
            StopCoroutine(UpdateCo);
        }
    }
}
