using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;

    public float shootStartDelay = 5f;
    public float shootCooldown = 1.2f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public GameObject assignedDoor;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private int direction = 1;

    private float leftBound;
    private float rightBound;

    private bool canShoot = false;
    private float shootTimer = 0f;

    private int hp = 3;
    private bool isKnockedOut = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Invoke(nameof(EnableShooting), shootStartDelay);
    }

    public void SetPlatform(GameObject platform)
    {
        float width = 2.5f;
        float centerX = platform.transform.position.x;

        leftBound = centerX - width;
        rightBound = centerX + width;
    }

    void Update()
    {
        if (isKnockedOut) return;

        Move();

        if (canShoot)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0f)
            {
                TryShoot();
                shootTimer = shootCooldown;
            }
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (transform.position.x <= leftBound)
        {
            direction = 1;
        }
        else if (transform.position.x >= rightBound)
        {
            direction = -1;
        }
    }

    void EnableShooting()
    {
        canShoot = true;
        shootTimer = 0f;
    }

    void TryShoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float dx = player.transform.position.x - transform.position.x;
        float dy = Mathf.Abs(player.transform.position.y - transform.position.y);

        if (dy < 1.5f)
        {
            int shootDir = dx > 0 ? 1 : -1;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Bullet b = bullet.GetComponent<Bullet>();
            if (b != null)
            {
                b.direction = shootDir;
                b.isPlayerBullet = false;
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isKnockedOut) return;

        hp -= dmg;

        if (hp <= 0)
        {
            KnockOut();
        }
    }

    void KnockOut()
    {
        isKnockedOut = true;

        rb.linearVelocity = Vector2.zero;

        if (sr != null)
        {
            sr.color = Color.gray;
        }
    }
}