using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    [SerializeField] protected float lifetime = 8f;
    [SerializeField] protected int damage = 1;

    protected float timer = 0f;

    protected virtual void Update()
    {
        UpdatePosition();

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Despawn();
        }
    }

    protected virtual void UpdatePosition()
    {
        // Child classes replace this
    }

    protected virtual void Despawn()
    {
        Destroy(gameObject);
    }

    protected virtual void Collision(PlayerController player)
    {
        player.UpdateHealth(-damage);
        Despawn();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Collision(player);
        }
    }
}