using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject characterMenuPanel;
    public GameObject infoMenuPanel;
    public GameObject difficultyMenuPanel;

    public string selectedCharacter = "Basic";
    public string selectedDifficulty = "Average Joe";

    // Start game
    public void StartGame()
    {
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
        selectedCharacter = character;
        Debug.Log("Selected Character: " + selectedCharacter);
    }

    // Difficulty selection logic
    public void SelectDifficulty(string difficulty)
    {
        selectedDifficulty = difficulty;
        Debug.Log("Selected Difficulty: " + selectedDifficulty);
    }
}