using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

public class ParticleManger : MonoBehaviour
{

    [SerializeField]
    ParticleSystem
        UpCollideSystem,
        DownSystem;

    [SerializeField]
    Sprite
        SkullSprite,
        HeartSprite;

    Sprite[] InGameSprites;
    [SerializeField] SpriteAtlas Atlas;

    void Awake()
    {
        Game.I.GameStatring += OnGameStart;
        Game.I.GameOvering += OnGameOver;
        Game.I.StartScreening += OnStartScreen;

        Game.I.LevelLoaded += OnLevelLoaded;

        InGameSprites = new Sprite[Atlas.spriteCount];
        Atlas.GetSprites(InGameSprites);
    }

    void OnGameStart()
    {
        UpCollideSystem.gameObject.SetActive(false);
    }

    void OnGameOver()
    {
        SetDownSystemSprite(SkullSprite);
    }

    void OnStartScreen()
    {
        SetDownSystemSprite(HeartSprite);
    }


    void OnLevelLoaded()
    {
        SetRandomSprite();
        var newpoz = DownSystem.transform.position;
        newpoz.y = -Camera.main.transform.position.y;
        DownSystem.transform.position = newpoz;
    }

    void SetRandomSprite()
    {
        SetDownSystemSprite(InGameSprites[UnityEngine.Random.Range(0, InGameSprites.Length)]);
    }

    void SetDownSystemSprite(Sprite sprite)
    {
        DownSystem.textureSheetAnimation.SetSprite(0, sprite);
    }

    #region deprecated

    IEnumerator SwitchSystems(ParticleSystem[] old, ParticleSystem[] @new)
    {
        var maxLifeTime = -1f;
        for (int i = 0; i < old.Length; i++)
        {
            var mainMoldule = old[i].main;
            mainMoldule.loop = false;
            mainMoldule.startSpeed = 5;
            if (mainMoldule.startLifetime.constant > maxLifeTime) maxLifeTime = mainMoldule.startLifetime.constant;

        }//disable loop
        yield return new WaitForSeconds(maxLifeTime);//wait until particles end
        for (int i = 0; i < old.Length; i++)
        {
            var mainMoldule = old[i].main;
            old[i].gameObject.SetActive(false);
            mainMoldule.loop = true;
        }//diactivate them and set loop true
        for (int i = 0; i < @new.Length; i++)
        {
            @new[i].gameObject.SetActive(true);
        }//activate new systems
    }
    IEnumerator SwitchSystems(ParticleSystem old, ParticleSystem @new)
    {
        var mainMoldule = old.main;
        mainMoldule.loop = false;
        yield return new WaitForSeconds(mainMoldule.startLifetime.constant);
        old.gameObject.SetActive(false);
        mainMoldule.loop = true;

        @new.gameObject.SetActive(true);
    }
    IEnumerator DisabelSystem(ParticleSystem system)
    {
        var mainMoldule = system.main;
        mainMoldule.loop = false;
        yield return new WaitForSeconds(mainMoldule.startLifetime.constant);
        system.gameObject.SetActive(false);
        mainMoldule.loop = true;

    }

    #endregion

}
