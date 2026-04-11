using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEndManager : MonoBehaviour
{
    public static LevelEndManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text endMessageText;
    [SerializeField] private TMP_Text finalGradeText;

    [Header("Scene Return Settings")]
    [SerializeField] private float returnDelay = 3f;
    [SerializeField] private string hallwaySceneName = "Hallway_Scene";

    [Header("Grading Settings")]
    [SerializeField] private float targetBugScore = 100f;
    [SerializeField] private float passThreshold = 75f;
    [SerializeField] private float bugWeight = 0.75f;
    [SerializeField] private float burnoutWeight = 0.25f;

    [Header("Save Settings")]
    [SerializeField] private string levelKey = "Level4_BugSquasher";

    private bool endingTriggered = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate LevelEndManager found. Destroying extra instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("LevelEndManager: End Panel is not assigned.");
        }

        if (endMessageText == null)
            Debug.LogWarning("LevelEndManager: End Message Text is not assigned.");

        if (finalGradeText == null)
            Debug.LogWarning("LevelEndManager: Final Grade Text is not assigned.");
    }

    public void TriggerGameOver()
    {
        if (endingTriggered) return;

        endingTriggered = true;
        Debug.Log("LevelEndManager: TriggerGameOver called.");

        StartCoroutine(ShowEndScreenAndReturn("GAME OVER", ""));
    }

    public void TriggerPassFail(float score, int burnoutLevel, int maxBurnout)
    {
        if (endingTriggered) return;

        endingTriggered = true;

        float finalGrade = CalculateFinalGrade(score, burnoutLevel, maxBurnout);
        string resultMessage = finalGrade >= passThreshold ? "YOU PASSED" : "YOU FAILED";
        string gradeMessage = "Grade: " + finalGrade.ToString("0") + "%";

        Debug.Log($"LevelEndManager: TriggerPassFail called | Score: {score}, Burnout: {burnoutLevel}/{maxBurnout}, Final Grade: {finalGrade}");

        StartCoroutine(ShowEndScreenAndReturn(resultMessage, gradeMessage));
    }

    private float CalculateFinalGrade(float score, int burnoutLevel, int maxBurnout)
    {
        float safeTargetBugScore = Mathf.Max(1f, targetBugScore);
        int safeMaxBurnout = Mathf.Max(1, maxBurnout);

        float bugPercent = Mathf.Clamp((score / safeTargetBugScore) * 100f, 0f, 100f);
        float burnoutPercent = Mathf.Clamp((1f - ((float)burnoutLevel / safeMaxBurnout)) * 100f, 0f, 100f);

        float finalPercent = (bugPercent * bugWeight) + (burnoutPercent * burnoutWeight);
        float clampedFinalPercent = Mathf.Clamp(finalPercent, 0f, 100f);

        if (MarkSaver.Instance != null)
        {
            MarkSaver.Instance.SaveGrade(levelKey, clampedFinalPercent);
        }
        else
        {
            Debug.LogWarning("MarkSaver.Instance is null. Grade was not saved, but end screen will still show.");
        }

        return clampedFinalPercent;
    }

    private IEnumerator ShowEndScreenAndReturn(string message, string grade)
    {
        Debug.Log($"LevelEndManager: Showing end screen | Message: {message} | Grade: {grade}");

        Time.timeScale = 0f;

        if (endPanel != null)
            endPanel.SetActive(true);

        if (endMessageText != null)
            endMessageText.text = message;

        if (finalGradeText != null)
            finalGradeText.text = grade;

        yield return new WaitForSecondsRealtime(returnDelay);

        Time.timeScale = 1f;
        SceneManager.LoadScene(hallwaySceneName);
    }

    [ContextMenu("Test Pass Screen")]
    private void TestPassScreen()
    {
        TriggerPassFail(18f, 1, 5);
    }

    [ContextMenu("Test Game Over Screen")]
    private void TestGameOverScreen()
    {
        TriggerGameOver();
    }
}