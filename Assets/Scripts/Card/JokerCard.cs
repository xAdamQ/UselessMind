public class JokerCard : BaseCard
{

    public static readonly float ShowTime = 1.25f;

    public void ShowHideAll()
    {
        for (int x = 0; x < Game.I.AllCards.Length; x++)
        {
            for (int y = 0; y < Game.I.AllCards[x].Count; y++)
            {
                Game.I.AllCards[x][y].ShowAnim(0);
                Game.I.AllCards[x][y].HideAnim(ShowTime + FlipTime);
            }
        }
    }

    protected override void Show()
    {
        base.Show();

        Game.I.AddBlockTime(ShowTime + FlipTime);

        ShowHideAll();
        AutoDie();
    }


}
