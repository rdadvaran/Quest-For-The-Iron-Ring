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

    // Grade percentage based on selected difficulty and completion time
    public float GetGradePercentage()
    {
        float timeFor100 = 75f;
        float timeFor85 = 100f;
        float timeFor70 = 130f;

        if (GameSession.Instance != null)
        {
            switch (GameSession.Instance.selectedDifficulty)
            {
                case "Idle Slacker":
                    timeFor100 = 45f;
                    timeFor85 = 60f;
                    timeFor70 = 75f;
                    break;

                case "Average Joe":
                    timeFor100 = 40f;
                    timeFor85 = 55f;
                    timeFor70 = 70f;
                    break;

                case "Goodie 2 Shoes":
                    timeFor100 = 35f;
                    timeFor85 = 50f;
                    timeFor70 = 65f;
                    break;

                case "Perfectionist":
                    timeFor100 = 25f;
                    timeFor85 = 40f;
                    timeFor70 = 55f;
                    break;

                default:
                    timeFor100 = 80f;
                    timeFor85 = 105f;
                    timeFor70 = 135f;
                    break;
            }
        }

        if (totalTime <= timeFor100) return 100f;
        if (totalTime <= timeFor85) return 85f;
        if (totalTime <= timeFor70) return 70f;
        return 50f;
    }

    // Required passing grade based on selected difficulty
    public float GetPassingPercentage()
    {
        if (GameSession.Instance != null)
        {
            switch (GameSession.Instance.selectedDifficulty)
            {
                case "Idle Slacker":
                    return 50f;

                case "Average Joe":
                    return 70f;

                case "Goodie 2 Shoes":
                    return 85f;

                case "Perfectionist":
                    return 100f;

                default:
                    return 70f;
            }
        }

        return 70f;
    }

    public bool DidPlayerPass()
    {
        MarkSaver.Instance.SaveGrade("Level1",GetGradePercentage());
        return GetGradePercentage() >= GetPassingPercentage();
    }

    public string CalculateGrade()
    {
        return GetGradePercentage().ToString("0") + "%";
    }
}