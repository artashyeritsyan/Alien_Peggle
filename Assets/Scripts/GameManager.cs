using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static event Action<bool> IsGamePaused;

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
    [SerializeField] Image timeBar;
    public float roundTime;  // TODO: need to get roundTIme from every round (and make private). But now gets from inspector
    private float timeRemaining;


    [SerializeField] int maxShotsCount = 5;
    private int shotsLeft;
    private int destroyedBallsCount;

    [SerializeField] PegSpawner spawner;
    private int maxPegsCount;
    private int destroyedPegsCount;


    [Header("Level Buttons Parameters")]
    [SerializeField] int levelsCount = 15;
    [SerializeField] Transform levelsLayout;
    [SerializeField] GameObject levelButtonPrefab;
    private List<GameObject> levelButtons;

    [Header("Star Sprites")]
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] Sprite filledStarSprite;

    private bool isGamePaused;
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
        maxPegsCount = spawner.GetPegsCount();
        shotsLeft = maxShotsCount;
        UpdateScoreText();
        UpdateShotsText();


    }

    private void Update()
    {
        if (!isGamePaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                GameOver();
            }

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

    void GameWin()
    {
        DisableAllPanels();
        gameWinPanel.SetActive(true);
        PauseGame(true);
    }

    void DisableAllPanels()
    {
        gameOverlay.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        menuPanel.SetActive(false);
        levelsPanel.SetActive(false);
    }

    void UpdateTimerUI()
    {
        timeBar.fillAmount = timeRemaining / roundTime;

        float t = timeRemaining / roundTime;
        timeBar.color = Color.Lerp(Color.red, new Color32(37, 113, 0, 255), t);
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
        //CheckIfWin();
        CheckIsGameOver();
    }

    void CheckIsGameOver()
    {
        if (shotsLeft <= 0 && destroyedBallsCount >= maxShotsCount)
        {
            if (isGamePaused) { return; }
            GameOver();
        }
    }

    void GameOver()
    {
        DisableAllPanels();
        gameOverPanel.SetActive(true);
        totalScoreText.text = "Total Score: " + destroyedPegsCount + "/" + maxPegsCount;
        IsGamePaused?.Invoke(true);
    }

    public void Restart()
    {
        // IDK part
        spawner.CreateLevel();
        maxPegsCount = spawner.GetPegsCount();
        destroyedPegsCount = 0;

        shotsLeft = maxShotsCount;
        destroyedBallsCount = 0;

        timeRemaining = roundTime;

        // UI part
        DisableAllPanels();
        gameOverlay.SetActive(true);
        UpdateScoreText();
        UpdateShotsText();
        UpdateTimerUI();

        PauseGame(false);
    }

    public void StartGame()
    {
        spawner.CreateLevel();
        maxPegsCount = spawner.GetPegsCount();
        destroyedPegsCount = 0;
        shotsLeft = maxShotsCount;
        destroyedBallsCount = 0;

        timeRemaining = roundTime;

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
        PauseGame(true);
        DisableAllPanels();
        menuPanel.SetActive(true);
    }

    public void CreateLevelButtons()
    {
        for (int i = 0; i < levelsCount; ++i)
        {
            int levelIndex = i+1;

            GameObject newButton = Instantiate(levelButtonPrefab, levelsLayout);
            newButton.name = "Level_" + levelIndex.ToString();
            newButton.GetComponentInChildren<TMP_Text>().text = levelIndex.ToString();

            newButton.GetComponent<Button>().onClick.AddListener(() => CallLevel(levelIndex));

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

    public void CallLevel(int level)
    {
        Debug.Log("Opening level " + level);
        // TODO: Open the level...
        // TODO: Give the level number to StartGame. (Or just set it here)
        PrepareLevel(level);
        StartGame();
    }

    void PrepareLevel(int level)
    {
        switch (level)
        {
            case 1:
                maxShotsCount = 7;
                spawner.SetXInterval(2.5f);
                spawner.SetYInterval(2);
                spawner.SetRandomStrength(1.5f);
                break;
            case 2:
                maxShotsCount = 8;
                spawner.SetXInterval(2.5f);
                spawner.SetYInterval(2);
                spawner.SetRandomStrength(1);
                break;
            case 3:
                maxShotsCount = 9;
                spawner.SetXInterval(2);
                spawner.SetYInterval(1);
                spawner.SetRandomStrength(1f);
                break;
            case 4:
                maxShotsCount = 10;
                spawner.SetXInterval(1.5f);
                spawner.SetYInterval(1);
                spawner.SetRandomStrength(1);
                break;
            case 5:
                maxShotsCount = 11;
                spawner.SetXInterval(1);
                spawner.SetYInterval(1);
                spawner.SetRandomStrength(0.5f);
                break;
            default:
                maxShotsCount = 12;
                spawner.SetXInterval(1);
                spawner.SetYInterval(1);
                spawner.SetRandomStrength(0);
                break;
        }
    }
}
