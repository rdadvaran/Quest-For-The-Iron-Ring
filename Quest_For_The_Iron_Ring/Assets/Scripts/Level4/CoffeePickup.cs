using UnityEngine;

public class CoffeePickup : MonoBehaviour
{
    [SerializeField] private int reduceBurnoutAmount = 1;
    [SerializeField] private float timeBonus = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReduceBurnout(reduceBurnoutAmount);
            GameManager.Instance.AddTime(timeBonus);
        }

        Destroy(gameObject);
    }
}