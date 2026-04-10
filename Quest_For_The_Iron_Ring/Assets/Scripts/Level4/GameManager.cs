using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float timer = 60f;
    [SerializeField] private int burnoutLevel = 0;
    [SerializeField] private int maxBurnout = 5;
    [SerializeField] private float bugsSquashed = 0f;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text burnoutText;

    [SerializeField] private Image burnoutMeterImage;
    [SerializeField] private Sprite[] burnoutMeterSprites;

    private int fastBugHitCounter = 0;
    private int fastBugKillCounter = 0;

    private bool gameEnded = false;

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
        UpdateUI();
    }

    private void Update()
    {
        if (gameEnded)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            UpdateUI();
            EndLevelWithGrade();
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "TIME: " + Mathf.CeilToInt(timer);

        if (scoreText != null)
            scoreText.text = "SCORE: " + (bugsSquashed % 1 == 0 ? bugsSquashed.ToString("0") : bugsSquashed.ToString("0.0"));

        if (burnoutText != null)
            burnoutText.text = "BURNOUT: " + burnoutLevel + "/" + maxBurnout;

        if (burnoutMeterImage != null && burnoutMeterSprites != null && burnoutLevel >= 0 && burnoutLevel < burnoutMeterSprites.Length)
        {
            burnoutMeterImage.sprite = burnoutMeterSprites[burnoutLevel];
        }
    }

    public void AddScore(float amount)
    {
        if (gameEnded) return;

        bugsSquashed += amount;
        UpdateUI();
    }

    public void AddBurnout(int amount)
    {
        if (gameEnded) return;

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
            else
            {
                Debug.LogWarning("LevelEndManager instance is missing.");
            }

            return;
        }

        UpdateUI();
    }

    public void ReduceBurnout(int amount)
    {
        if (gameEnded) return;

        burnoutLevel -= amount;

        if (burnoutLevel < 0)
            burnoutLevel = 0;

        UpdateUI();
    }

    public void AddTime(float amount)
    {
        if (gameEnded) return;

        timer += amount;
        UpdateUI();
    }

    private void EndLevelWithGrade()
    {
        if (gameEnded) return;

        gameEnded = true;

        Debug.Log($"Level ended. Score: {bugsSquashed}, Burnout: {burnoutLevel}/{maxBurnout}");

        if (LevelEndManager.Instance != null)
        {
            LevelEndManager.Instance.TriggerPassFail(bugsSquashed, burnoutLevel, maxBurnout);
        }
        else
        {
            Debug.LogWarning("LevelEndManager instance is missing.");
        }
    }

    public int GetBurnoutLevel()
    {
        return burnoutLevel;
    }

    public int GetMaxBurnout()
    {
        return maxBurnout;
    }

    public float GetScore()
    {
        return bugsSquashed;
    }

    public void RegisterFastBugHit()
    {
        if (gameEnded) return;

        fastBugHitCounter++;

        if (fastBugHitCounter >= 2)
        {
            fastBugHitCounter = 0;
            AddBurnout(1);
        }
    }

    public void RegisterFastBugKill()
    {
        if (gameEnded) return;

        fastBugKillCounter++;

        if (fastBugKillCounter >= 2)
        {
            fastBugKillCounter = 0;
            AddScore(1);
        }
    }
}