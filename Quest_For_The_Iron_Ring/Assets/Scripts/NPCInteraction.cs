using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject talkPrompt;
    [SerializeField] private string message = "TA: Welcome to Amit Chakma Engineering Building. Are you ready for your first challenge?";
    [SerializeField] private float typingSpeed = 0.04f;

    private bool playerInRange = false;
    private bool isDialogueOpen = false;
    private bool isTyping = false;
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
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isDialogueOpen)
            {
                OpenDialogue();
            }
            else
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
        dialogueText.text = "";

        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
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

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            isTyping = false;
            isDialogueOpen = false;

            if (dialogueBox != null)
                dialogueBox.SetActive(false);

            if (dialogueText != null)
                dialogueText.text = "";

            if (talkPrompt != null)
                talkPrompt.SetActive(false);
        }
    }
}