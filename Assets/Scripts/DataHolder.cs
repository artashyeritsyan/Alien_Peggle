using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    DataHolder instance;
    public int currentLevel;

    [Header("Player Score for Levels")]
    [SerializeField] int LevelsCount = 15;
    private bool[] isLevelCompleted;
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

        isLevelCompleted = new bool[LevelsCount];
        levelsBestShots = new int[LevelsCount];
        levelsBestTimes = new float[LevelsCount];
        levelsDestroyedPegs = new int[LevelsCount];

        //for (int i = 0; i < LevelsCount; ++i)
        //{
        //    isLevelCompleted[i] = false;
        //    levelsBestShots[i] = -1;
        //    levelsBestTimes[i] = -1;
        //    levelsDestroyedPegs[i] = -1;
        //}

        instance = this;
        DontDestroyOnLoad(gameObject);
        //LoadPlayerProgress();

    }


    public string Data { get; private set; }

    // TODO: Also add here to load it from JSON files (saves)
    // and save it as well
    private void Start()
    {
        //PlayerPrefs.DeleteAll(); // To remove saves
    }

    public void SetLevelCompleted(int level)
    {
        isLevelCompleted[level] = true;
    }

    public bool GetIsLevelCompleted(int level)
    {
        return isLevelCompleted[level];
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
        Debug.Log("Saving levelsCount: " + currentLevel);
        PlayerPrefs.SetInt("levelsCount", LevelsCount);
        Debug.Log("Saving 1LevelTime: " + levelsBestTimes[0]);
        PlayerPrefs.SetFloat("1LevelTime", levelsBestTimes[0]);
        Debug.Log("Saving 1LevelShots: " + levelsBestShots[0]);
        PlayerPrefs.SetInt("1LevelShots", levelsBestShots[0]);
        //levelsBestTimes
        //Application.persistentDataPath    DATA SAVING PATH IN EVERY DEVICE
        // TODO: Make it save into Json file and save here
    }

    public void LoadPlayerProgress()
    {
        currentLevel = PlayerPrefs.GetInt("levelsCount", 0);
        levelsBestTimes[0] = PlayerPrefs.GetFloat("1LevelTime", -1);
        levelsBestShots[0] = PlayerPrefs.GetInt("1LevelShots", -1);

    }
}
