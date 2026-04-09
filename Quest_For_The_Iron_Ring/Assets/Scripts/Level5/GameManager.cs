using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string selectedCharacter = "Player";
    public string selectedDifficulty = "AverageJoe";

    public int enemiesMissed = 0;

    public bool isLevel5Completed = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (GameSession.Instance != null)
        {
            selectedCharacter = GameSession.Instance.selectedCharacter;
            selectedDifficulty = GameSession.Instance.selectedDifficulty;
        }
    }

    public int GetPlayerMaxHP()
    {
        if (selectedCharacter == "GymRat_Player" || selectedCharacter == "AI_Player")
            return 5;

        return 3;
    }

    public bool HasDoubleJump()
    {
        return selectedCharacter == "GymRat_Player" || selectedCharacter == "AI_Player";
    }

    public float GetCarryTime()
    {
        if (selectedCharacter == "GymRat_Player" || selectedCharacter == "AI_Player")
            return 8f;

        return 5f;
    }

    public int GetPassThreshold()
    {
        switch (selectedDifficulty)
        {
            case "IdleSlacker":
                return 50;
            case "AverageJoe":
                return 70;
            case "Goody2Shoes":
                return 85;
            case "Perfectionist":
                return 100;
        }

        return 70;
    }
}