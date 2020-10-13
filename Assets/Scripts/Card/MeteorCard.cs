using UnityEngine;
using System.Collections.Generic;
using System;

public class MeteorCard : BaseCard
{

    protected override void Show()
    {
        base.Show();

        AutoDie();

        KillClones();
    }

    void KillClones()
    {
        var ord = new List<OrdinaryCard>();
        for (int x = 0; x < Level.Current.GridSize.x; x++)
        {
            for (int y = 0; y < Game.I.AllCards[x].Count; y++)
            {
                if (Game.I.AllCards[x][y].GetComponent<OrdinaryCard>()) ord.Add(Game.I.AllCards[x][y].GetComponent<OrdinaryCard>());
            }
        }//pick all ordinary cards in game

        ord.Shuffle();

        var ordInd = 0;
        List<OrdinaryCard> exactMatch = null;
        OrdinaryCard coupleMatch = null;
        var choosen = new List<OrdinaryCard>();

        var fillMatchArr = new Action<OrdinaryCard>[]
         {

            (card) =>
            {

                //this code is asked every rand card selected to search sp if it 10,12 we know from the begining
                //and the and the marching level is based on level so it's choosen once at start

                exactMatch = ord.FindAll(everyO => everyO.Id == card.Id);
                if(card.Id == 12)
                    coupleMatch = ord.Find(everyO => everyO.Id == 10);
                else if(card.Id == 10)
                    coupleMatch = ord.Find(everyO => everyO.Id == 12);

            },

            (card) =>
            {

                exactMatch = ord.FindAll(everyO => card.Color == everyO.Color && everyO.Id == card.Id);

                if(card.Id == 12)
                    coupleMatch = ord.Find(everyO => card.Color == everyO.Color &&  everyO.Id == 10);
                else if(card.Id == 10)
                    coupleMatch = ord.Find(everyO => card.Color == everyO.Color &&  everyO.Id == 12);

            },

            (card) =>
            {

                exactMatch = ord.FindAll(everyO => card.Color == everyO.Color && everyO.Id == card.Id && card.Shape == everyO.Shape);

                if(card.Id == 12)
                    coupleMatch = ord.Find(everyO => card.Color == everyO.Color && card.Shape == everyO.Shape && everyO.Id == 10);
                else if(card.Id == 10)
                    coupleMatch = ord.Find(everyO => card.Color == everyO.Color && card.Shape == everyO.Shape && everyO.Id == 12);

            },

         };
        Action<OrdinaryCard> fillMatch = fillMatchArr[Level.Current.MatchingLevel];

        do
        {

            exactMatch = null;
            coupleMatch = null;

            if (ordInd >= ord.Count - 1)
            {
                break;
            }

            var randCard = ord[ordInd];

            fillMatch(randCard);

            ordInd++;

            if (exactMatch.Count >= Level.Current.CloneCount)
            {
                choosen.AddRange(exactMatch.GetRange(0, Level.Current.CloneCount));
                break;
            }
            else if (coupleMatch != null)
            {
                choosen.Add(randCard);
                choosen.Add(coupleMatch);
                break;
            }

        }
        while (true); //while a card fail to find enough match we pick another card

        for (int i = 0; i < choosen.Count; i++)
        {
            OrdinaryCard.Flipped.Add(choosen[i]);
            choosen[i].ShowAnim(0);
        }

        AutoKill(OrdinaryCard.Flipped.ConvertAll(x => (BaseCard)x));
        OrdinaryCard.Flipped.Clear();
    }


}

