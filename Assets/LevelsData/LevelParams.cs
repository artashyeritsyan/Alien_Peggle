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

public enum Aliens
{
    green,
    pink,
    blue,
    beige,
    yellow
}

[CreateAssetMenu(fileName = "LevelParams", menuName = "Scriptable Objects/LevelParams")]
public class LevelParams : ScriptableObject
{
    [SerializeField] int levelNumber;
    [SerializeField] Biomes levelBiome;
    [SerializeField] Aliens levelAlien;
    [SerializeField] GameObject[] spawningPeggles;

    [Header("Level Characteristics")]
    [SerializeField] int pegsCount;
    [SerializeField] int maxShots;
    [SerializeField] int shotsForStar;
    [SerializeField] int timeForStar;


    public int GetLevelNumber() {  return levelNumber; }
    //public Biomes GetBiome() { return LevelBiome; }  // Dont need now
    public int GetBiome() { return ((int)levelBiome); }
    public int GetAlien() { return ((int)levelAlien); }
    public int GetPegsCount() { return pegsCount; }
    public int GetMaxShots() { return maxShots; }
    public int GetShotsForStar () { return shotsForStar; }
    public int GetTimeForStar() { return timeForStar; }

    public GameObject[] GetLevelPegs() { return spawningPeggles; }


}
