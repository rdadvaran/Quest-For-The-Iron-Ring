using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.PlayerDied();
            }

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.FileDestroyed(false);
            }

            Destroy(other.gameObject);
            return;
        }

        Destroy(other.gameObject);
    }
}