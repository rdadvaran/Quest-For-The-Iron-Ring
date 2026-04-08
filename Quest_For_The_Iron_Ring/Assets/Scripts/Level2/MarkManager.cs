using UnityEngine;

public class MarkManager : MonoBehaviour
{
    public float passingGrade = 60f;
    public float currentMark = 0f;

    private int totalQuestions = 0;
    private int correctAnswers = 0;

    public void Setup(int questionCount)
    {
        totalQuestions = questionCount;
        correctAnswers = 0;
        currentMark = 0f;
    }

    public void RegisterCorrect()
    {
        correctAnswers++;
        CalculateMark();
    }

    public float CalculateMark()
    {
        if (totalQuestions <= 0) return 0f;

        currentMark = ((float)correctAnswers / totalQuestions) * 100f;
        return currentMark;
    }

    public bool CheckPass()
    {
        return currentMark >= passingGrade;
    }
}