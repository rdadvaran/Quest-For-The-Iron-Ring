using UnityEngine;

public class L1JigsawCharacterBuff : MonoBehaviour
{
    [SerializeField] private GameObject umlDiagramPreview; // Image shown in bottom-right

    private void Start()
    {
        // Hide the image by default
        if (umlDiagramPreview != null)
        {
            umlDiagramPreview.SetActive(false);
        }

        // Stop if GameSession is missing
        if (GameSession.Instance == null)
            return;

        // Get the selected character
        string selectedCharacter = GameSession.Instance.selectedCharacter;

        // Show the image only for Artist or AI
        if (selectedCharacter == "Artist_Player" || selectedCharacter == "AI_Player")
        {
            if (umlDiagramPreview != null)
            {
                umlDiagramPreview.SetActive(true);
            }

            Debug.Log("Jigsaw UML preview enabled for: " + selectedCharacter);
        }
    }
}