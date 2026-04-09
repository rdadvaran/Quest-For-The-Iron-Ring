using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float boostedMoveSpeed = 6.5f;

    // Plane bounds
    [SerializeField] private float minX = -5.6f;
    [SerializeField] private float maxX = 5.6f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private float currentMoveSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentMoveSpeed = baseMoveSpeed;

        // Speed boost for Gamer or AI character
        if (GameSession.Instance != null)
        {
            string selected = GameSession.Instance.selectedCharacter;

            if (selected == "Gamer_Player" || selected == "AI_Player")
            {
                currentMoveSpeed = boostedMoveSpeed;
            }
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = moveInput * currentMoveSpeed;

            // Clamp player position so they cannot leave the plane
            Vector2 clampedPos = rb.position;
            clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
            clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
            rb.position = clampedPos;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (animator != null)
        {
            bool isMoving = moveInput != Vector2.zero;
            animator.SetBool("isMoving", isMoving);
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            if (isMoving)
            {
                animator.SetFloat("LastInputX", moveInput.x);
                animator.SetFloat("LastInputY", moveInput.y);
            }
        }
    }
}