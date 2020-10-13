using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPartiSys : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<ParticleSystem>().Stop();
    }
}
