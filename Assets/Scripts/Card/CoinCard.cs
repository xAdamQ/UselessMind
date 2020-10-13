using UnityEngine;

public class CoinCard : ResourceCard
{
    [SerializeField] int Amount;

    protected override void Show()
    {
        base.Show();

        GameData.I.Coins += Amount;
        GameData.Save();

        PlayParti();
    }

    protected override Vector2 TargetCanvasPoz => Game.I.CoinText.transform.position;

    void PlayParti()
    {
        var system = Ref.I.ConsumeCoinParti;

        system.transform.position = transform.position + Vector3.back;
        system.Play();
    }

}
