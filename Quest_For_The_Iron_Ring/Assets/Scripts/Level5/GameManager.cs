using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string selectedCharacter = "Default";
    public string selectedDifficulty = "AverageJoe";

    public bool isLevel5Completed = false;

    private MainMenuActions menu;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        menu = FindObjectOfType<MainMenuActions>();

        if (menu != null)
        {
            selectedCharacter = menu.selectedCharacter;
            selectedDifficulty = menu.selectedDifficulty;
        }
        else
        {
            Debug.LogWarning("MainMenuActions not found!");
        }
    }

    public int GetPlayerMaxHP()
    {
        if (selectedCharacter == "Gym Rat" || selectedCharacter == "AI")
            return 5;

        return 3;
    }

    public bool HasDoubleJump()
    {
        return selectedCharacter == "Gym Rat" || selectedCharacter == "AI";
    }

    public float GetCarryTime()
    {
        if (selectedCharacter == "Gym Rat" || selectedCharacter == "AI")
            return 8f;

        return 5f;
    }

    public int GetPassThreshold()
    {
        switch (selectedDifficulty)
        {
            case "Idle Slacker":
                return 50;
            case "Average Joe":
                return 70;
            case "Goody 2 Shoes":
                return 85;
            case "Perfectionist":
                return 100;
        }

        return 70;
    }
}