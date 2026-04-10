using UnityEngine;

public class SphereBehavior : ProjectileBehavior
{
    private Vector2 moveDirection = Vector2.down;

    public void SetDirection(Vector2 newDirection)
    {
        moveDirection = newDirection.normalized;
    }

    protected override void UpdatePosition()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    protected override void Collision(PlayerController player)
    {
        player.UpdateHealth(-1);
        Despawn();
    }
}