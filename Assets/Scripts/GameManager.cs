using System;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> IsGamePaused;

    [SerializeField] GameObject gameOverlay;
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;

    [SerializeField] PegSpawner spawner;
    [SerializeField] GameObject player;
    [SerializeField] Vector2 playerStartPosition;
    private int score;

    private bool gamePaused = false;
    void Start()
    {
        gameOverlay.SetActive(true);
        gameOverPanel.SetActive(false);

        scoreText.text = "Score: 0";

        gamePaused = false;
        IsGamePaused?.Invoke(false);
    }

    private void OnEnable()
    {
        Ball.OnPointCollected += AddScore;
        Ball.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        Ball.OnPointCollected -= AddScore;
        Ball.OnGameOver -= GameOver;
    }

    void Update()
    {
        
    }


    void AddScore()
    {
        ++score;
        scoreText.text = "Score: " + score;
    }

    void GameOver()
    {
        gameOverlay.SetActive(false);
        gameOverPanel.SetActive(true);
        totalScoreText.text = "Total Score: " + score;
        IsGamePaused?.Invoke(true);
    }

    public void Restart()
    {
        //SceneManager.LoadScene(0);
        gameOverlay.SetActive(true);
        gameOverPanel.SetActive(false);
        score = 0;
        scoreText.text = "Score: " + score;
        spawner.CreateLevel();
        IsGamePaused?.Invoke(false);
    }
}
