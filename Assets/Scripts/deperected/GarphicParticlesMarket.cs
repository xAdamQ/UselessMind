using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarphicParticlesMarket : MonoBehaviour
{

    void Start()
    {
        Refresh();
        XpLevelManger.I.LevelUping += Refresh;
    }

    void Refresh()
    {

    }


}
