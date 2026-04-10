using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject talkPrompt;

    [Header("Dialogue Settings")]
    [TextArea(3, 6)]
    [SerializeField] private string message = "TA: Welcome! Are you ready for your first challenge?\n\nPress Y for Yes or N for No.";
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Level Info")]
    [SerializeField] private string levelKey = "Level1";
    [SerializeField] private string sceneToLoad = "L1_Classroom";

    private bool playerInRange = false;
    private bool isDialogueOpen = false;
    private bool isTyping = false;
    private bool waitingForChoice = false;

    private Coroutine typingCoroutine;

    private void Start()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        if (talkPrompt != null)
            talkPrompt.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        // Press E to open dialogue when near the NPC
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame && !isDialogueOpen)
        {
            OpenDialogue();
            return;
        }

        // After dialogue finishes typing, allow Y/N choice
        if (isDialogueOpen && !isTyping && waitingForChoice)
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                StartLevel();
            }
            else if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                CloseDialogue();
            }
        }
    }

    private void OpenDialogue()
    {
        if (dialogueBox == null || dialogueText == null)
            return;

        dialogueBox.SetActive(true);
        isDialogueOpen = true;
        waitingForChoice = false;

        if (talkPrompt != null)
            talkPrompt.SetActive(false);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private void CloseDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        isTyping = false;
        isDialogueOpen = false;
        waitingForChoice = false;

        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";

        if (playerInRange && talkPrompt != null)
            talkPrompt.SetActive(true);
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        waitingForChoice = false;
        dialogueText.text = "";

        // Type the message one letter at a time
        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        waitingForChoice = true;
    }

    private void StartLevel()
    {
        if (string.IsNullOrWhiteSpace(sceneToLoad))
        {
            Debug.LogWarning("Scene name is empty on " + gameObject.name);
            return;
        }

        if (MarkSaver.Instance != null && !MarkSaver.Instance.CanEnterLevel(levelKey))
        {
            if (dialogueText != null)
            {
                dialogueText.text = "You already passed this level, so this room is locked.";
            }

            waitingForChoice = false;
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!isDialogueOpen && talkPrompt != null)
                talkPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CloseDialogue();

            if (talkPrompt != null)
                talkPrompt.SetActive(false);
        }
    }
}