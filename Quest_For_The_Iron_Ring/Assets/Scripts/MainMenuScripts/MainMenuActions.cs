using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject characterMenuPanel;
    public GameObject infoMenuPanel;
    public GameObject difficultyMenuPanel;

    // Start game
    public void StartGame()
    {
        if (GameSession.Instance != null)
        {
            Debug.Log("Starting game with character: " + GameSession.Instance.selectedCharacter);
            Debug.Log("Starting game with difficulty: " + GameSession.Instance.selectedDifficulty);
        }

        SceneManager.LoadScene("Hallway_Scene");
    }


    // Opening menus
    public void OpenCharacterMenu()
    {
        characterMenuPanel.SetActive(true);
    }

    public void OpenInfoMenu()
    {
        infoMenuPanel.SetActive(true);
    }

    public void OpenDifficultyMenu()
    {
        difficultyMenuPanel.SetActive(true);
    }

    // Closing menus
    public void CloseAllMenus()
    {
        characterMenuPanel.SetActive(false);
        infoMenuPanel.SetActive(false);
        difficultyMenuPanel.SetActive(false);
    }

    // Character selection logic
    public void SelectCharacter(string character)
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.selectedCharacter = character;
        }

        Debug.Log("Selected Character: " + character);
    }

    
    // Difficulty selection logic
    public void SelectDifficulty(string difficulty)
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.selectedDifficulty = difficulty;
        }

        Debug.Log("Selected Difficulty: " + difficulty);
    }

    //To keep character and difficulty variables
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}