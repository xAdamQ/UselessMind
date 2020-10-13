using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCard : MonoBehaviour
{
    bool Shown;
    static List<StartCard> All = new List<StartCard>();
    private void OnMouseDown()
    {
        if (MyLib.IsMouseOverUI())
            return;

        if (Shown == false)
        {
            iTween.RotateTo(gameObject, Vector3.zero, .5f);
            Shown = true;
            All.Add(this);
        }
        if (All.Count == 2)
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(.5f);


        transform.parent.gameObject.SetActive(false);
        foreach (var item in All)
        {
            item.Shown = false;
            item.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        All.Clear();

        Game.I.StartNewGame(GameData.I.OpenedPack, GameData.I.OpenedFlag);
    }
}
