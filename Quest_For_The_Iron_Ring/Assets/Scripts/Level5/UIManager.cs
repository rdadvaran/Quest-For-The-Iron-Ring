using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI burnoutText;
    public TextMeshProUGUI filesText;
    public TextMeshProUGUI centerText;
    public TextMeshProUGUI subText;
    public TextMeshProUGUI warningText;

    private int totalEnemies = 4;
    private int remainingEnemies = 4;

    private bool levelEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateBurnout(3);
        UpdateFiles(0, 0);
        warningText.text = "";
    }

    public void UpdateBurnout(int hp)
    {
        burnoutText.text = "Burnout: " + hp;
    }

    public void UpdateFiles(int pushed, int missed)
    {
        remainingEnemies = totalEnemies - (pushed + missed);
        filesText.text = "Files remaining: " + remainingEnemies;

        if (!levelEnded && remainingEnemies <= 0)
        {
            EndLevel();
        }
    }

    public void ShowWrongDoor(bool show)
    {
        if (show)
        {
            warningText.text = "Merge Conflict";
            warningText.color = Color.red;
        }
        else
        {
            warningText.text = "";
        }
    }

    public void PlayerDied()
    {
        if (levelEnded) return;

        levelEnded = true;

        centerText.text = "Commit Failed";
        subText.text = "";
    }

    void EndLevel()
    {
        if (levelEnded) return;

        levelEnded = true;

        centerText.text = "Game Over";

        int pushed = GameManager.Instance.enemiesPushed;
        string difficulty = GameManager.Instance.selectedDifficulty;

        bool passed = false;

        if (difficulty == "Idle Slacker" && pushed >= 2) passed = true;
        if (difficulty == "Average Joe" && pushed >= 3) passed = true;
        if (difficulty == "Goody 2 Shoes" && pushed >= 3) passed = true;
        if (difficulty == "Perfectionist" && pushed >= 4) passed = true;

        subText.text = passed ? "You passed" : "You failed";
    }
}