using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float boostedMoveSpeed = 15f;
    [SerializeField] private string boostedLevelName = "Level4";

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private Bug nearbyBug;
    private float currentMoveSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentMoveSpeed = moveSpeed;
    }

    void Start()
    {
        ApplyCharacterBoost();
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
            rb.linearVelocity = moveInput * currentMoveSpeed;
        }
    }

    private void ApplyCharacterBoost()
    {
        currentMoveSpeed = moveSpeed;

        bool isLevelWithBoost = SceneManager.GetActiveScene().name == boostedLevelName;

        if (!isLevelWithBoost)
            return;

        if (CharacterManager.Instance == null)
            return;

        if (CharacterManager.Instance.HasLevel4SpeedBoost())
        {
            currentMoveSpeed = boostedMoveSpeed;
            Debug.Log("Level 4 character speed boost applied. Speed = " + currentMoveSpeed);
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

    // for level 3
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
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