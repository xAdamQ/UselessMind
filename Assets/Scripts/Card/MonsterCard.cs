using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCard : BaseCard
{

    bool Flipped;

    [SerializeField] Sprite WokenImg;

    protected override void Show()
    {
        base.Show();

        Hide(FlipTime + SeeTime + FlipTime);

        if (!Flipped)
        {
            Flipped = true;
        }
        else
        {
            var mainCam = Camera.main.GetComponent<CameraManger>();

            GetComponent<SpriteRenderer>().sprite = WokenImg;

            mainCam.ShakeAnim();
            Game.I.StartCoroutine(Game.I.GameOver(CameraManger.ShakeTime));
        }


    }


}
