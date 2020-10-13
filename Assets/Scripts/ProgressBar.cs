using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProgressBar : MonoBehaviour
{
    [SerializeField] protected Image Fill;

    protected float fillValue;
    public float FillValue
    {
        get => fillValue;
        set
        {
            fillValue = value;

            Fill.rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                fillValue * GetComponent<RectTransform>().rect.width); //because sliced img can't be filled

        }
    }

}
