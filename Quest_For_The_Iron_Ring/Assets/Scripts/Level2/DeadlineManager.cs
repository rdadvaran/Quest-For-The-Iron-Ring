using TMPro;
using UnityEngine;

public class DeadlineManager : MonoBehaviour
{
    public float timeRemaining = 120f;
    public bool hasDeadline = true;
    public TMP_Text timerText;

    private bool timerRunning = false;

    // 🔹 Difficulty-based time settings
    public float idleSlackerTime = 180f;      
    public float averageJoeTime = 120f;       
    public float goody2ShoesTime = 90f;      
    public float perfectionistTime = 60f;  

    public void StartTimer()
    {
        timeRemaining = GetTimeBasedOnDifficulty();
        timerRunning = true;
        UpdateTimerUI();
    }

    public void StartTimer(float startTime)
    {
        // Optional override 
        timeRemaining = startTime;
        timerRunning = true;
        UpdateTimerUI();
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    void Update()
    {
        if (!timerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) timeRemaining = 0;
            UpdateTimerUI();
        }
        else
        {
            timerRunning = false;
        }
    }

    public bool IsTimeUp()
    {
        return timeRemaining <= 0;
    }

    public float GetTimeBasedOnDifficulty()
    {
        if (GameSession.Instance == null)
        {
            Debug.LogWarning("GameSession not found. Using default time.");
            return averageJoeTime;
        }

        string difficulty = GameSession.Instance.selectedDifficulty;

        switch (difficulty)
        {
            case "Idle Slacker":
                return idleSlackerTime;

            case "Average Joe":
                return averageJoeTime;

            case "Goody 2 Shoes":
                return goody2ShoesTime;

            case "Perfectionist":
                return perfectionistTime;

            default:
                Debug.LogWarning("Unknown difficulty: " + difficulty);
                return averageJoeTime;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}