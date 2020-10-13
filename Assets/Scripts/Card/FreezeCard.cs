using UnityEngine;

public class FreezeCard : ResourceCard
{
    protected override Vector2 TargetCanvasPoz => TimeManger.I.transform.position;

    protected override void Show()
    {
        base.Show();

        TimeManger.I.FreezeCard();

    }
}
