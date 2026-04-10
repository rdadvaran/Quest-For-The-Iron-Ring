using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level2Manager : MonoBehaviour
{
    [Header("Question Data")]
    public List<Task2Data> tasks = new List<Task2Data>();

    [Header("Managers")]
    public MarkManager markManager;
    public DeadlineManager deadlineManager;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text promptTitleText;
    public TMP_Text promptBodyText;
    public TMP_Text codeBlockText;
    public TMP_InputField answerInput;
    public GameObject resultPanel;
    public TMP_Text resultTitleText;
    public TMP_Text finalScoreText;

    [Header("Hacker Bonus")]
    public bool isHackerPlayer = false;
    public float bonusTimePercent = 0.2f;
    public int freeHints = 1;

    private int currentTaskIndex = 0;
    private bool challengeEnded = false;
    private bool hintUsed = false;

    void Start()
    {
        resultPanel.SetActive(false);

        markManager.Setup(tasks.Count);
        GlobalGameManager.Instance.ResetGameData();

        isHackerPlayer = GameSession.Instance != null && (GameSession.Instance.selectedCharacter == "Hacker_Player" || GameSession.Instance.selectedCharacter == "AI_Player");
        float baseTime = deadlineManager.GetTimeBasedOnDifficulty();
        if (isHackerPlayer)
        {
            baseTime += baseTime * bonusTimePercent;
        }

        deadlineManager.StartTimer(baseTime);

        ShowTask();
        UpdateScoreUI();
    }

    void Update()
    {
        if (challengeEnded) return;

        if (deadlineManager.IsTimeUp())
        {
            EndChallenge();
        }
    }

    void ShowTask()
    {
        if (currentTaskIndex >= tasks.Count)
        {
            EndChallenge();
            return;
        }

        Task2Data currentTask = tasks[currentTaskIndex];

        promptTitleText.text = "Question " + (currentTaskIndex + 1);
        promptBodyText.text = currentTask.challengePrompt;
        codeBlockText.text = currentTask.incompleteCode;
        answerInput.text = "";
        hintUsed = false;
    }

    public void SubmitAnswer()
    {
        if (challengeEnded) return;

        Task2Data currentTask = tasks[currentTaskIndex];
        string playerAnswer = answerInput.text.Trim();

        if (CheckAnswer(playerAnswer, currentTask.correctAnswer))
        {
            markManager.RegisterCorrect();
        }

        UpdateScoreUI();

        currentTaskIndex++;
        ShowTask();
    }

    bool CheckAnswer(string playerAnswer, string correctAnswer)
    {
        return playerAnswer.Trim().ToLower() == correctAnswer.Trim().ToLower();
    }

    public void UseHint()
    {
        if (challengeEnded) return;
        if (!isHackerPlayer) return;
        if (freeHints <= 0) return;
        if (hintUsed) return;

        Task2Data currentTask = tasks[currentTaskIndex];
        promptBodyText.text += "\n\nHint: " + currentTask.hintText;

        freeHints--;
        hintUsed = true;
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + markManager.CalculateMark().ToString("0") + "%";
    }

    void EndChallenge()
    {
        challengeEnded = true;

        deadlineManager.StopTimer();
        GlobalGameManager.Instance.StopTimer();

        float finalMark = markManager.CalculateMark();
        bool passed = markManager.CheckPass();

        GlobalGameManager.Instance.finalGrade = passed ? "PASS" : "FAIL";
        GlobalGameManager.Instance.puzzleCompleted = passed;

        resultPanel.SetActive(true);
        resultTitleText.text = passed ? "PASS" : "FAIL";
        finalScoreText.text = "Final Score: " + finalMark.ToString("0") + "%";
    }

    public void ReturnToHallway()
    {
        SceneManager.LoadScene("Hallway_Scene");
    }
}