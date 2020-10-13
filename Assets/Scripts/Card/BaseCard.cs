using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public abstract class BaseCard : MonoBehaviour
{
    public static float FlipTime => .2f;
    public static float SeeTime => .25f;
    public static float DieTime => .3f;
    public readonly static Vector2 Size = new Vector2(1.42f, 1.92f);
    readonly static float Spacing = .2f;
    public Vector2Int Index;
    public static readonly Vector2 TotalSize = new Vector2(Size.x + Spacing, Size.y + Spacing);

    public static event System.Action Showing, Shown;

    public void SetAndAnimateIndex(Vector2Int ind)
    {
        Index = ind;
        AnimateToCurrentIndex(0);
    }

    public void AnimateToCurrentIndex(float delay)
    {
        var convertedInd = IndConversation[(int)Level.Current.GravityDir](Index);
        var poz = new Vector3(convertedInd.x * TotalSize.x, convertedInd.y * TotalSize.y);
        transform.DOLocalMove(poz, .3f).SetDelay(delay);
    }

    static System.Func<Vector2Int, Vector2Int>[] IndConversation = new System.Func<Vector2Int, Vector2Int>[]{
        v => v,
        v => v,
        v => new Vector2Int(v.x, Level.Current.GridSize.y - 1 - v.y),
        };

    public void ShowAnim(float delay = 0)
    {
        DOTween.Sequence().
            AppendInterval(delay).
            Append(transform.DOLocalRotate(new Vector3(0, 0, 0), FlipTime));
    }
    public void HideAnim(float delay = 0)
    {
        DOTween.Sequence().
            AppendInterval(delay).
            Append(transform.DOLocalRotate(new Vector3(0, 180, 0), FlipTime)).
            AppendCallback(() => IsShown = false);
    }

    bool IsShown;
    void OnMouseDown()
    {
        if (Game.I.ClickAvai)
        {
            if (IsShown)
            {
                Hide(0);
            }
            else
            {
                Show();
                Shown?.Invoke();
            }
        }
    }

    protected virtual void Show()
    {
        Showing?.Invoke();

        IsShown = true;
        ShowAnim();

        Game.I.AddBlockTime(FlipTime);

        if (GetComponent<OrdinaryCard>() == false)
        {
            while (OrdinaryCard.Flipped.Count != 0)
            {
                OrdinaryCard.Flipped[0].Hide(0);
            }
        }

    }
    protected virtual void Hide(float animWait)
    {
        HideAnim(animWait);
    }

    protected void UpdateAffectedIndices()
    {
        for (int y = Index.y + 1; y < Game.I.AllCards[Index.x].Count; y++)
        {
            var curCard = Game.I.AllCards[Index.x][y];
            curCard.Index += Vector2Int.down;

            if (Affected.Contains(curCard) == false)
                Affected.Add(curCard);
        }
    }

    protected static void AnimateAffectedByGravity(float additionalDelay = 0)
    {
        foreach (var item in Affected) item.AnimateToCurrentIndex(FlipTime + SeeTime + additionalDelay);
    }

    static int CardsCount;
    static IEnumerator CheckLevelDone()
    {
        if (CardsCount <= Level.Current.monsterCount)
        {
            Game.I.StartCoroutine(Game.I.LevelDone());//set level done flag to true before anything

            if (Level.Current.monsterCount > 0)
            {
                for (int x = 0; x < Game.I.AllCards.Length; x++) for (int y = 0; y < Game.I.AllCards[x].Count; y++)
                        Game.I.AllCards[x][y].ShowAnim(0);

                Game.I.AddBlockTime(FlipTime + SeeTime);
                yield return new WaitForSeconds(FlipTime + SeeTime);
            }
        }
    }

    #region die
    protected static List<BaseCard> Affected = new List<BaseCard>();

    // all next funs call this
    void AnimatedDestroy()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        DieFeedback.Play();

        Destroy(transform.GetChild(0).gameObject);
        Destroy(gameObject, TotalDieDelay + DieTime);
    }

    protected virtual Sequence DieFeedback => DOTween.Sequence()
        .AppendInterval(TotalDieDelay)
        .Append(GetComponent<SpriteRenderer>().DOFade(.2f, DieTime))
        .Join(GetComponent<SpriteRenderer>().transform.DOScale(Vector3.one * .2f, DieTime))
        .InsertCallback(TotalDieDelay * .8f, () => Instantiate(ExpParti, transform.position + Vector3.back, new Quaternion()));

    static ParticleSystem ExpParti;

    protected float TotalDieDelay;
    void EssDie(float AdditionalDelay = 0)
    {
        UpdateAffectedIndices();
        Game.I.AllCards[Index.x].RemoveAt(Index.y);

        TotalDieDelay = FlipTime + SeeTime + AdditionalDelay;
        AnimatedDestroy();

        CardsCount--;
    }
    void GravityDie(float AdditionalDelay)
    {
        EssDie(AdditionalDelay);
        AnimateAffectedByGravity(AdditionalDelay);
    }
    protected void AutoDie(float AdditionalDelay = 0)
    {
        if (Level.Current.GravityDir != GravityDir.NON)
            GravityDie(AdditionalDelay);
        else
            EssDie(AdditionalDelay);

        Affected.Clear();
        Game.I.StartCoroutine(CheckLevelDone());
    }

    static void SimpleKill(List<BaseCard> died)
    {
        for (int i = 0; i < died.Count; i++)
        {
            died[i].EssDie();
        }
    }
    static void GravityKill(List<BaseCard> died)
    {
        for (int i = 0; i < died.Count; i++)
        {
            died[i].EssDie();
        }
        AnimateAffectedByGravity();
    }
    protected static void AutoKill(List<BaseCard> died)
    {

        //void debugGame()
        //{
        //    var debuText = "";
        //    var allCards = Game.I.AllCards;
        //    for (int x = 0; x < allCards.Length; x++)
        //    {
        //        for (int y = 0; y < allCards[x].Count; y++)
        //        {
        //            debuText += allCards[x][y].Index.ToString(); //+ allCards[x][y].GetComponent<OrdinaryCard>().Id + " ";
        //        }
        //        debuText += '\n';
        //    }
        //    Debug.Log(debuText);
        //}
        //debugGame();

        died = died.OrderByDescending(o => o.Index.y).ToList();

        if (Level.Current.GravityDir != GravityDir.NON)
            GravityKill(died);
        else
            SimpleKill(died);

        Affected.Clear();
        Game.I.StartCoroutine(CheckLevelDone());
    }
    #endregion

    public static void DestroyAllCards()
    {
        OrdinaryCard.Flipped.Clear();
        for (int x = 0; x < Game.I.AllCards.Length; x++)
        {
            for (int y = 0; y < Game.I.AllCards[x].Count; y++)
            {
                Game.I.AllCards[x][y].AnimatedDestroy();
            }
            Game.I.AllCards[x].Clear();
        }
    }

    public static void OneTimeIni()
    {
        Game.I.LevelLoaded += EveryLevelIni;

        ExpParti = Resources.Load<ParticleSystem>("Parti/Exp");
    }

    public static void EveryLevelIni()
    {
        CardsCount = Level.Current.GridSize.x * Level.Current.GridSize.y;

        //ColumnIndices = new int[Level.Current.GridSize.x];
        //for (int i = 0; i < Level.Current.GridSize.x; i++)
        //    ColumnIndices[i] = i;

        //Game.I.StartCoroutine(Solver());
    }

    //static int[] ColumnIndices;
    //static Vector2Int prev;
    //static IEnumerator Solver()
    //{
    //    ColumnIndices.Shuffle();
    //    var xRand = 0;
    //    while (Game.I.AllCards[ColumnIndices[xRand]].Count == 0)
    //    {
    //        xRand++;
    //        if (xRand == ColumnIndices.Length)
    //        {
    //            yield break;
    //        }
    //    }

    //    var yRand = Random.Range(0, Game.I.AllCards[ColumnIndices[xRand]].Count);

    //    Debug.Log(Game.I.AllCards[ColumnIndices[xRand]][yRand].Index);
    //    if (prev == new Vector2Int(xRand, yRand))
    //    {
    //        Debug.Log("aa");
    //        goto skip;
    //    }

    //    prev = new Vector2Int(xRand, yRand);
    //    Game.I.AllCards[ColumnIndices[xRand]][yRand].Show();

    //    skip:
    //    yield return new WaitForSeconds(.3f);

    //    Game.I.StartCoroutine(Solver());
    //}
}
