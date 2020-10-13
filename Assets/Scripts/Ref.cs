using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ref : MonoBehaviour
{
    public static Ref I;

    void Awake()
    {
        I = this;
    }

    public ParticleSystem ConsumeCoinParti;

}
