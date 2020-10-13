public class HeartMarket : StandardMarket
{
    protected override int TotalReqCoins => 2000;

    protected override int UpgradesCount => 3;

    protected override float IncreaseExponent => 1.7f;

    protected override int CurrentUpgrade { get => GameData.I.Lifes; set => GameData.I.Lifes = value; }
}
