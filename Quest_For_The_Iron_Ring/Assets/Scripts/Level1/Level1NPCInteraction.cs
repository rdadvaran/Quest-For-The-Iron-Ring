using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Level1NPCInteraction : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialogueBox;   // The full dialogue panel
    [SerializeField] private TMP_Text dialogueText;    // The text inside the dialogue panel
    [SerializeField] private GameObject talkPrompt;    // Small prompt like "Press E to talk"

    [Header("Dialogue Settings")]
    [TextArea(3, 6)]
    [SerializeField] private string message = "TA: Welcome! Are you ready for your first challenge?\n\nPress Y for Yes or N for No."; 
    // Message shown if the player has NOT passed the level yet

    [TextArea(3, 6)]
    [SerializeField] private string passedMessage = "TA: You already passed this level, so you do not need to do it again.";
    // Message shown if the player already passed the level

    [SerializeField] private float typingSpeed = 0.04f; 
    // Speed for typing effect

    [Header("Level Settings")]
    [SerializeField] private string sceneToLoad = "L1_Clasroom"; 
    // Exact name of the classroom scene to load

    [SerializeField] private string levelKey = "Level1Passed";   
    // PlayerPrefs key used to remember if this level was passed

    private bool playerInRange = false;     
    // True when the player is inside the NPC trigger

    private bool isDialogueOpen = false;    
    // True when the dialogue box is currently open

    private bool isTyping = false;          
    // True while the text is being typed letter by letter

    private bool waitingForChoice = false;  
    // True when player is allowed to press Y or N

    private Coroutine typingCoroutine;      
    // Stores the running typing coroutine so it can be stopped if needed

    private void Start()
    {
        // Hide dialogue box when scene starts
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        // Hide talk prompt when scene starts
        if (talkPrompt != null)
            talkPrompt.SetActive(false);

        // Clear dialogue text when scene starts
        if (dialogueText != null)
            dialogueText.text = "";
    }

    private void Update()
    {
        // Stop if keyboard is not available
        if (Keyboard.current == null) return;

        // Press E to open dialogue when player is in range
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame && !isDialogueOpen)
        {
            OpenDialogue();
            return;
        }

        // If dialogue is open and finished typing, allow Y/N choice
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

        // If player already passed, no Y/N choice is needed
        // Let them close that message with E or N
        if (isDialogueOpen && !isTyping && !waitingForChoice)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.nKey.wasPressedThisFrame)
            {
                CloseDialogue();
            }
        }
    }

    private void OpenDialogue()
    {
        // Stop if required UI references are missing
        if (dialogueBox == null || dialogueText == null)
            return;

        // Show the dialogue panel
        dialogueBox.SetActive(true);
        isDialogueOpen = true;
        waitingForChoice = false;

        // Hide the "Press E" prompt while dialogue is open
        if (talkPrompt != null)
            talkPrompt.SetActive(false);

        // Stop previous typing coroutine if one is already running
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Check if the player already passed this level
        bool alreadyPassed = PlayerPrefs.GetInt(levelKey, 0) == 1;

        // Show a different message depending on pass status
        if (alreadyPassed)
        {
            typingCoroutine = StartCoroutine(TypeText(passedMessage, false));
        }
        else
        {
            typingCoroutine = StartCoroutine(TypeText(message, true));
        }
    }

    private void CloseDialogue()
    {
        // Stop typing if still running
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Reset dialogue states
        isTyping = false;
        isDialogueOpen = false;
        waitingForChoice = false;

        // Hide dialogue box
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        // Clear dialogue text
        if (dialogueText != null)
            dialogueText.text = "";

        // Re-show talk prompt if player is still near the NPC
        if (playerInRange && talkPrompt != null)
            talkPrompt.SetActive(true);
    }

    private IEnumerator TypeText(string textToType, bool allowChoiceAfter)
    {
        // Start typing effect
        isTyping = true;
        waitingForChoice = false;
        dialogueText.text = "";

        // Add one character at a time
        foreach (char letter in textToType)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Typing is finished
        isTyping = false;

        // Only allow Y/N choice if this message should have it
        waitingForChoice = allowChoiceAfter;
    }

    private void StartLevel()
    {
        // Check if the player already passed the level
        bool alreadyPassed = PlayerPrefs.GetInt(levelKey, 0) == 1;

        // If already passed, do not load the level again
        if (alreadyPassed)
            return;

        // Make sure the scene name is not empty
        if (string.IsNullOrWhiteSpace(sceneToLoad))
        {
            Debug.LogWarning("Scene name is empty on " + gameObject.name);
            return;
        }

        // Load the classroom scene
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the thing entering is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Show talk prompt if dialogue is not already open
            if (!isDialogueOpen && talkPrompt != null)
                talkPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the thing leaving is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Close dialogue if player walks away
            CloseDialogue();

            // Hide talk prompt
            if (talkPrompt != null)
                talkPrompt.SetActive(false);
        }
    }
}