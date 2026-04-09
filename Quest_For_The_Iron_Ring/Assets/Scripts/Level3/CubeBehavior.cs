using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private Vector2 moveDirection = new Vector2(1f, -1f);

    [Header("Plane Bounds")]
    [SerializeField] private float minX = -5.6f;
    [SerializeField] private float maxX = 5.6f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    [Header("Logo Spawn")]
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private float iconSpawnInterval = 8f;

    private float iconTimer = 0f;

    private void Update()
    {
        UpdatePosition();
        Drop();
    }

    public void UpdatePosition()
    {
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime);

        Vector3 pos = transform.position;

        if (pos.x <= minX || pos.x >= maxX)
        {
            moveDirection.x *= -1f;
        }

        if (pos.y <= minY || pos.y >= maxY)
        {
            moveDirection.y *= -1f;
        }

        // Keep cube inside bounds
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    // Spawns a logo from the cube position
    public void Drop()
    {
        if (iconPrefab == null) return;

        iconTimer += Time.deltaTime;

        if (iconTimer >= iconSpawnInterval)
        {
            iconTimer = 0f;
            Instantiate(iconPrefab, transform.position, Quaternion.identity);
        }
    }

    public void Despawn()
    {
        // Cube does not despawn in this level
    }

    public void Collision(PlayerController player)
    {
        player.UpdateHealth(-1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Collision(player);
        }
    }
}