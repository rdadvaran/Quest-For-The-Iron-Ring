using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.enemiesMissed++;

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateFiles(GameManager.Instance.enemiesPushed, GameManager.Instance.enemiesMissed);
            }
        }

        Destroy(other.gameObject);
    }
}