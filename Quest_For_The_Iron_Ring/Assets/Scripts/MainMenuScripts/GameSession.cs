using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    public string selectedCharacter = "Basic";
    public string selectedDifficulty = "Average Joe";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}