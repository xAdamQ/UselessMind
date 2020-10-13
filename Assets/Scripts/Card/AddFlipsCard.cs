using UnityEngine;

public class AddFlipsCard : ResourceCard
{
    protected override Vector2 TargetCanvasPoz => FlipsManger.I.transform.position;
    protected override void Show()
    {
        base.Show();

        FlipsManger.I.MaxFlips += GameData.I.ExtraFlipAmount;
    }


}
