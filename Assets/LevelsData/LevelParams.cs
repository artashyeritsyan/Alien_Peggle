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
    [SerializeField] System.Collections.Generic.List<GameObject> spawningPeggles;

    [Header("Level Characteristics")]
    [SerializeField] int pegsCount;
    [SerializeField] int maxShots;
    [SerializeField] int shotsForStar;
    [SerializeField] int timeForStar;


    //public Biomes GetBiome() { return LevelBiome; }
    public int GetBiome() { return ((int)LevelBiome); }
    public int GetPegsCount() { return pegsCount; }
    public int GetMaxShots() { return maxShots; }
    public int GetShotsForStars () { return shotsForStar; }
    public int GetTimeForStar() { return timeForStar; }


}
