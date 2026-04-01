using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] PegSpawner pegSpawner;
    [SerializeField] PegFigures pegFigures;

    [Header("Backgorund")]
    [SerializeField] List<Sprite> backgrounds;
    [SerializeField] GameObject backgroundObject;
    [SerializeField] AudioSource bgAudioSource;
    [SerializeField] List<AudioClip> levelsBgMusic;

    [Header("Aliens")] 
    [SerializeField] List<Sprite> aliens;
    [SerializeField] List<Sprite> lasers;
    [SerializeField] List<Sprite> halos;
    [SerializeField] GameObject alienObject;
    [SerializeField] GameObject alienShadowObj;
    [SerializeField] GameObject laserObject;
    [SerializeField] GameObject haloObject;

    public void ConstructLevel(LevelParams levelParams)
    {
        int alienIndex = levelParams.GetAlien();
        alienObject.GetComponent<SpriteRenderer>().sprite = aliens[alienIndex];
        alienShadowObj.GetComponent<SpriteRenderer>().sprite = aliens[alienIndex];
        laserObject.GetComponent<SpriteRenderer>().sprite = lasers[alienIndex];
        haloObject.GetComponent<SpriteRenderer>().sprite = halos[alienIndex];

        bgAudioSource.Stop();
        bgAudioSource.clip = levelsBgMusic[levelParams.GetBiome()];
        bgAudioSource.Play();

        // dnum enq backgroundy
        backgroundObject.GetComponent<SpriteRenderer>().sprite = backgrounds[levelParams.GetBiome()];
        pegSpawner.SetPegPrefabs(levelParams.GetLevelPegs());

        pegSpawner.CreateLevelWithPegCount(levelParams.GetPegsCount());

        for (int i = 0; i < pegFigures.positions.Length; i++)
        {
            //Instantiate()
        }
        // TODO: Call te pegSpawner to spawn the exact count of pegs we needed. (Also I can pre create the level)
        // TODO: Refactor the PegSpawner to Create pegs in exact pegs without random
    }
}
