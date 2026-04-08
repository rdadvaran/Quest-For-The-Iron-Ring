using UnityEngine;

public class FastBug : Bug
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            base.OnCollisionEnter2D(collision);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RegisterFastBugHit();
            }

            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int amount)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(0.5f);
        }

        Destroy(gameObject);
    }
}