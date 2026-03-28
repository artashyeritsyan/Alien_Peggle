using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerProgressData
{
    public int levelsCount;
    public int currentLevel;
    public bool[] isLevelCompleted;
    public int[] levelsDestroyedPegs;
    public int[] levelsBestShots;
    public float[] levelsBestTimes;
}

public class DataHolder : MonoBehaviour
{
    public static DataHolder instance;

    public int currentLevel;

    [Header("Player Score for Levels")]
    [SerializeField] int LevelsCount = 15;

    private bool[] isLevelCompleted;
    private int[] levelsDestroyedPegs;
    private int[] levelsBestShots;
    private float[] levelsBestTimes;

    private string savePath;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Application.persistentDataPath + "/player_progress.json";

        InitArrays();
        LoadPlayerProgress();
    }

    private void InitArrays()
    {
        isLevelCompleted = new bool[LevelsCount];
        levelsDestroyedPegs = new int[LevelsCount];
        levelsBestShots = new int[LevelsCount];
        levelsBestTimes = new float[LevelsCount];

        for (int i = 0; i < LevelsCount; i++)
        {
            isLevelCompleted[i] = false;
            levelsDestroyedPegs[i] = -1;
            levelsBestShots[i] = -1;
            levelsBestTimes[i] = -1;
        }
    }

    // =========================
    // GETTERS / SETTERS
    // =========================

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
        if (levelsDestroyedPegs[level] == -1 || newPegsCount < levelsDestroyedPegs[level])
        {
            levelsDestroyedPegs[level] = newPegsCount;
            return true;
        }
        return false;
    }

    public int GetLevelDestroyedPegs(int level)
    {
        return levelsDestroyedPegs[level];
    }

    public bool SetNewBestTime(int level, float newTime)
    {
        if (levelsBestTimes[level] == -1 || newTime < levelsBestTimes[level])
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
        if (levelsBestShots[level] == -1 || newShotsCount < levelsBestShots[level])
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

    public void SavePlayerProgress()
    {
        PlayerProgressData data = new PlayerProgressData
        {
            levelsCount = LevelsCount,
            currentLevel = currentLevel,
            isLevelCompleted = isLevelCompleted,
            levelsDestroyedPegs = levelsDestroyedPegs,
            levelsBestShots = levelsBestShots,
            levelsBestTimes = levelsBestTimes
        };

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("Saved to: " + savePath);
    }

    public void LoadPlayerProgress()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            PlayerProgressData data = JsonUtility.FromJson<PlayerProgressData>(json);

            LevelsCount = data.levelsCount;
            currentLevel = data.currentLevel;
            isLevelCompleted = data.isLevelCompleted;
            levelsDestroyedPegs = data.levelsDestroyedPegs;
            levelsBestShots = data.levelsBestShots;
            levelsBestTimes = data.levelsBestTimes;

            Debug.Log("Loaded from: " + savePath);
        }
        else
        {
            Debug.Log("No save file found. Using default values.");
        }
    }
}