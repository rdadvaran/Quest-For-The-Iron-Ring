using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class MarkSaver : MonoBehaviour
{
    public static MarkSaver Instance;

    private Dictionary<string, float> grades = new Dictionary<string, float>();

    private string[] levelNames = { "Level1", "Level2", "Level3", "Level4", "Level5"};

    [SerializeField] private float passingGrade = 60f;

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
        grade = Mathf.Clamp(grade, 0f, 100f);
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

    public bool HasGrade(string levelName)
    {
        return GetGrade(levelName) >= 0f;
    }

    public bool HasPassedLevel(string levelName)
    {
        float grade = GetGrade(levelName);
        return grade >= passingGrade;
    }

    // If already passed, they should not enter again
    public bool CanEnterLevel(string levelName)
    {
        return !HasPassedLevel(levelName);
    }

    public bool HasPassedAllLevels()
    {
        foreach (string level in levelNames)
        {
            if (!HasPassedLevel(level))
                return false;
        }

        return true;
    }

    public int GetPassedLevelCount()
    {
        int count = 0;

        foreach (string level in levelNames)
        {
            if (HasPassedLevel(level))
                count++;
        }

        return count;
    }

    public string GetAllGradesText()
    {
        StringBuilder sb = new StringBuilder();

        foreach (string level in levelNames)
        {
            float grade = GetGrade(level);

            if (grade < 0f)
            {
                sb.AppendLine(level + ": Not Completed");
            }
            else if (grade >= passingGrade)
            {
                sb.AppendLine(level + ": " + grade.ToString("F1") + "% (Passed)");
            }
            else
            {
                sb.AppendLine(level + ": " + grade.ToString("F1") + "% (Failed)");
            }
        }

        return sb.ToString();
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