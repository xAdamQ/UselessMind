public class StartUncoverMarket : StandardMarket
{

    protected override int TotalReqCoins => 1500;

    protected override int UpgradesCount => 10;

    protected override float IncreaseExponent => 1.4f;

    protected override int CurrentUpgrade { get => GameData.I.UncoverdOnStart; set => GameData.I.UncoverdOnStart = value; }


}
