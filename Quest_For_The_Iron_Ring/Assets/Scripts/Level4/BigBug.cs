using UnityEngine;

public class BigBug : Bug
{
    [Header("Big Bug Movement")]
    [SerializeField] private float chaseSpeed = 1f;
    [SerializeField] private float chaseTurnSpeed = 2f;

    private Transform player;

    protected override void Start()
    {
        base.Start();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    protected override Vector2 GetMovementDirection()
    {
        // If player not found, behave like normal bug
        if (player == null)
            return base.GetMovementDirection();

        // Direction toward player
        Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;

        // Current movement direction
        Vector2 currentDirection = direction;

        // Smooth turning toward player
        Vector2 newDirection = Vector2.Lerp(currentDirection, toPlayer, chaseTurnSpeed * Time.deltaTime).normalized;

        // Update base direction so animation stays correct
        direction = newDirection;

        return direction;
    }

    protected override float GetMoveSpeed()
    {
        return chaseSpeed;
    }
}