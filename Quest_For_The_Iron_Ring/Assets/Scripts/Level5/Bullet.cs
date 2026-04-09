using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int direction = 1;

    public bool isPlayerBullet;

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerBullet && other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(1);
            }
            Destroy(gameObject);
        }

        if (!isPlayerBullet && other.CompareTag("Player"))
        {
            PlayerController p = other.GetComponent<PlayerController>();
            if (p != null)
            {
                p.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}