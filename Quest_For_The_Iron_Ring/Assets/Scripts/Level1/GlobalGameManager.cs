using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    // Shared timer across classroom + jigsaw scenes
    public float totalTime = 0f;
    public bool timerRunning = true;

    // Shared progress data
    public int totalPieces = 9;
    public int collectedPieces = 0;
    public int correctPlacements = 0;

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
        collectedPieces = 0;
        correctPlacements = 0;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
}