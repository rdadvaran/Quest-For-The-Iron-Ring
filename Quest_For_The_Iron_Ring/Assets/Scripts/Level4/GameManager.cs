using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gameplay")]
    [SerializeField] private float timer = 60f;
    [SerializeField] private int burnoutLevel = 0;
    [SerializeField] private int maxBurnout = 5;
    [SerializeField] private float bugsSquashed = 0f;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text burnoutText;

    [Header("Burnout Meter")]
    [SerializeField] private Image burnoutMeterImage;
    [SerializeField] private Sprite[] burnoutMeterSprites;

    [Header("Start Countdown")]
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private float startDelay = 5f;

    [Header("Character Bonus")]
    [SerializeField] private float burnoutRecoveryInterval = 30f;

    private float burnoutRecoveryTimer = 0f;

    private int fastBugHitCounter = 0;
    private int fastBugKillCounter = 0;

    private bool gameEnded = false;
    private bool gameStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ApplyDifficultyTimer();
        UpdateUI();
        StartCoroutine(StartGameCountdown());
    }

    private void Update()
    {
        if (gameEnded || !gameStarted)
            return;

        timer -= Time.deltaTime;

        HandleCharacterBonus(); // 👈 NEW

        if (timer <= 0f)
        {
            timer = 0f;
            UpdateUI();
            EndLevelWithGrade();
            return;
        }

        UpdateUI();
    }

    private void HandleCharacterBonus()
    {
        // Only apply in Level4
        if (SceneManager.GetActiveScene().name != "Level4")
            return;

        if (CharacterManager.Instance == null)
            return;

        if (!CharacterManager.Instance.HasLevel4BurnoutRecovery())
            return;

        burnoutRecoveryTimer += Time.deltaTime;

        if (burnoutRecoveryTimer >= burnoutRecoveryInterval)
        {
            burnoutRecoveryTimer = 0f;
            ReduceBurnout(1);
            Debug.Log("Burnout reduced by character bonus.");
        }
    }

    private void ApplyDifficultyTimer()
    {
        if (DifficultyManager.Instance == null)
            return;

        switch (DifficultyManager.Instance.GetDifficulty())
        {
            case DifficultyManager.Difficulty.IdleSlacker:
                timer = 75f;
                break;

            case DifficultyManager.Difficulty.AverageJoe:
                timer = 65f;
                break;

            case DifficultyManager.Difficulty.Goody2Shoes:
                timer = 60f;
                break;

            case DifficultyManager.Difficulty.Perfectionist:
                timer = 55f;
                break;
        }

        Debug.Log("Timer set to: " + timer);
    }

    private IEnumerator StartGameCountdown()
    {
        gameStarted = false;
        Time.timeScale = 0f;

        if (instructionPanel != null)
            instructionPanel.SetActive(true);

        int countdownSeconds = Mathf.CeilToInt(startDelay);

        for (int i = countdownSeconds; i > 0; i--)
        {
            if (instructionText != null)
            {
                instructionText.text =
                    "WASD - MOVE\n" +
                    "SPACEBAR - SQUASH BUGS\n" +
                    "THE BIGBUG NEEDS 3 HITS!\n\n" +
                    "KEEP THE SCORE HIGH AND BURNOUT LOW!\n\n" +
                    "GAME STARTING IN " + i + "...";
            }

            yield return new WaitForSecondsRealtime(1f);
        }

        if (instructionText != null)
            instructionText.text = "GO!";

        yield return new WaitForSecondsRealtime(0.5f);

        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        Time.timeScale = 1f;
        gameStarted = true;
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "TIME: " + Mathf.CeilToInt(timer);

        if (scoreText != null)
            scoreText.text = "SCORE: " + (bugsSquashed % 1 == 0 ? bugsSquashed.ToString("0") : bugsSquashed.ToString("0.0"));

        if (burnoutText != null)
            burnoutText.text = "BURNOUT: " + burnoutLevel + "/" + maxBurnout;

        if (burnoutMeterImage != null &&
            burnoutMeterSprites != null &&
            burnoutLevel >= 0 &&
            burnoutLevel < burnoutMeterSprites.Length)
        {
            burnoutMeterImage.sprite = burnoutMeterSprites[burnoutLevel];
        }
    }

    public void AddScore(float amount)
    {
        if (gameEnded || !gameStarted) return;

        bugsSquashed += amount;
        UpdateUI();
    }

    public void AddBurnout(int amount)
    {
        if (gameEnded || !gameStarted) return;

        burnoutLevel += amount;

        if (burnoutLevel >= maxBurnout)
        {
            burnoutLevel = maxBurnout;
            gameEnded = true;
            UpdateUI();

            if (LevelEndManager.Instance != null)
            {
                LevelEndManager.Instance.TriggerGameOver();
            }

            return;
        }

        UpdateUI();
    }

    public void ReduceBurnout(int amount)
    {
        if (gameEnded || !gameStarted) return;

        burnoutLevel -= amount;

        if (burnoutLevel < 0)
            burnoutLevel = 0;

        UpdateUI();
    }

    public void AddTime(float amount)
    {
        if (gameEnded || !gameStarted) return;

        timer += amount;
        UpdateUI();
    }

    private void EndLevelWithGrade()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (LevelEndManager.Instance != null)
        {
            LevelEndManager.Instance.TriggerPassFail(bugsSquashed, burnoutLevel, maxBurnout);
        }
    }

    public int GetBurnoutLevel() => burnoutLevel;
    public int GetMaxBurnout() => maxBurnout;
    public float GetScore() => bugsSquashed;
    public bool HasGameStarted() => gameStarted;

    public void RegisterFastBugHit()
    {
        if (gameEnded || !gameStarted) return;

        fastBugHitCounter++;

        if (fastBugHitCounter >= 2)
        {
            fastBugHitCounter = 0;
            AddBurnout(1);
        }
    }

    public void RegisterFastBugKill()
    {
        if (gameEnded || !gameStarted) return;

        fastBugKillCounter++;

        if (fastBugKillCounter >= 2)
        {
            fastBugKillCounter = 0;
            AddScore(1);
        }
    }
}