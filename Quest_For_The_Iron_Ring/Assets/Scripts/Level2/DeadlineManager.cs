using UnityEngine;
using TMPro;

public class DeadlineManager : MonoBehaviour
{
    public float timeRemaining = 120f;
    public bool hasDeadline = true;
    public TMP_Text timerText;

    private bool timerRunning = false;

    public void StartTimer(float startTime)
    {
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

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}