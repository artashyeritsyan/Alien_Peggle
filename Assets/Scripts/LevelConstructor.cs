using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] PegSpawner pegSpawner;

    [SerializeField] List<Sprite> backgrounds;
    [SerializeField] GameObject backgroundObject;

    public void ConstructLevel(LevelParams levelParams)
    {
        // dnum enq backgroundy
        backgroundObject.GetComponent<SpriteRenderer>().sprite = backgrounds[levelParams.GetBiome()];
        pegSpawner.SetPegPrefabs(levelParams.GetLevelPegs());

        pegSpawner.CreateLevelWithPegCount(levelParams.GetPegsCount());
        // TODO: Call te pegSpawner to spawn the exact count of pegs we needed. (Also I can pre create the level)
        // TODO: Refactor the PegSpawner to Create pegs in exact pegs without random
    }
}
