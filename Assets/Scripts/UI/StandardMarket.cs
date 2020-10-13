using UnityEngine;
using UnityEngine.UI;

public abstract class StandardMarket : MonoBehaviour
{
    void Start()
    {
        SetUI();
    }

    [SerializeField] Text Amount, ReqCoinsText;
    [SerializeField] Button BuyButton;

    protected abstract int TotalReqCoins { get; }
    protected abstract int UpgradesCount { get; }
    protected abstract float IncreaseExponent { get; }
    protected abstract int CurrentUpgrade { get; set; }
    //protected virtual float UpgradeAmount => 1f;

    int CurrentReqCoins { get; set; }

    void SetUI()
    {
        if (CurrentUpgrade < UpgradesCount)
        {
            CurrentReqCoins = MyLib.ExponantialSliceAtInd(UpgradesCount, IncreaseExponent, TotalReqCoins, CurrentUpgrade);
            ReqCoinsText.text = CurrentReqCoins.ToString();
        }
        else
        {
            Destroy(BuyButton.gameObject);
        }

        //var cursor = Caption.text.Length - 1;
        //while (Caption.text[cursor] != ' ') //keep the last of text space+anyChar to make it work
        //    cursor--;
        //Caption.text = Caption.text.Remove(cursor + 1) + CurrentUpgrade;

        Amount.text = CurrentUpgrade.ToString();
    }

    public void Buy()
    {
        if (GameData.I.Coins < CurrentReqCoins)
            return;

        //CurrentUpgrade += UpgradeAmount;
        CurrentUpgrade++;

        GameData.I.Coins -= CurrentReqCoins;
        GameData.Save();

        SetUI();
    }
}
