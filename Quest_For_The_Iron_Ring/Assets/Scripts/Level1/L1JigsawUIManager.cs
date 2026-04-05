using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class L1JigsawUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI correctText;

    [Header("Finish Panel")]
    public GameObject finishPanel;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI gradeText;

    [Header("Scene Transition")]
    public string nextSceneName = "Hallway_Scene";

    private bool levelComplete = false;

    private void Start()
    {
        UpdateTimerUI();
        UpdateCorrectUI();

        if (finishPanel != null)
        {
            finishPanel.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateTimerUI();

        // Optional: allow Enter key to go back after finishing
        if (levelComplete && Input.GetKeyDown(KeyCode.Return))
        {
            ReturnToHallway();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null || GlobalGameManager.Instance == null)
            return;

        int minutes = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime / 60f);
        int seconds = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime % 60f);

        timerText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void AddCorrectPlacement()
    {
        if (GlobalGameManager.Instance == null)
            return;

        GlobalGameManager.Instance.correctPlacements++;
        UpdateCorrectUI();
    }

    public void UpdateCorrectUI()
    {
        if (correctText == null || GlobalGameManager.Instance == null)
            return;

        correctText.text = "Correct Placements: " +
                           GlobalGameManager.Instance.correctPlacements +
                           " / " +
                           GlobalGameManager.Instance.totalPieces;
    }

    public void ShowFinishPanel()
    {
        if (GlobalGameManager.Instance == null)
            return;

        GlobalGameManager.Instance.StopTimer();
        GlobalGameManager.Instance.finalGrade = GlobalGameManager.Instance.CalculateGrade();
        GlobalGameManager.Instance.puzzleCompleted = true;

        int minutes = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime / 60f);
        int seconds = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime % 60f);

        if (finalTimeText != null)
        {
            finalTimeText.text = "Final Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        if (gradeText != null)
        {
            gradeText.text = "Grade: " + GlobalGameManager.Instance.finalGrade;
        }

        if (finishPanel != null)
        {
            finishPanel.SetActive(true);
        }

        levelComplete = true;
    }

    public void ReturnToHallway()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}