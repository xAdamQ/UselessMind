using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCircle : ProgressBar
{
    new public float FillValue
    {
        get => fillValue;
        set
        {
            fillValue = value;

            Fill.fillAmount = fillValue;
        }
    }
}
