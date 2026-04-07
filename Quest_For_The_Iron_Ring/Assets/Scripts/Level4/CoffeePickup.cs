using UnityEngine;

public class CoffeePickup : MonoBehaviour
{
    [SerializeField] private int restoreAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Coffee picked up! Restore amount: " + restoreAmount);
        Destroy(gameObject);
    }
}