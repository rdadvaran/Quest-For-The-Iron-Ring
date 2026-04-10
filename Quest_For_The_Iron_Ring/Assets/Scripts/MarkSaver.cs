using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class MarkSaver : MonoBehaviour
{
    public static MarkSaver Instance;

    private Dictionary<string, float> grades = new Dictionary<string, float>();

    // Put all your challenge names here
    private string[] levelNames = { "Level1", "Level2", "Level3", "Level4", "Level5" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGrade(string levelName, float grade)
    {
        grades[levelName] = grade;
        PlayerPrefs.SetFloat(levelName, grade);
        PlayerPrefs.Save();
    }

    public float GetGrade(string levelName)
    {
        if (grades.ContainsKey(levelName))
            return grades[levelName];

        if (PlayerPrefs.HasKey(levelName))
            return PlayerPrefs.GetFloat(levelName);

        return -1f;
    }

    private void LoadGrades()
    {
        foreach (string level in levelNames)
        {
            if (PlayerPrefs.HasKey(level))
            {
                grades[level] = PlayerPrefs.GetFloat(level);
            }
        }
    }

    public string GetAllGradesText()
    {
        StringBuilder sb = new StringBuilder();

        foreach (string level in levelNames)
        {
            float grade = GetGrade(level);

            if (grade >= 0)
                sb.AppendLine(level + ": " + grade.ToString("F1") + "%");
            else
                sb.AppendLine(level + ": Not Completed");
        }

        return sb.ToString();
    }

    public void ResetGrades()
    {
        grades.Clear();

        foreach (string level in levelNames)
        {
            PlayerPrefs.DeleteKey(level);
        }

        PlayerPrefs.Save();
    }
}