using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class L1ClassroomDeskInteraction : MonoBehaviour
{
    public string puzzleSceneName = "L1_JigsawPuzzle";
    public TextMeshProUGUI deskPromptText;

    private bool playerInRange = false;
    private L1ClassroomUIManager uiManager;

    private void Start()
    {
        uiManager = FindFirstObjectByType<L1ClassroomUIManager>();

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
                SceneManager.LoadScene(puzzleSceneName);
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