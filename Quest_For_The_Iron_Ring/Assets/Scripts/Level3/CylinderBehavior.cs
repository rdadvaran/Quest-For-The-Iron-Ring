using UnityEngine;

public class CylinderBehavior : ProjectileBehavior
{
    private Vector2 moveDirection = Vector2.right;

    // Called right after spawn
    public void SetDirection(Vector2 newDirection)
    {
        moveDirection = newDirection.normalized;
    }

    protected override void UpdatePosition()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    protected override void Collision(PlayerController3 player)
    {
        player.UpdateHealth(-1);
        Despawn();
    }
}