using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static event Action<bool> IsGamePaused;

    // ====================== UI Part ===========================
    [Header("Game Panels")]
    [SerializeField] GameObject gameOverlay;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameWinPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject levelsPanel;

    [Header("Overlay texts")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI shotsText;
    [SerializeField] TextMeshProUGUI timeText;
    public float maxTimeForStar;  // TODO: need to get roundTIme from every round (and make private). But now gets from inspector
    private float currentTime;

    [Header("Level Buttons Parameters")]
    [SerializeField] int levelsCount = 15;
    [SerializeField] Transform levelsLayout;
    [SerializeField] GameObject levelButtonPrefab;
    private List<GameObject> levelButtons;

    [Header("Star Sprites")]
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] Sprite filledStarSprite;
    // ====================== UI Part End ===========================


    [SerializeField] int maxShotsCount = 5;
    private int shotsLeft;
    private int destroyedBallsCount;

    // rewrite this part
    [SerializeField] PegSpawner spawner;
    private int maxPegsCount;
    private int destroyedPegsCount;
    private bool isBallInGame;


    private bool isGamePaused;

    [Header("Levels Parameters And Construction")]
    [SerializeField] LevelConstructor levelConstructor;
    [SerializeField] List<LevelParams> levelsParams;
    private int currentLevelIdx;

    [Header("Levels Confirming Panel")]
    [SerializeField] GameObject levelConfirmingPanel;
    [SerializeField] GameObject levelInfoPanel;
    [SerializeField] TextMeshProUGUI levelNumberText;
    [SerializeField] TextMeshProUGUI pegsCountInfoText;
    [SerializeField] TextMeshProUGUI timeInfoText;
    [SerializeField] TextMeshProUGUI shotsInfoText;
    [SerializeField] Button levelStartButton;

    [SerializeField] DataHolder dataHolder;

    [SerializeField] AudioSource clickSound;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

    }

    void Start()
    {
        PauseGame(true);

        levelButtons = new List<GameObject>();
        // TODO: Create DataHolder script to save all info about levels
        CreateLevelButtons();
        //savedData = FindFirstObjectByType<DataHolder>(); 

        DisableAllPanels();
        menuPanel.SetActive(true);

        destroyedBallsCount = 0;
        isBallInGame = false;
        shotsLeft = maxShotsCount;
        UpdateScoreText();
        UpdateShotsText();
    }

    private void Update()
    {
        if (!isGamePaused)
        {
            currentTime += Time.deltaTime;

            UpdateTimerUI();
        }
    }

    private void OnEnable()
    {
        Ball.OnPointCollected += AddScore;
        Ball.OnBallDestroyed += BallDestroyed;
        BallSpawner.OnBallShot += BallShot;
    }

    private void OnDisable()
    {
        Ball.OnPointCollected -= AddScore;
        Ball.OnBallDestroyed -= BallDestroyed;
        BallSpawner.OnBallShot -= BallShot;
    }

    void BallShot()
    {
        if (shotsLeft > 0)
        {
            shotsLeft--;
        }
        isBallInGame = true;
        UpdateShotsText();
    }

    void UpdateShotsText()
    {
        shotsText.text = "Shots: " + shotsLeft + "/" + maxShotsCount;
    }

    void CheckIfWin()
    {
        if (maxPegsCount == destroyedPegsCount)
        {
            GameWin();
        }
    }

    void DisableAllPanels()
    {
        gameOverlay.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        menuPanel.SetActive(false);
        levelsPanel.SetActive(false);
        levelConfirmingPanel.SetActive(false);
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        float t = currentTime / maxTimeForStar;
        //timeBar.color = Color.Lerp(Color.red, new Color32(22, 187, 121, 255), t);
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + destroyedPegsCount + "/" + maxPegsCount;
    }

    void AddScore()
    {
        ++destroyedPegsCount;
        UpdateScoreText();
        CheckIfWin();
    }

    void BallDestroyed ()
    {
        destroyedBallsCount++;
        isBallInGame = false;
        //CheckIfWin();
        CheckIsGameLoose();
    }

    public bool CanShoot()
    {
        return !isBallInGame;
    }

    void CheckIsGameLoose()
    {
        if (shotsLeft <= 0 && destroyedBallsCount >= maxShotsCount)
        {
            if (isGamePaused) { return; }
            GameOver();
        }
    }

    void GameWin()
    {
        DisableAllPanels();
        gameWinPanel.SetActive(true);
        totalScoreText.text = "Total Score: " + destroyedPegsCount + "/" + maxPegsCount;

        UpdateNewScores();
        CheckIfStarRequired();

        // TODO: Add here to set why the stars are added, 1 from this 3 (Win, Time, Shots)
        Debug.Log("Start fo Win");
        AddStar(currentLevelIdx);


        PauseGame(true);
    }

    void GameOver()
    {
        DisableAllPanels();
        gameOverPanel.SetActive(true);
        totalScoreText.text = "Total Score: " + destroyedPegsCount + "/" + maxPegsCount;

        PauseGame(true);
    }

    public void NextLevel()
    {
        if (currentLevelIdx == levelsParams.Count - 1)
        {
            return;
        }
        else
        {
            currentLevelIdx++;
            StartGame(currentLevelIdx);
        }
    }

    public void Restart()
    {
        // No different logic yet
        StartGame(currentLevelIdx);
    }

    public void StartGame(int levelIndex)
    {
        ClearLevel();
        PrepareLevel(levelIndex);
        //spawner.CreateLevel();
        //maxPegsCount = spawner.GetPegsCount();
        destroyedPegsCount = 0;

        shotsLeft = maxShotsCount;
        destroyedBallsCount = 0;
        isBallInGame = false;

        currentTime = 0;

        // UI part
        DisableAllPanels();
        gameOverlay.SetActive(true);
        UpdateScoreText();
        UpdateShotsText();
        UpdateTimerUI();

        PauseGame(false);
    }


    public int GetLeftShotsCount()
    {
        return shotsLeft;
    }

    public void PauseGame(bool isPaused)
    {
        isGamePaused = isPaused;
        IsGamePaused?.Invoke(isPaused);
    }

    public void OpenLevelsPanel()
    {
        DisableAllPanels();
        levelsPanel.SetActive(true);
    }

    public void OpenMenuPanel()
    {
        ClearLevel();

        PauseGame(true);
        DisableAllPanels();
        menuPanel.SetActive(true);
    }

    public void CreateLevelButtons()
    {
        for (int i = 0; i < levelsCount; ++i)
        {
            int levelNumber = i+1;

            GameObject newButton = Instantiate(levelButtonPrefab, levelsLayout);
            newButton.name = "Level_" + levelNumber.ToString();
            newButton.GetComponentInChildren<TMP_Text>().text = levelNumber.ToString();

            newButton.GetComponent<Button>().onClick.AddListener(() => SetChoosenLevelIdx(i));
            newButton.GetComponent<Button>().onClick.AddListener(() => clickSound.Play());

            levelButtons.Add(newButton);
            //newButton.GetChildren
        }
    }

    public void UpdateLevelButtons()
    {
        for (int i = 0; i < levelButtons.Count; ++i)
        {
            // TODO: Stex DataHolderi meji tvyalnery lcnel levelneri mej, astxery poxel kaxvac qanakic

            Transform stars = levelButtons[i].transform.GetChild(0).transform;
            for (int j = 0; j < stars.childCount; ++j)
            {
                stars.GetChild(j).GetComponent<Image>().sprite = filledStarSprite;
            }
        }
    }

    void UpdateNewScores()
    {
        dataHolder.SetNewBestTime(currentLevelIdx, currentTime);
        dataHolder.SetNewBestShot(currentLevelIdx, maxShotsCount - shotsLeft);
    }

    void CheckIfStarRequired()
    {
        if(currentTime <= maxTimeForStar)
        {
            Debug.Log("Start fo TIme");
            AddStar(currentLevelIdx);
        }

        if (maxShotsCount - shotsLeft <= levelsParams[currentLevelIdx].GetShotsForStar())
        {


            Debug.Log("Start fo SHots");
            Debug.Log(maxShotsCount + " - " + shotsLeft + " <= " + levelsParams[currentLevelIdx].GetShotsForStar());
            AddStar(currentLevelIdx);
        }
    }

    void AddStar(int level)
    {
        Transform stars = levelButtons[level].transform.GetChild(0).transform;
        for (int j = 0; j < stars.childCount; ++j)
        {
            if(stars.GetChild(j).GetComponent<Image>().sprite == emptyStarSprite)
            {
                stars.GetChild(j).GetComponent<Image>().sprite = filledStarSprite;
                return;
            }
        }
    }

    // This Function Can be called only from outside, And only from button!

    void OpenLevelConfirmingPanel(int level)
    {
        levelConfirmingPanel.SetActive(true);
        levelNumberText.text = "Level" + (level + 1).ToString();
        levelStartButton.GetComponent<Button>().onClick.AddListener(() => CallLevel(level));

        pegsCountInfoText.text = dataHolder.GetLevelDestroyedPegs(level).ToString() + "/" + levelsParams[level].GetLevelPegs().ToString();
        timeInfoText.text = dataHolder.GetLevelBestTime(level).ToString();
        shotsInfoText.text = dataHolder.GetLevelBestShot(level).ToString();
        // TODO: Add here the Time and Shots that needed for Star
    }

    public void SetChoosenLevelIdx(int level)
    {
        Debug.Log("Choosing level " + level);
        currentLevelIdx = level;

        OpenLevelConfirmingPanel(level);
    }

    public void CallLevel(int level)
    {
        Debug.Log("Opening level " + level);
        //int levelIndex = level;
        //currentLevelIdx = levelIndex;
        StartGame(level);
    }

    void PrepareLevel(int level)
    {
        maxTimeForStar = levelsParams[level].GetTimeForStar();
        maxShotsCount = levelsParams[level].GetMaxShots();

        levelConstructor.ConstructLevel(levelsParams[level]);
        maxPegsCount = levelsParams[level].GetPegsCount();
    }

    void ClearLevel()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");

        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }

        spawner.ClearPegs();
    }

}
