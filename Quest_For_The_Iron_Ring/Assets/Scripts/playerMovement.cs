Fusing UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private Bug nearbyBug;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (nearbyBug != null && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            nearbyBug.TakeDamage(1);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = moveInput * moveSpeed;
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

    public void SetNearbyBug(Bug bug)
    {
        nearbyBug = bug;
    }

    public void ClearNearbyBug(Bug bug)
    {
        if (nearbyBug == bug)
        {
            nearbyBug = null;
        }
    }
}