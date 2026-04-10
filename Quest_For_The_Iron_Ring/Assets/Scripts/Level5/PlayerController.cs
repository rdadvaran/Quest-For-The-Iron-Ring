using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;

    private GameObject bulletPrefab;
    private Transform firePoint;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Collider2D col;

    private bool isGrounded;
    private bool wasGrounded;
    private bool canDoubleJump;

    private int facingDirection = 1;

    private string characterType;

    private bool isInitialized = false;

    private int hp = 3;

    private Enemy carriedEnemy = null;
    private float carryTimer = 0f;
    private float carryDuration = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (GameManager5.Instance != null)
        {
            characterType = GameManager5.Instance.selectedCharacter;
        }
    }

    public void Initialize(GameObject bullet, Transform fire)
    {
        bulletPrefab = bullet;
        firePoint = fire;
        isInitialized = true;
    }

    void Update()
    {
        Move();
        Jump();
        Shoot();
        HandleCarry();
    }

    void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move != 0)
        {
            facingDirection = (int)Mathf.Sign(move);
        }

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        isGrounded = CheckGrounded();

        if (isGrounded && !wasGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (canDoubleJump && (characterType == "GymRat_Player" || characterType == "AI_Player"))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

        wasGrounded = isGrounded;
    }

    bool CheckGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer) != null;
    }

    void Shoot()
    {
        if (!isInitialized) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Bullet b = bullet.GetComponent<Bullet>();
            if (b != null)
            {
                b.direction = facingDirection;
                b.isPlayerBullet = true;
            }
        }
    }

    void HandleCarry()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (carriedEnemy == null)
            {
                TryPickUp();
            }
            else
            {
                DropEnemy();
            }
        }

        if (carriedEnemy != null)
        {
            carryTimer -= Time.deltaTime;

            Vector3 offset = new Vector3(0.8f * facingDirection, 0f, 0f);
            carriedEnemy.transform.position = transform.position + offset;

            if (carryTimer <= 0f)
            {
                DropEnemy();
            }
        }
    }

    void TryPickUp()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (Collider2D hit in hits)
        {
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null && e.IsKnockedOut())
            {
                carriedEnemy = e;
                carryTimer = carryDuration;

                Collider2D enemyCol = e.GetComponent<Collider2D>();
                if (enemyCol != null)
                {
                    enemyCol.enabled = false;
                }

                return;
            }
        }
    }

    void DropEnemy()
    {
        if (carriedEnemy != null)
        {
            Collider2D enemyCol = carriedEnemy.GetComponent<Collider2D>();
            if (enemyCol != null)
            {
                enemyCol.enabled = true;
            }
        }

        carriedEnemy = null;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBurnout(hp);
        }
        
        if (hp <= 0)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.PlayerDied();
            }
            
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}