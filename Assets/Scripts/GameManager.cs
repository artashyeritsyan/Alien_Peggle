using Microsoft.Win32.SafeHandles;
using System;
using TMPro;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> IsGamePaused;

    [SerializeField] GameObject gameOverlay;
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI shotsText;

    [SerializeField] int maxShotsCount = 5;
    private int shotsLeft;
    private int destroyedBallsCount;

    [SerializeField] PegSpawner spawner;
    private int pegsCount;
    private int score;

    private bool gamePaused = false;
    void Start()
    {
        destroyedBallsCount = 0;

        gameOverlay.SetActive(true);
        gameOverPanel.SetActive(false);

        UpdateScoreText();

        gamePaused = false;
        IsGamePaused?.Invoke(false);

        pegsCount = spawner.GetPegsCount();

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

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score + "/" + pegsCount;
    }

    void AddScore()
    {
        ++score;
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
        gameOverlay.SetActive(false);
        gameOverPanel.SetActive(true);
        totalScoreText.text = "Total Score: " + score + "/" + pegsCount;
        IsGamePaused?.Invoke(true);
    }

    public void Restart()
    {
        //SceneManager.LoadScene(0);

        // UI part
        gameOverlay.SetActive(true);
        gameOverPanel.SetActive(false);
        score = 0;
        UpdateScoreText();
        UpdateShotsText();

        // IDK part
        spawner.CreateLevel();
        IsGamePaused?.Invoke(false);
        pegsCount = spawner.GetPegsCount();
        shotsLeft = maxShotsCount;
        destroyedBallsCount = 0;
    }

    public int GetLeftShotsCount()
    {
        return shotsLeft;
    }
}
