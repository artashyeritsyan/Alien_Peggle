using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] PegSpawner pegSpawner;

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

        if (levelParams.GetPatternParent() != null)
        {
            pegSpawner.CreateLevelByPattern(levelParams.GetPatternParent());
        }
        else
        {
            pegSpawner.CreateLevelWithPegCount(levelParams.GetPegsCount());
        }

        //switch (levelParams.GetBiome()) 
        //{
        //    // if biome 2-nd (water biome)
        //    case 0:
        //        if (levelParams.GetLevelId() % 3 == 1)
        //        {
        //            pegSpawner.CreateLevelByPattern(patternParents[2]);
        //        }
        //        else if (levelParams.GetLevelId() % 3 == 2)
        //        {
        //            pegSpawner.CreateLevelByPattern(patternParents[3]);
        //        }
        //        else
        //        {
        //            pegSpawner.CreateLevelWithPegCount(levelParams.GetPegsCount());
        //        }
        //        break;
        //    case 2:
        //        // Just a temporary solution. // TODO: make the Level params map strucuture, Handle this parts more clear
        //        if (levelParams.GetLevelId() % 3 == 1)
        //        {
        //            pegSpawner.CreateLevelByPattern(patternParents[0]);
        //        }
        //        else if(levelParams.GetLevelId() % 3 == 2)
        //        {
        //            pegSpawner.CreateLevelByPattern(patternParents[1]);
        //        }
        //        else
        //        {
        //            pegSpawner.CreateLevelWithPegCount(levelParams.GetPegsCount());
        //        }
        //        break;

        //    default:
        //        pegSpawner.CreateLevelWithPegCount(levelParams.GetPegsCount());
        //        break;
        //}

        //pegSpawner.CreateLevelWithPegCount(levelParams.GetPegsCount());

        //for (int i = 0; i < pegFigures.positions.Length; i++)
        //{
        //    //Instantiate()
        //}
        // TODO: Call te pegSpawner to spawn the exact count of pegs we needed. (Also I can pre create the level)
        // TODO: Refactor the PegSpawner to Create pegs in exact pegs without random
    }

    
}
