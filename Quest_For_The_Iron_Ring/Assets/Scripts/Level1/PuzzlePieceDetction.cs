using UnityEngine;

public class L1PuzzlePiece : MonoBehaviour
{
    private L1ClassroomUIManager uiManager;

    private void Start()
    {
        uiManager = FindFirstObjectByType<L1ClassroomUIManager>();
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