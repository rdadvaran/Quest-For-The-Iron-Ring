using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int leve5Grade = 0;

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

    void ReturnToHallway()
    {
        SceneManager.LoadScene("Hallway_Scene");
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
            GameManager5.Instance.enemiesPushed++;
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

        Invoke(nameof(ReturnToHallway), 5f);
    }

    void EndLevel()
    {
        if (levelEnded) return;

        levelEnded = true;

        centerText.text = "Game Over";

        int pushed = 0;
        string difficulty = "AverageJoe";

        if (GameManager5.Instance != null)
        {
            pushed = GameManager5.Instance.enemiesPushed;
            difficulty = GameManager5.Instance.selectedDifficulty;
        }

        bool passed = false;

        if ((difficulty == "Idle Slacker" || difficulty == "IdleSlacker") && pushed == 1)
        {
            passed = true;
            leve5Grade = 50;
            GameManager5.Instance.isLevel5Completed = true;
        }
        if ((difficulty == "Average Joe" || difficulty == "AverageJoe") && pushed == 2)
        {
            passed = true;
            leve5Grade = 70;
            GameManager5.Instance.isLevel5Completed = true;
        }
        if ((difficulty == "Goody 2 Shoes" || difficulty == "Goody2Shoes") && pushed == 3)
        {
            passed = true;
            leve5Grade = 85;
            GameManager5.Instance.isLevel5Completed = true;
        }
        if (difficulty == "Perfectionist" && pushed >= 4)
        {
            passed = true;
            leve5Grade = 100;
            GameManager5.Instance.isLevel5Completed = true;
        }

        subText.gameObject.SetActive(true);
        subText.text = passed ? "You passed" : "You failed";

        Debug.Log("Difficulty = " + difficulty);
        Debug.Log("Enemies Pushed = " + pushed);
        Debug.Log("SubText = " + subText.text);

        Invoke(nameof(ReturnToHallway), 5f);
    }
}