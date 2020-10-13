using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class ResourceCard : BaseCard
{
    public static float AddtionalSeeTime => .4f;

    protected override void Show()
    {
        base.Show();

        AutoDie(AddtionalSeeTime);
    }

    protected override Sequence DieFeedback => base.DieFeedback
        .Insert(TotalDieDelay, transform.DOMove(Camera.main.ScreenToWorldPoint(TargetCanvasPoz), DieTime));

    protected abstract Vector2 TargetCanvasPoz { get; }

}
