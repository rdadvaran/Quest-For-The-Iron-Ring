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

    private int filesRemaining = 4;
    private bool levelEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateBurnout(3);
        UpdateFiles();
        warningText.text = "";
        centerText.text = "";
        subText.text = "";
    }

    public void UpdateBurnout(int hp)
    {
        burnoutText.text = "Burnout: " + hp;
    }

    public void UpdateFiles()
    {
        filesText.text = "Files Remaining: " + filesRemaining;

        if (!levelEnded && filesRemaining <= 0)
        {
            EndLevel();
        }
    }

    public void FileDestroyed(bool pushed)
    {
        if (levelEnded) return;

        if (pushed)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.enemiesPushed++;
            }
        }

        filesRemaining--;
        if(filesRemaining < 0)
        {
            filesRemaining = 0;
        }

        UpdateFiles();
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

        int pushed = 0;
        string difficulty = "";

        if (GameManager.Instance != null)
        {
            pushed = GameManager.Instance.enemiesPushed;
            difficulty = GameManager.Instance.selectedDifficulty;
        }

        bool passed = false;

        if ((difficulty == "Idle Slacker" || difficulty == "IdleSlacker") && pushed >= 2) passed = true;
        if ((difficulty == "Average Joe" || difficulty == "AverageJoe") && pushed >= 3) passed = true;
        if ((difficulty == "Goody 2 Shoes" || difficulty == "Goody2Shoes") && pushed >= 3) passed = true;
        if (difficulty == "Perfectionist" && pushed >= 4) passed = true;

        if(GameManager.Instance != null)
        {
            GameManager.Instance.isLevel5Completed = passed;
        }

        subText.gameObject.SetActive(true);
        subText.text = passed ? "You passed" : "You failed";

        Debug.Log("Difficulty = " + difficulty);
        Debug.Log("Enemies Pushed = " + pushed);
        Debug.Log("SubText = " + subText.text);
    }
}