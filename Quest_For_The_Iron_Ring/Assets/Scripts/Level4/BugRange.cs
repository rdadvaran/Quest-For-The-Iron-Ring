using UnityEngine;

public class BugRange : MonoBehaviour
{
    private Bug parentBug;

    private void Awake()
    {
        parentBug = GetComponentInParent<Bug>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement player = collision.GetComponent<playerMovement>();
            if (player != null && parentBug != null)
            {
                player.SetNearbyBug(parentBug);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement player = collision.GetComponent<playerMovement>();
            if (player != null)
            {
                player.ClearNearbyBug(parentBug);
            }
        }
    }
}