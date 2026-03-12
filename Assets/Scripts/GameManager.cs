using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverlay;
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    private int score;


    void Start()
    {
        gameOverlay.SetActive(true);
        gameOverPanel.SetActive(false);


        scoreText.text = "Score: 0";
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

    // Update is called once per frame
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
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
