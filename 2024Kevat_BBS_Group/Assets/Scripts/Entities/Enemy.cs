using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public List<ResourceDrop> drops;
    private bool resourcesDropped;

    [Serializable]
    public class ResourceDrop
    {
        public ResourceManager.ResourceType type;
        public int amount;
    }

    public Resource resourcePrefab;
    public EnemyManager manager;

    protected override Vector2 GetMoveDirection()
    {
        return ((Vector2)EnemyManager.instance.player.transform.position - rb.position).normalized;
    }

    protected override void Die()
    {
        DropLoot();
        manager.EnemyDie(this);
        base.Die();
        FindObjectOfType<AudioManager>().Play("EnemyDie");
    }

    private void DropLoot()
    {
        // Sometimes an enemy can die again before being removed from the world, this check prevents dropping resources twice.
        if (resourcesDropped) return;
        
        foreach (ResourceDrop drop in drops)
        {
            var rd = Instantiate(resourcePrefab, transform.position, resourcePrefab.transform.rotation);
            rd.type = drop.type;
            rd.amount = drop.amount;
            rd.Init();
        }

        resourcesDropped = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) Attack(other.gameObject.GetComponent<Entity>());
    }
}
