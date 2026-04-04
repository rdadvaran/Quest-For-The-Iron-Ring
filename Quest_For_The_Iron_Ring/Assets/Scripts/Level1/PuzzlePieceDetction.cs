using UnityEngine;

public class PuzzlePieceDetection : MonoBehaviour
{
    private LevelUIManager uiManager;

    private void Start()
    {
        uiManager = FindFirstObjectByType<LevelUIManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (uiManager != null)
            {
                uiManager.CollectPiece();
            }

            Destroy(gameObject);
        }
    }
}