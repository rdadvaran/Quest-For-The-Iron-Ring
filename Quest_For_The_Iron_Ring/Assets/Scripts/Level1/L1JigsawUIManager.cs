using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class L1JigsawUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;      // Shows the live timer during the puzzle
    public TextMeshProUGUI correctText;    // Shows how many correct placements the player has made

    [Header("Finish Panel")]
    public GameObject finishPanel;         // Panel that appears when the puzzle is completed
    public TextMeshProUGUI finalTimeText;  // Shows the player's final completion time
    public TextMeshProUGUI gradeText;      // Shows final grade %, required %, and pass/fail result

    [Header("Scene Transition")]
    public string nextSceneName = "Hallway_Scene"; // Scene to return to after pressing E

    private bool levelComplete = false;    // Tracks whether the puzzle has been completed

    private void Start()
    {
        // Update UI right away when the scene starts
        UpdateTimerUI();
        UpdateCorrectUI();

        // Hide the finish panel at the start of the level
        if (finishPanel != null)
        {
            finishPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Keep the timer text updating every frame
        UpdateTimerUI();

        // After the level is complete, allow the player to press E to go back
        if (levelComplete && Input.GetKeyDown(KeyCode.E))
        {
            ReturnToHallway();
        }
    }

    private void UpdateTimerUI()
    {
        // Stop if references are missing
        if (timerText == null || GlobalGameManager.Instance == null)
            return;

        // Convert total seconds into minutes and seconds
        int minutes = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime / 60f);
        int seconds = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime % 60f);

        // Show time in MM:SS format
        timerText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void AddCorrectPlacement()
    {
        // Stop if the game manager does not exist
        if (GlobalGameManager.Instance == null)
            return;

        // Increase correct placement count by 1
        GlobalGameManager.Instance.correctPlacements++;

        // Refresh the UI text
        UpdateCorrectUI();
    }

    public void UpdateCorrectUI()
    {
        // Stop if references are missing
        if (correctText == null || GlobalGameManager.Instance == null)
            return;

        // Show current correct placements out of total puzzle pieces
        correctText.text = "Correct Placements: " +
                           GlobalGameManager.Instance.correctPlacements +
                           " / " +
                           GlobalGameManager.Instance.totalPieces;
    }

    public void ShowFinishPanel()
    {
        // Stop if the game manager does not exist
        if (GlobalGameManager.Instance == null)
            return;

        // Stop the timer and save final results
        GlobalGameManager.Instance.StopTimer();
        GlobalGameManager.Instance.finalGrade = GlobalGameManager.Instance.CalculateGrade();
        GlobalGameManager.Instance.puzzleCompleted = true;

        // Get pass/fail info from the game manager
        float requiredGrade = GlobalGameManager.Instance.GetPassingPercentage();
        bool didPass = GlobalGameManager.Instance.DidPlayerPass();

        // Convert final time into MM:SS
        int minutes = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime / 60f);
        int seconds = Mathf.FloorToInt(GlobalGameManager.Instance.totalTime % 60f);

        // Show final completion time
        if (finalTimeText != null)
        {
            finalTimeText.text = "Final Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        // Show final grade, required grade, and result
        if (gradeText != null)
        {
            gradeText.text =
                "Final Grade: " + GlobalGameManager.Instance.finalGrade +
                "\nRequired Grade: " + requiredGrade.ToString("0") + "%" +
                "\nResult: " + (didPass ? "PASS" : "FAIL");
        }

        // Open the finish panel
        if (finishPanel != null)
        {
            finishPanel.SetActive(true);
        }

        // Mark level as complete so E can return to hallway
        levelComplete = true;
    }

    public void ReturnToHallway()
    {
        // Load the next scene after the player presses E
        SceneManager.LoadScene(nextSceneName);
    }
}