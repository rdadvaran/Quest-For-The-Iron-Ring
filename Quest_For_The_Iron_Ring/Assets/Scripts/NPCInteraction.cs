using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject talkPrompt;

    private bool playerInRange = false;

    private void Start()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        if (talkPrompt != null)
            talkPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleDialogue();
        }
    }

    private void ToggleDialogue()
    {
        if (dialogueBox == null)
            return;

        bool isActive = dialogueBox.activeSelf;
        dialogueBox.SetActive(!isActive);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (talkPrompt != null)
                talkPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (dialogueBox != null)
                dialogueBox.SetActive(false);

            if (talkPrompt != null)
                talkPrompt.SetActive(false);
        }
    }
}