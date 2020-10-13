using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class MyLib
{
    #region project specific
    public static RectTransform MainCanvas;

    public static void Ini(RectTransform mainCanvas)
    {
        MainCanvas = mainCanvas;
    }
    #endregion

    public static void Shuffle<T>(this IList<T> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            var temp = List[i];
            int randomIndex = Random.Range(i, List.Count);
            List[i] = List[randomIndex];
            List[randomIndex] = temp;
        }
    }

    #region random
    public static int BiasedRandom(int min, int max, float amount, bool towardMax = true)
    {
        max--; min++;

        var rand = towardMax ? 1 - Mathf.Pow(Random.Range(0f, 1f), 1f / amount) : Mathf.Pow(Random.Range(0f, 1f), 1f / amount);

        return (int)Mathf.Floor((rand * (1 + max - min)) + min);
    }
    public static int BiasedRandom(BiasedRandomInRange BRR)
    {
        var rand = BRR.TowardMax ?
            Mathf.Pow(Random.Range(0f, 1f), 1f / BRR.AmountOfBais) :
            1 - Mathf.Pow(Random.Range(0f, 1f), 1f / BRR.AmountOfBais);

        return (int)Mathf.Floor((rand * (1 + BRR.Range.y - BRR.Range.x)) + BRR.Range.x);
    }
    public static float BiasedRandom(BiasedRandomInFloatRange BRR)
    {
        var rand = BRR.TowardMax ?
            Mathf.Pow(Random.Range(0f, 1f), 1f / BRR.AmountOfBais) :
            1 - Mathf.Pow(Random.Range(0f, 1f), 1f / BRR.AmountOfBais);

        return (rand * (BRR.Range.y - BRR.Range.x)) + BRR.Range.x;
    }
    public static int SlicedRandom(float[] poss)
    {
        var rand = Random.Range(0f, 1f);
        var totalPoss = 0f;

        for (int i = 0; i < poss.Length; i++)
        {
            totalPoss += poss[i];
            if (rand < totalPoss)
            {
                return i;
            }
        }

        return -1;
    }
    ///<summary>
    /// takes array of poss, based on it it return random num (poss sum doesn't have to be 1)
    ///</summary>
    public static int SumRandom(IList<float> poss)
    {
        var possSum = 0f;
        for (int i = 0; i < poss.Count; i++)
        {
            possSum += poss[i];
        }

        var rand = Random.Range(0f, possSum);

        var addedPoss = 0f;
        for (int i = 0; i < poss.Count; i++)
        {
            addedPoss += poss[i];
            if (rand < addedPoss)
            {
                return i;
            }
        }

        return -1;
    }
    //my fair random
    public static float PeriodsRandom(List<Period> periods)
    {
        var totalDelta = 0f;
        for (int i = 0; i < periods.Count; i++)
            totalDelta += periods[i].Delta;

        var peroidPoss = new float[periods.Count];
        for (int i = 0; i < periods.Count; i++)
            peroidPoss[i] = periods[i].Delta / totalDelta;

        var choosenPeriod = periods[SlicedRandom(peroidPoss)];

        return Random.Range(choosenPeriod.Start, choosenPeriod.End);
    }
    #endregion

    /// <summary>
    /// casted to int
    /// </summary>
    public static int GetExpResult(float coefficient, float exponent, int @base)
    {
        return (int)(coefficient * Mathf.Pow(@base, exponent));
    }
    public static int GetBaseOfExp(float coefficient, float exponent, int result)
    {
        return (int)Mathf.Pow((result / coefficient), 1 / exponent);
    }

    public static bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    #region Feedback Anim

    public static void DoNotifyFeedback(this Image image)
    {
        image.transform.DOScale(image.transform.localScale * 1.4f, 1).From();
        image.DOColor(Color.red, 1f).From();
    }
    public static void DoStartFeedback(this RectTransform transform)
    {
        transform.DOLocalMove(Vector3.zero, 1.5f).From();
    }
    public static void DoEndFeedback(this RectTransform transform, float delay)
    {
        var defualtPoz = transform.position;

        DOTween.Sequence()
            .AppendInterval(delay)
            .Append(transform.DOLocalMove(PozAsCenterPivot(transform, Vector3.zero), 1f))
            .AppendCallback(() => { transform.position = defualtPoz; transform.localScale = Vector3.one; }).SetEase(Ease.OutExpo);
    }

    #endregion

    /// <summary>
    /// edit the fun if you use scale more then 1
    /// </summary>
    public static Vector2 PozAsCenterPivot(RectTransform rectTransform, Vector2 poz)
    {
        return new Vector2
            (
            ((rectTransform.pivot.x - .5f) * rectTransform.sizeDelta.x) + poz.x
            ,
            ((rectTransform.pivot.y - .5f) * rectTransform.sizeDelta.y) + poz.y
            );
    }

    #region Exp slice
    /// <summary>
    /// Makes an array of the big value sliced exponentially
    /// </summary>
    public static void ExponentialSlice(ref float[] resultArray, int arraySize, float startValue, float endValue, float exponent)
    {
        resultArray = new float[arraySize];

        float n = Mathf.Pow(exponent, arraySize) - 1;
        float d = exponent - 1;
        float t = (endValue - startValue) / (n / d);

        for (int x = 0; x < arraySize; x++)
        {
            float interval = t * Mathf.Pow(exponent, x);
            resultArray[x] = interval;
        }
    }
    /// <summary>
    /// just like above but make int array
    /// </summary>
    public static void ExponentialSlice(ref int[] resultArray, int arraySize, int endValue, float exponent, int startValue = 0)
    {
        resultArray = new int[arraySize];

        float n = Mathf.Pow(exponent, arraySize) - 1;
        float d = exponent - 1;
        float t = (endValue - startValue) / (n / d);

        for (int x = 0; x < arraySize; x++)
        {
            float interval = t * Mathf.Pow(exponent, x);
            resultArray[x] = (int)interval;
        }

    }
    /// <summary>
    /// fisrt assume that you made an array with "ExponentialSlice" and you get the element at this index
    /// </summary>
    public static int ExponantialSliceAtInd(int arraySize, float exponent, int totalValue, int Ind)
    {
        float n = Mathf.Pow(exponent, arraySize) - 1;
        float d = exponent - 1;
        float t = totalValue / (n / d);

        return (int)(t * Mathf.Pow(exponent, Ind));
    }
    /// <summary>
    /// assume you made an array by "ExponentialSlice" and you get the sum of element from element 0 to your index
    /// </summary>
    public static float ExponentialSumAtInd(int arraySize, float exponent, int Ind, float startValue, float endValue)
    {
        float n = Mathf.Pow(exponent, arraySize) - 1;
        float d = exponent - 1;
        float t = (endValue - startValue) / (n / d);

        float result = startValue;
        for (int x = 0; x <= Ind; x++)
        {
            result += t * Mathf.Pow(exponent, x);
        }

        return result;
    }
    #endregion

    public static void PlayParti(this Transform transform, ParticleSystem sys)
    {

        sys.transform.position = transform.position + Vector3.back;

        sys.Play();

    }

}

[System.Serializable]
public struct Period : System.IComparable
{
    public float Start, End, Delta;
    public Period(float start, float end)
    {
        Start = start;
        End = end;
        Delta = End - Start;
    }

    public int CompareTo(object obj)
    {
        var other = (Period)obj;
        return Delta.CompareTo(other.Delta);
    }//no null handling as struct can't be null

    public override string ToString()
    {
        return Start + " to " + End;
    }

    public static Period operator +(Period period, float floatVal)
    {
        return new Period(period.Start + floatVal, period.End + floatVal);
    }
    public static Period operator +(float floatVal, Period period)
    {
        return period + floatVal;
    }




}

[System.Serializable]
public struct BiasedRandomInRange
{
    public Vector2Int Range;
    public float AmountOfBais;
    public bool TowardMax;
}
[System.Serializable]
public struct BiasedRandomInFloatRange
{
    public Vector2 Range;
    public float AmountOfBais;
    public bool TowardMax;
}
