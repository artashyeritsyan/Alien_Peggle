using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public enum Biomes
{
    Electro,
    Lava,
    Water,
    Rock,
    Purple,
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
    // The IDs must be unique. // later make it auto incremental (if creates more levels) 
    [SerializeField] int levelId;
    [SerializeField] Biomes levelBiome;
    [SerializeField] Aliens levelAlien;
    [SerializeField] GameObject[] spawningPeggles;
    [SerializeField] Transform patternParent;

    [Header("Level Characteristics")]
    [SerializeField] int pegsCount;
    [SerializeField] int maxShots;
    [SerializeField] int shotsForStar;
    [SerializeField] int timeForStar;

    private void Awake()
    {
        if (patternParent)
        {
            pegsCount = patternParent.childCount;
        }
    }

    public int GetLevelId() {  return levelId; }
    //public Biomes GetBiome() { return LevelBiome; }  // Dont need now
    public int GetBiome() { return ((int)levelBiome); }
    public int GetAlien() { return ((int)levelAlien); }
    public int GetPegsCount() { return pegsCount; }

    public Transform GetPatternParent() { return patternParent; }

    public int GetMaxShots() { return maxShots; }
    public int GetShotsForStar () { return shotsForStar; }
    public int GetTimeForStar() { return timeForStar; }

    public GameObject[] GetLevelPegs() { return spawningPeggles; }


}
