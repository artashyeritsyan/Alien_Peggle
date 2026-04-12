using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    [SerializeField] GameObject overlayShotsStar;
    [SerializeField] GameObject overlayTimeStar;

    [Header("Win Panel Parameters")]
    [SerializeField] GameObject winPanelStars;
    private Vector2 winStarStartScale;


    [Header("Level Buttons Parameters")]
    [SerializeField] int levelsCount = 15;
    [SerializeField] Transform levelsLayout;
    [SerializeField] GameObject levelButtonPrefab;
    private List<GameObject> levelButtons;

    [Header("Star Sprites")]
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] Sprite filledStarSprite;
    // ====================== UI Part End ===========================


    [SerializeField] int maxShotsCount = 12;
    private int shots;
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
    [SerializeField] TextMeshProUGUI timeForStarText;
    [SerializeField] TextMeshProUGUI shotsForStarText;
    [SerializeField] Button levelStartButton;
    [SerializeField] GameObject levelInfoStars;

    [Header("Sounds")]
    [SerializeField] AudioSource BgAudioSource;
    [SerializeField] AudioClip defaultBgMusic;
    [SerializeField] AudioSource clickSound;
    [SerializeField] Button overlaySoundButton;
    [SerializeField] Button overlayMusicButton;
    [SerializeField] Button menuSoundButton;
    [SerializeField] Button menuMusicButton;

    [SerializeField] Sprite soundONSprite;
    [SerializeField] Sprite soundOFFSprite;

    [SerializeField] Sprite musicONSprite;
    [SerializeField] Sprite musicOFFSprite;

    [SerializeField] bool isSoundOn;
    [SerializeField] bool isMusicOn;

    [SerializeField] DataHolder dataHolder;


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
        winStarStartScale = winPanelStars.transform.GetChild(0).localScale;

        PauseGame(true);

        levelButtons = new List<GameObject>();
        // TODO: Create DataHolder script to save all info about levels
        CreateLevelButtons();
        dataHolder.LoadPlayerProgress();
        UpdateLevelsStars();
        //savedData = FindFirstObjectByType<DataHolder>();

        DisableAllPanels();
        menuPanel.SetActive(true);

        destroyedBallsCount = 0;
        isBallInGame = false;
        shots = 0;
        UpdateScoreText();
        UpdateShotsText();

        isMusicOn = true;
        isSoundOn = true;
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
        if (shots < maxShotsCount)
        {
            shots++;
        }
        isBallInGame = true;
        UpdateShotsText();
    }

    void UpdateShotsText()
    {
        Transform Star = overlayShotsStar.transform;

        if (shots <= levelsParams[currentLevelIdx].GetShotsForStar())
        {
            shotsText.text = "Shots: " + shots + "/" + levelsParams[currentLevelIdx].GetShotsForStar();
            overlayShotsStar.GetComponent<Image>().sprite = filledStarSprite;

            if (!DOTween.IsTweening(Star))
            {
                Star.DOScale(Star.localScale * 0.8f, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            }
        }
        else
        {
            shotsText.text = "Shots: " + shots + "/" + maxShotsCount;
            overlayShotsStar.GetComponent<Image>().sprite = emptyStarSprite;

            if (DOTween.IsTweening(Star))
            {
                Star.DOKill();
            }
        }
    }

    void CheckIfWin()
    {
        if (maxPegsCount == destroyedPegsCount)
        {
            GameWin();
        }
    }

    public void PlayClickSound()
    {
        if (isSoundOn)
        {
            clickSound.Play();
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

        Transform Star = overlayTimeStar.transform;
        if (currentTime < levelsParams[currentLevelIdx].GetTimeForStar())
        {
            overlayTimeStar.GetComponent<Image>().sprite = filledStarSprite;
            if (!DOTween.IsTweening(Star))
            {
                Star.DOScale(Star.localScale * 0.8f, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            }
        }
        else
        {
            overlayTimeStar.GetComponent<Image>().sprite = emptyStarSprite;
            if (DOTween.IsTweening(Star))
            {
                Star.DOKill();
            }
        }

        //float time = currentTime / maxTimeForStar;
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

    void BallDestroyed()
    {
        destroyedBallsCount++;
        isBallInGame = false;
        //CheckIfWin();
        CheckIsGameLose();
    }

    public bool CanShoot()
    {
        return !isBallInGame;
    }

    void CheckIsGameLose()
    {
        if (shots >= maxShotsCount && destroyedBallsCount >= maxShotsCount)
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
        CheckIfStarRequired(currentLevelIdx);
        AnimateStars();
        dataHolder.SavePlayerProgress();

        Debug.Log("Start fo Win");
        PauseGame(true);
    }

    void GameOver()
    {
        DisableAllPanels();
        gameOverPanel.SetActive(true);
        totalScoreText.text = "Total Score: " + destroyedPegsCount + "/" + maxPegsCount;

        // Just updating the destroyed counts
        dataHolder.SetNewDestroyedPegs(currentLevelIdx, destroyedPegsCount);
        dataHolder.SavePlayerProgress();

        //dataHolder.SavePlayerProgress();

        PauseGame(true);
    }

    public void NextLevel()
    {
        if (currentLevelIdx == levelsParams.Count - 1 || currentLevelIdx == 15 && !isSecretLevelUnlocked)
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

        shots = 0;
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


    public int GetShotsLeft()
    {
        return maxShotsCount - shots;
    }

    public void PauseGame(bool isPaused)
    {
        isGamePaused = isPaused;
        IsGamePaused?.Invoke(isPaused);
    }

    public void OpenLevelsPanel()
    {
        //dataHolder.SavePlayerProgress();
        //dataHolder.LoadPlayerProgress();
        //UpdateLevelsStars();

        if (gameOverlay.activeSelf || gameOverPanel.activeSelf || gameWinPanel.activeSelf)
        {
            BgAudioSource.Stop();
            BgAudioSource.clip = defaultBgMusic;
            BgAudioSource.Play(); 
        }
        DisableAllPanels();
        levelsPanel.SetActive(true);
    }

    public void OpenMenuPanel()
    {
        ClearLevel();
        if (gameOverlay.activeSelf || gameOverPanel.activeSelf || gameWinPanel.activeSelf)
        {
            BgAudioSource.Stop();
            BgAudioSource.clip = defaultBgMusic;
            if (isMusicOn)
            {
                BgAudioSource.Play();
            }
        }

        PauseGame(true);
        DisableAllPanels();

        dataHolder.SavePlayerProgress();
        menuPanel.SetActive(true);

    }

    public void CreateLevelButtons()
    {
        for (int i = 0; i < levelsCount; ++i)
        {
            int levelNumber = i;

            GameObject newButton = Instantiate(levelButtonPrefab, levelsLayout);
            newButton.name = "Level_" + (levelNumber + 1).ToString();
            newButton.GetComponentInChildren<TMP_Text>().text = (levelNumber + 1).ToString();

            newButton.GetComponent<Button>().onClick.AddListener(() => SetChoosenLevelIdx(levelNumber));
            newButton.GetComponent<Button>().onClick.AddListener(() => PlayClickSound());

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
        Debug.Log("Current Level" + currentLevelIdx);
        dataHolder.SetLevelCompleted(currentLevelIdx);
        dataHolder.SetNewDestroyedPegs(currentLevelIdx, destroyedPegsCount);
        dataHolder.SetNewBestTime(currentLevelIdx, currentTime);
        dataHolder.SetNewBestShot(currentLevelIdx, shots);
    }

    void UpdateLevelsStars()
    {
        for (int i = 0; i < levelButtons.Count; ++i)
        {

            if (dataHolder.GetIsLevelCompleted(i))
            {
                AddStar(i, 0);
            }

            if (dataHolder.GetLevelBestTime(i) > 0 && dataHolder.GetLevelBestTime(i) <= levelsParams[i].GetTimeForStar())
            {
                Debug.Log("Start fo Time");
                // 1 stands for StarForTime
                AddStar(i, 1);
            }

            if (dataHolder.GetLevelBestShot(i) > 0 &&  dataHolder.GetLevelBestShot(i) <= levelsParams[i].GetShotsForStar())
            {
                Debug.Log("Start fo SHots");
                // 2 stands for StarForShot
                AddStar(i, 2);
            }
        }
    }

    void CheckIfStarRequired(int level)
    {
        if (dataHolder.GetIsLevelCompleted(level))
        {
            AddStar(currentLevelIdx, 0);
        }


        if (dataHolder.GetLevelBestTime(level) <= levelsParams[level].GetTimeForStar())
        {
            Debug.Log("Start fo TIme");
            // 1 stands for StarForTime
            AddStar(level, 1);
        }

        if (shots <= levelsParams[level].GetShotsForStar())
        {
            // 2 stands for StarForShot
            AddStar(level, 2);
        }

        // Setting the Win panel start. // TODO:: Later make it another function
        Transform stars = levelButtons[level].transform.GetChild(0).transform;
        winPanelStars.transform.GetChild(0).GetComponent<Image>().sprite = stars.GetChild(0).GetComponent<Image>().sprite;
        winPanelStars.transform.GetChild(1).GetComponent<Image>().sprite = stars.GetChild(1).GetComponent<Image>().sprite;
        winPanelStars.transform.GetChild(2).GetComponent<Image>().sprite = stars.GetChild(2).GetComponent<Image>().sprite;
    }

    // Adding star to level, and second int for choosing is it for Win for Time or Shots
    void AddStar(int level, int starIdx)
    {
        if (starIdx > 3) return;
        Transform stars = levelButtons[level].transform.GetChild(0).transform;

        if (stars.GetChild(starIdx).GetComponent<Image>().sprite == emptyStarSprite)
        {
            stars.GetChild(starIdx).GetComponent<Image>().sprite = filledStarSprite;
        }
    }

    void AnimateStars()
    {

        for (int i = 0; i < 3; i++)
        {
            Transform childObj = winPanelStars.transform.GetChild(i);
            childObj.DOKill();
            if (childObj.GetComponent<Image>().sprite == filledStarSprite)
            {
                childObj.localScale *= 0.05f;
                childObj.DOScale(winStarStartScale, 0.5f).SetEase(Ease.OutQuad)
                    .OnComplete(() => childObj.DOScale(winStarStartScale * 0.7f, 0.6f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo));
            }
        }

    }

    // This Function Can be called only from outside, And only from button!
    void OpenLevelConfirmingPanel(int level)
    {
        levelConfirmingPanel.SetActive(true);
        levelNumberText.text = "Level" + (level + 1).ToString();
        levelStartButton.GetComponent<Button>().onClick.AddListener(() => CallLevel(level));


        pegsCountInfoText.text = "Pegs:  " + (dataHolder.GetLevelDestroyedPegs(level) < 0 ? 0 : dataHolder.GetLevelDestroyedPegs(level)).ToString()
            + "/" + levelsParams[level].GetPegsCount().ToString();
        shotsInfoText.text = "Best Shots:  " + (dataHolder.GetLevelBestShot(level) < 0 ? 0 : dataHolder.GetLevelBestShot(level)).ToString();
        shotsForStarText.text = "Target Shots:  " + levelsParams[level].GetShotsForStar().ToString();

        //Time prints part
        {
            float bestTime = dataHolder.GetLevelBestTime(level);
            if (bestTime < 0)
                bestTime = 0;

            float starTime = levelsParams[level].GetTimeForStar();

            timeInfoText.text = "Best Time:  " + System.TimeSpan.FromSeconds(bestTime).ToString(@"mm\:ss");
            timeForStarText.text = "Target Time:  " + System.TimeSpan.FromSeconds(starTime).ToString(@"mm\:ss");
        }

        // TODO: Refactor this logic later
        Transform stars = levelButtons[level].transform.GetChild(0).transform;
        levelInfoStars.transform.GetChild(0).GetComponent<Image>().sprite = stars.GetChild(0).GetComponent<Image>().sprite;
        levelInfoStars.transform.GetChild(1).GetComponent<Image>().sprite = stars.GetChild(1).GetComponent<Image>().sprite;
        levelInfoStars.transform.GetChild(2).GetComponent<Image>().sprite = stars.GetChild(2).GetComponent<Image>().sprite;
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

    public void ResetStars()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {

            Transform stars = levelButtons[i].transform.GetChild(0).transform;
            for (int j = 0; j < stars.childCount; ++j)
            {
                if (stars.GetChild(j).GetComponent<Image>().sprite == filledStarSprite)
                {
                    stars.GetChild(j).GetComponent<Image>().sprite = emptyStarSprite;
                }
            }
        }
    }

    public void ToggleSound()
    {
        Image overlayButton = overlaySoundButton.gameObject.transform.GetChild(0).GetComponent<Image>();
        Image menuButton = menuSoundButton.gameObject.transform.GetChild(0).GetComponent<Image>();
        if (isSoundOn)
        {
            isSoundOn = false;
            overlayButton.sprite = soundOFFSprite;
            menuButton.sprite = soundOFFSprite;

        }
        else
        {
            isSoundOn = true;
            overlayButton.sprite = soundONSprite;
            menuButton.sprite = soundONSprite;
        }
    }
    public void ToggleMusic()
    {
        Image overlayButton = overlayMusicButton.gameObject.transform.GetChild(0).GetComponent<Image>();
        Image menuButton = menuMusicButton.gameObject.transform.GetChild(0).GetComponent<Image>();
        if (isMusicOn)
        {
            isMusicOn = false;
            overlayButton.sprite = musicOFFSprite;
            menuButton.sprite = musicOFFSprite;
            BgAudioSource.volume = 0;
        }
        else
        {
            isMusicOn = true;
            overlayButton.sprite = musicONSprite;
            menuButton.sprite = musicONSprite;
            BgAudioSource.volume = 1;
        }
    }

    bool isSecretLevelUnlocked = false;

    public void ShowSecretLevel()
    {
        if (isSecretLevelUnlocked) return;

        levelsCount = 16;

        int levelNumber = 15;

        GameObject newButton = Instantiate(levelButtonPrefab, levelsLayout);
        newButton.name = "Level_" + (levelNumber + 1).ToString();
        newButton.GetComponentInChildren<TMP_Text>().text = (levelNumber + 1).ToString();

        newButton.GetComponent<Button>().onClick.AddListener(() => OpenSecretLevel());
        newButton.GetComponent<Button>().onClick.AddListener(() => PlayClickSound());

        levelButtons.Add(newButton);
        isSecretLevelUnlocked = true;
    }

    public void OpenSecretLevel()
    {
        Debug.Log("Secret level unlocked");
        StartGame(15);
    }

    public bool GetIsSoundOn()
    {
        return isSoundOn;
    }

    public bool GetIsMusicOn()
    {
        return isSoundOn;
    }
}
