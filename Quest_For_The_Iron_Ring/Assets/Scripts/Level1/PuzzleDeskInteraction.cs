using UnityEngine;
using TMPro;

public class PuzzleDeskInteraction : MonoBehaviour
{
    public string puzzleSceneName = "Jigsaw_Puzzle";
    public TextMeshProUGUI deskPromptText;

    private bool playerInRange = false;
    private LevelUIManager uiManager;
    private SceneFader sceneFader;

    private void Start()
    {
        uiManager = FindFirstObjectByType<LevelUIManager>();
        sceneFader = FindFirstObjectByType<SceneFader>();

        if (deskPromptText != null)
        {
            deskPromptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (deskPromptText == null || uiManager == null)
            return;

        if (playerInRange && uiManager.AreAllPiecesCollected())
        {
            deskPromptText.gameObject.SetActive(true);
            deskPromptText.text = "Press E to start puzzle";

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (sceneFader != null)
                {
                    sceneFader.FadeToScene(puzzleSceneName);
                }
            }
        }
        else
        {
            deskPromptText.gameObject.SetActive(false);
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

            if (deskPromptText != null)
            {
                deskPromptText.gameObject.SetActive(false);
            }
        }
    }
}