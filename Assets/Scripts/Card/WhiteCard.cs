using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCard : BaseCard
{
    protected override void Show()
    {
        base.Show();

        AutoDie();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
