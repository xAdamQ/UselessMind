using UnityEngine;
using System.Collections.Generic;

public delegate T Func<T>();

public class OrdinaryCard : BaseCard
{
    public int Id { get; private set; }
    static readonly int[] ShapeToColor = new int[] { 0, 1, 1, 0 };

    public int Color, Shape;
    public void SetData(int longIndex)
    {
        Shape = longIndex / 13; //int division
        Color = ShapeToColor[Shape];
        Id = longIndex % 13;
        GetComponent<SpriteRenderer>().sprite = OrdinarySprites[longIndex];
    }


    public static Func<bool> IsMatching;
    static Func<bool>[] MatchingLevel = new Func<bool>[] { IdMatch, ColorMatch, ShapeMatch };
    public static bool IdMatch()
    {
        var matching = true;
        for (int i = 1; i < Flipped.Count; i++)
        {
            if (Flipped[0].Id != Flipped[i].Id)
            {
                matching = false;
            }
        }

        if (matching == false && Flipped.Count == 2) //couple match
        {
            if ((Flipped[0].Id == 10 || Flipped[0].Id == 12) && (Flipped[1].Id == 10 || Flipped[1].Id == 12))
            {
                matching = true;
            }
        }

        return matching;
    }
    public static bool ColorMatch()
    {
        var matching = true;
        for (int i = 1; i < Flipped.Count; i++)
        {
            if (Flipped[0].Color != Flipped[i].Color)
            {
                matching = false;
            }
        }
        return matching && IdMatch();
    }
    public static bool ShapeMatch()
    {
        var matching = true;
        for (int i = 1; i < Flipped.Count; i++)
        {
            if (Flipped[0].Shape != Flipped[i].Shape)
            {
                matching = false;
            }
        }
        return matching && IdMatch();
    }


    static Sprite[] OrdinarySprites;

    public static List<OrdinaryCard> Flipped = new List<OrdinaryCard>();
    protected override void Show()
    {
        base.Show();

        if (Game.I.IsOver)
        {
            Debug.Log("GOver");
            return;
        }

        Flipped.Add(this);

        if (Flipped.Count == Level.Current.CloneCount) //reached clone count
        {
            if (IsMatching()) //is matching
            {
                AutoKill(Flipped.ConvertAll(x => (BaseCard)x));
            }
            else //flip back
            {
                FlipBackFlipped();
            }

            Flipped.Clear();
        }

    }

    protected override void Hide(float animWait)
    {
        base.Hide(animWait);
        Flipped.Remove(this);
    }

    static void FlipBackFlipped()
    {
        while (Flipped.Count != 0)
        {
            Flipped[0].Hide(SeeTime);
        }
    }

    new public static void OneTimeIni()
    {
        OrdinarySprites = Resources.LoadAll<Sprite>("Sprites/nums3");

        Game.I.LevelLoaded += () => { IsMatching = MatchingLevel[Level.Current.MatchingLevel]; };
    }
}
