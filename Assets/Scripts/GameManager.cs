using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> IsGamePaused;

    [SerializeField] GameObject gameOverlay;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameWinPanel;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI shotsText;

    [SerializeField] int maxShotsCount = 5;
    private int shotsLeft;
    private int destroyedBallsCount;

    [SerializeField] PegSpawner spawner;
    private int maxPegsCount;
    private int destroyedPegsCount;

    private bool gamePaused = false;
    void Start()
    {
        destroyedBallsCount = 0;

        DisableAllPanels();
        gameOverlay.SetActive(true);

        UpdateScoreText();

        gamePaused = false;
        IsGamePaused?.Invoke(false);

        maxPegsCount = spawner.GetPegsCount();

        shotsLeft = maxShotsCount;
        UpdateShotsText();
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
    }

    void DisableAllPanels()
    {
        gameOverlay.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + destroyedPegsCount + "/" + maxPegsCount;
    }

    void AddScore()
    {
        ++destroyedPegsCount;
        UpdateScoreText();
    }

    void BallDestroyed ()
    {
        destroyedBallsCount++;

        CheckIsGameOver();
    }

    void CheckIsGameOver()
    {
        if (shotsLeft <= 0 && destroyedBallsCount >= maxShotsCount)
        {
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
        //SceneManager.LoadScene(0);

        // UI part
        DisableAllPanels();
        gameOverlay.SetActive(true);
        destroyedPegsCount = 0;
        UpdateScoreText();
        UpdateShotsText();

        // IDK part
        spawner.CreateLevel();
        IsGamePaused?.Invoke(false);
        maxPegsCount = spawner.GetPegsCount();
        shotsLeft = maxShotsCount;
        destroyedBallsCount = 0;
    }

    public int GetLeftShotsCount()
    {
        return shotsLeft;
    }
}
