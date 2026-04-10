using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    public enum Difficulty
    {
        IdleSlacker,
        AverageJoe,
        Goody2Shoes,
        Perfectionist
    }

    [SerializeField] private Difficulty selectedDifficulty = Difficulty.AverageJoe;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetIdleSlacker()
    {
        selectedDifficulty = Difficulty.IdleSlacker;
    }

    public void SetAverageJoe()
    {
        selectedDifficulty = Difficulty.AverageJoe;
    }

    public void SetGoody2Shoes()
    {
        selectedDifficulty = Difficulty.Goody2Shoes;
    }

    public void SetPerfectionist()
    {
        selectedDifficulty = Difficulty.Perfectionist;
    }

    public Difficulty GetDifficulty()
    {
        return selectedDifficulty;
    }

    public float GetSpawnInterval()
    {
        switch (selectedDifficulty)
        {
            case Difficulty.IdleSlacker: return 2.3f;
            case Difficulty.AverageJoe: return 1.7f;
            case Difficulty.Goody2Shoes: return 1.3f;
            case Difficulty.Perfectionist: return 1.0f;
            default: return 1.7f;
        }
    }

    public int GetMaxBugsOnScreen()
    {
        switch (selectedDifficulty)
        {
            case Difficulty.IdleSlacker: return 4;
            case Difficulty.AverageJoe: return 6;
            case Difficulty.Goody2Shoes: return 8;
            case Difficulty.Perfectionist: return 10;
            default: return 6;
        }
    }

    public float GetTargetBugScore()
    {
        switch (selectedDifficulty)
        {
            case Difficulty.IdleSlacker: return 50f;
            case Difficulty.AverageJoe: return 70f;
            case Difficulty.Goody2Shoes: return 85f;
            case Difficulty.Perfectionist: return 100f;
            default: return 70f;
        }
    }

    public float GetPassThreshold()
    {
        return 75f;
    }
}