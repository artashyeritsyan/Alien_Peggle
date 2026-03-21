using NUnit.Framework;
using UnityEngine;

public enum Biomes
{
    plains,
    beach,
    ice,
    city,
    hell
}

[CreateAssetMenu(fileName = "LevelParams", menuName = "Scriptable Objects/LevelParams")]
public class LevelParams : ScriptableObject
{
    [SerializeField] Biomes LevelBiome;
    [SerializeField] GameObject[] spawningPeggles;

    [Header("Level Characteristics")]
    [SerializeField] int pegsCount;
    [SerializeField] int maxShots;
    [SerializeField] int shotsForStar;
    [SerializeField] int timeForStar;


    //public Biomes GetBiome() { return LevelBiome; }  // Dont need now
    public int GetBiome() { return ((int)LevelBiome); }
    public int GetPegsCount() { return pegsCount; }
    public int GetMaxShots() { return maxShots; }
    public int GetShotsForStar () { return shotsForStar; }
    public int GetTimeForStar() { return timeForStar; }

    public GameObject[] GetLevelPegs() { return spawningPeggles; }


}
