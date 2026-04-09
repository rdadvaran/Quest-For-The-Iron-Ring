using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEndManager : MonoBehaviour
{
    public static LevelEndManager Instance;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text endMessageText;
    [SerializeField] private TMP_Text finalGradeText;

    [SerializeField] private float returnDelay = 3f;
    [SerializeField] private string hallwaySceneName = "Hallway_Scene";

    [SerializeField] private float targetBugScore = 20f;
    [SerializeField] private float passThreshold = 75f;
    [SerializeField] private float bugWeight = 0.7f;
    [SerializeField] private float burnoutWeight = 0.3f;

    private bool endingTriggered = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (endPanel != null)
            endPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        if (endingTriggered) return;
        StartCoroutine(ShowEndScreenAndReturn("GAME OVER", ""));
    }

    public void TriggerPassFail(float score, int burnoutLevel, int maxBurnout)
    {
        if (endingTriggered) return;

        float finalGrade = CalculateFinalGrade(score, burnoutLevel, maxBurnout);

        string resultMessage = finalGrade >= passThreshold ? "YOU PASSED" : "YOU FAILED";
        string gradeMessage = "Grade: " + finalGrade.ToString("0") + "%";

        StartCoroutine(ShowEndScreenAndReturn(resultMessage, gradeMessage));
    }

    private float CalculateFinalGrade(float score, int burnoutLevel, int maxBurnout)
    {
        float bugPercent = 0f;

        if (targetBugScore > 0f)
            bugPercent = Mathf.Clamp01(score / targetBugScore) * 100f;

        float burnoutPercent = (1f - ((float)burnoutLevel / maxBurnout)) * 100f;

        float finalPercent = (bugPercent * bugWeight) + (burnoutPercent * burnoutWeight);

        return Mathf.Clamp(finalPercent, 0f, 100f);
    }

    private IEnumerator ShowEndScreenAndReturn(string message, string grade)
    {
        endingTriggered = true;
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
}