using UnityEngine;

public class Enemy : Entity
{
    public Rigidbody2D player;

    protected override Vector2 GetMoveDirection()
    {
        return (player.position - rb.position).normalized;
    }
}
