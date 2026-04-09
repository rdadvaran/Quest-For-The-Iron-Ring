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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            characterType = GameManager.Instance.selectedCharacter;
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

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
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