using UnityEngine;

public class Bug : MonoBehaviour
{
    [Header("Bug Stats")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private int burnoutDamage = 1;
    [SerializeField] private float scoreValue = 1f;
    [SerializeField] private int health = 1;

    [Header("Animation Frames")]
    [SerializeField] private Sprite[] bugDownFrames;
    [SerializeField] private Sprite[] bugLeftFrames;
    [SerializeField] private Sprite[] bugRightFrames;
    [SerializeField] private Sprite[] bugUpFrames;

    [SerializeField] private float animationSpeed = 0.15f;
    [SerializeField] private GameObject killPrompt;

    protected Vector2 direction;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float animationTimer;
    private int currentFrame;

    private enum FacingDirection
    {
        Down,
        Left,
        Right,
        Up
    }

    private FacingDirection currentFacing = FacingDirection.Down;
    private FacingDirection lastFacing = FacingDirection.Down;

    protected virtual void Start()
    {
        direction = Random.insideUnitCircle.normalized;

        if (direction == Vector2.zero)
            direction = Vector2.right;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateFacingDirection();
        ForceSetFirstFrame();
    }

    protected virtual void FixedUpdate()
    {
        direction = GetMovementDirection();

        if (rb != null)
        {
            rb.linearVelocity = direction * GetMoveSpeed();
        }

        UpdateFacingDirection();
        UpdateBugAnimation();
    }

    protected virtual Vector2 GetMovementDirection()
    {
        return direction;
    }

    protected virtual float GetMoveSpeed()
    {
        return speed;
    }

    private void UpdateFacingDirection()
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            currentFacing = direction.x > 0 ? FacingDirection.Right : FacingDirection.Left;
        }
        else
        {
            currentFacing = direction.y > 0 ? FacingDirection.Up : FacingDirection.Down;
        }

        if (currentFacing != lastFacing)
        {
            currentFrame = 0;
            animationTimer = 0f;
            ForceSetFirstFrame();
            lastFacing = currentFacing;
        }
    }

    private void ForceSetFirstFrame()
    {
        if (spriteRenderer == null)
            return;

        Sprite[] frames = GetCurrentDirectionFrames();

        if (frames != null && frames.Length > 0)
        {
            spriteRenderer.sprite = frames[0];
        }
    }

    private void UpdateBugAnimation()
    {
        if (spriteRenderer == null)
            return;

        Sprite[] currentFrames = GetCurrentDirectionFrames();

        if (currentFrames == null || currentFrames.Length == 0)
            return;

        animationTimer += Time.fixedDeltaTime;

        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame++;

            if (currentFrame >= currentFrames.Length)
                currentFrame = 0;

            spriteRenderer.sprite = currentFrames[currentFrame];
        }
    }

    private Sprite[] GetCurrentDirectionFrames()
    {
        switch (currentFacing)
        {
            case FacingDirection.Left:
                return bugLeftFrames;
            case FacingDirection.Right:
                return bugRightFrames;
            case FacingDirection.Up:
                return bugUpFrames;
            default:
                return bugDownFrames;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            direction = Vector2.Reflect(direction, normal).normalized;

            direction += Random.insideUnitCircle * 0.15f;
            direction = direction.normalized;

            if (direction == Vector2.zero)
                direction = Vector2.right;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddBurnout(burnoutDamage);
            }

            Destroy(gameObject);
        }
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
            }

            Destroy(gameObject);
        }
    }

    public void ShowKillPrompt()
    {
        if (killPrompt != null)
            killPrompt.SetActive(true);
    }

    public void HideKillPrompt()
    {
        if (killPrompt != null)
            killPrompt.SetActive(false);
    }
}