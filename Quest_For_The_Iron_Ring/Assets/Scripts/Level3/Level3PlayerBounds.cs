using UnityEngine;

public class Level3PlayerBounds : MonoBehaviour
{
    [Header("Shape Jam Bounds")]
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -4.275f;
    [SerializeField] private float maxY = 5.125f;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void SetBounds(float newMinX, float newMaxX, float newMinY, float newMaxY)
    {
        minX = newMinX;
        maxX = newMaxX;
        minY = newMinY;
        maxY = newMaxY;
    }

    private void FixedUpdate()
    {
        ClampInsideBounds();
    }

    private void ClampInsideBounds()
    {
        Vector2 position = rb != null ? rb.position : (Vector2)transform.position;

        float halfWidth = 0f;
        float halfHeight = 0f;

        if (col != null)
        {
            Bounds bounds = col.bounds;
            halfWidth = bounds.extents.x;
            halfHeight = bounds.extents.y;
        }

        float clampedX = Mathf.Clamp(position.x, minX + halfWidth, maxX - halfWidth);
        float clampedY = Mathf.Clamp(position.y, minY + halfHeight, maxY - halfHeight);

        Vector2 clampedPosition = new Vector2(clampedX, clampedY);

        if (rb != null)
        {
            rb.position = clampedPosition;

            Vector2 velocity = rb.linearVelocity;

            if ((Mathf.Approximately(clampedX, minX + halfWidth) && velocity.x < 0f) ||
                (Mathf.Approximately(clampedX, maxX - halfWidth) && velocity.x > 0f))
            {
                velocity.x = 0f;
            }

            if ((Mathf.Approximately(clampedY, minY + halfHeight) && velocity.y < 0f) ||
                (Mathf.Approximately(clampedY, maxY - halfHeight) && velocity.y > 0f))
            {
                velocity.y = 0f;
            }

            rb.linearVelocity = velocity;
        }
        else
        {
            transform.position = clampedPosition;
        }
    }
}