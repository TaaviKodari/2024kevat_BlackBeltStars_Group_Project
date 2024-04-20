using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public List<ResourceDrop> drops;

    [Serializable]
    public class ResourceDrop
    {
        public ResourceManager.ResourceType type;
        public int amount;
    }

    public ResourceData resourcePrefab;

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
        foreach (ResourceDrop drop in drops)
        {
            ResourceData rd = Instantiate<ResourceData>(resourcePrefab, transform.position, resourcePrefab.transform.rotation);
            rd.type = drop.type;
            rd.amount = drop.amount;
            rd.Init();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) Attack(other.gameObject.GetComponent<Entity>());
    }
}
