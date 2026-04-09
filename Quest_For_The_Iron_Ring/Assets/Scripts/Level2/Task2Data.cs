using System;

[Serializable]
public class Task2Data
{
    public string challengePrompt;
    public string incompleteCode;
    public string correctAnswer;
    public int difficultyLevel;
    public string hintText;
    public int attemptsAllowed = 1;
}