using UnityEngine;

public class Enemy : Entity
{
    protected override Vector2 GetMoveDirection()
    {
        return ((Vector2)EnemyManager.instance.player.transform.position - rb.position).normalized;
    }

    protected override void Die()
    {
        DropLoot();
        base.Die();
    }

    private void DropLoot()
    {
        // TODO: Drop loot
    }
}
