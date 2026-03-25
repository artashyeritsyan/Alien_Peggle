using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    DataHolder instance;
    public int currentLevel;

    [Header("Player Score for Levels")]
    [SerializeField] int LevelsCount = 15;
    private int[] levelsDestroyedPegs;
    private int[] levelsBestShots;
    private float[] levelsBestTimes;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        levelsBestShots = new int[LevelsCount];
        levelsBestTimes = new float[LevelsCount];
        levelsDestroyedPegs = new int[LevelsCount];

        for (int i = 0; i < LevelsCount; ++i)
        {
            levelsBestShots[i] = 0;
            levelsBestTimes[i] = 0;
            levelsDestroyedPegs[i] = 0;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        //LoadPlayerProgress();

    }


    public string Data { get; private set; }

    // TODO: Also add here to load it from JSON files (saves)
    // and save it as well
    private void Start()
    {

    }

    public bool SetNewDestroyedPegs(int level, int newPegsCount)
    {
        if (newPegsCount < levelsDestroyedPegs[level])
        {
            levelsDestroyedPegs[level] = newPegsCount;
            return true;
        }
        return false;
    }

    public int GetLevelDestroyedPegs(int level)
    {
        Debug.Log("PegsDestrpyed: " + levelsDestroyedPegs[level]);
        return levelsDestroyedPegs[level];
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

    public void OnLevelWon()
    {
        currentLevel++;
        SavePlayerProgress();
    }

    public void LoadCurrentLevel()
    {
        // Load the level if needed
    }

    public void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("level", currentLevel);
    }

    public void LoadPlayerProgress()
    {
        currentLevel = PlayerPrefs.GetInt("level", 0);
    }
}
