using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    private int currentHealth;
    private Task3 levelManager;

    public int CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        levelManager = FindObjectOfType<Task3>();
    }

    // Damage or heal the player
    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth <= 0 && levelManager != null)
        {
            levelManager.LoseLevel();
        }
    }

    public void Interact()
    {
        Debug.Log("Player interacted.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Pick up a logo
        IconBehavior icon = other.GetComponent<IconBehavior>();
        if (icon != null)
        {
            icon.Collect();
        }
    }
}