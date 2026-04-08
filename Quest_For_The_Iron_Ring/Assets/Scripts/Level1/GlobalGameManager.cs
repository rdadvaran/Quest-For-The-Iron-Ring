using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    // Shared timer across classroom and jigsaw scenes
    public float totalTime = 0f;
    public bool timerRunning = true;

    // Shared progress data
    public int totalPieces = 9;
    public int collectedPieces = 0;
    public int correctPlacements = 0;

    // Final result data
    public string finalGrade = "";
    public bool puzzleCompleted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (timerRunning)
        {
            totalTime += Time.deltaTime;
        }
    }

    public void ResetGameData()
    {
        totalTime = 0f;
        timerRunning = true;
        totalPieces = 9;
        collectedPieces = 0;
        correctPlacements = 0;
        finalGrade = "";
        puzzleCompleted = false;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public string CalculateGrade()
    {
        if (totalTime < 90f) return "A";
        if (totalTime < 120f) return "B";
        if (totalTime < 150f) return "C";
        if (totalTime < 180f) return "D";
        return "F";
    }
}