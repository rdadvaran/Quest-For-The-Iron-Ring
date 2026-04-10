using UnityEngine;

public class PlayerController3 : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    private int currentHealth;
    private Task3 levelManager;
    private bool initialized = false;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (initialized) return;

        currentHealth = maxHealth;
        levelManager = FindObjectOfType<Task3>();
        initialized = true;
    }

    public void UpdateHealth(int amount)
    {
        if (!initialized)
        {
            Initialize();
        }

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
        if (!initialized)
        {
            Initialize();
        }

        IconBehavior icon = other.GetComponent<IconBehavior>();
        if (icon != null)
        {
            icon.Collect();
        }
    }
}