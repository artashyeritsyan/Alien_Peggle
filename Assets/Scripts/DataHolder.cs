using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    [Header("Player Score for Levels")]
    [SerializeField] int LevelsCount = 15;
    private int[] levelsBestShots;
    private float[] levelsBestTimes;

    public string Data { get; private set; }

    // TODO: Also add here to load it from JSON files (saves)
    // and save it as well
    private void Start()
    {
        levelsBestShots = new int[LevelsCount];
        levelsBestTimes = new float[LevelsCount];
    }

    public bool SetNewBestTime(int level, float newTime)
    {
        if (newTime < levelsBestTimes[level])
        {
            levelsBestTimes[level] = newTime;
            return true;
        }
        return false;
    }

    public float GetLevelBestTime(int level)
    {
        return levelsBestTimes[level];
    }

    public bool SetNewBestShot(int level, int newShotsCount)
    {
        if (newShotsCount < levelsBestShots[level])
        {
            levelsBestShots[level] = newShotsCount;
            return true;    
        }
        return false;
    }

    public int GetLevelBestShot(int level) 
    { 
        return levelsBestShots[level];
    }
}
