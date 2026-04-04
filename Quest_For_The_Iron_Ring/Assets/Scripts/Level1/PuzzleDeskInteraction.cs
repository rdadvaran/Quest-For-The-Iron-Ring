using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PuzzleDeskInteraction : MonoBehaviour
{
    public string puzzleSceneName = "Jigsaw_Puzzle";
    public TextMeshProUGUI promptText;

    private bool playerInRange = false;
    private LevelUIManager uiManager;

    private void Start()
    {
        uiManager = FindFirstObjectByType<LevelUIManager>();

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange)
        {
            HidePrompt();
            return;
        }

        if (uiManager == null)
        {
            HidePrompt();
            return;
        }

        if (uiManager.AreAllPiecesCollected())
        {
            ShowPrompt("Press E to start puzzle");

            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(puzzleSceneName);
            }
        }
        else
        {
            ShowPrompt("Collect all puzzle pieces first");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HidePrompt();
        }
    }

    private void ShowPrompt(string message)
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = message;
        }
    }

    private void HidePrompt()
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }
}