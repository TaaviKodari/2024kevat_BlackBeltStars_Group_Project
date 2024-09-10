using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    // List of resource drops that this enemy will drop when it dies
    public List<ResourceDrop> drops;
    // Flag to ensure resources are dropped only once when the enemy dies
    private bool resourcesDropped;

    // Serializable class representing a resource drop, including its type and amount
    [Serializable]
    public class ResourceDrop
    {
        public ResourceType type; // Type of resource to drop
        public int amount; // Amount of the resource to drop
    }
    
    // Reference to the EnemyManager that handles this enemy
    public EnemyManager manager;

    // Override of the abstract GetMoveDirection method from the Entity class
    // Returns the direction towards the player, normalized for consistent speed
    protected override Vector2 GetMoveDirection()
    {
        return ((Vector2)EnemyManager.instance.player.transform.position - rb.position).normalized;
    }

    // Override of the Die method from the Entity class
    // Handles the enemy's death by dropping loot, notifying the manager, and playing a sound
    protected override void Die()
    {
        // Drop the loot upon death
        DropLoot();
        // Notify the EnemyManager that this enemy has died
        manager.EnemyDie(this);
        // Call the base class's Die method to handle destruction
        base.Die();
        // Play the "EnemyDie" sound effect
        FindObjectOfType<AudioManager>().PlayFull("EnemyDie");
    }

    // Method to drop loot when the enemy dies
    private void DropLoot()
    {
        // Check if resources have already been dropped to prevent duplication
        if (resourcesDropped) return;
        
        // Iterate through each ResourceDrop in the drops list and spawn them
        foreach (var drop in drops)
        {
            ResourceManager.Instance.SpawnResource(drop.type, drop.amount, transform.position);
        }

        // Mark resources as dropped to prevent dropping again
        resourcesDropped = true;
    }

    // Unity method called when this object collides with another 2D object
    void OnCollisionEnter2D(Collision2D other)
    {
        // If the collided object is tagged as "Player", attack the player entity
        if (other.gameObject.CompareTag("Player")) 
        {
            // Call the Attack method inherited from the Entity class
            Attack(other.gameObject.GetComponent<Entity>());
        }
    }
}
