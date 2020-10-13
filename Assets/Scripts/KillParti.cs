using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParti : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Kill());
    }

    /// <summary>
    /// make enum specify lifetime type
    /// </summary>
    IEnumerator Kill()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.startLifetime.constant);
        Destroy(gameObject);
    }
}
