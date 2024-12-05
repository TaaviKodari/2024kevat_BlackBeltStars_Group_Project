using System;
using System.Collections.Generic;
using GameState;
using Pathfinding;
using Sound;
using UnityEngine;
using UnityEngine.Serialization;
using Attribute = Attributes.Attribute;

public class Enemy : Entity
{
    // List of resource drops that this enemy will drop when it dies
    public List<ResourceDrop> drops;
    public int goldAmount;
    // Flag to ensure resources are dropped only once when the enemy dies
    private bool resourcesDropped;
    
    // when the enemy has attacked last time
    private float lastAttackTime = 0f;
    
    // Serializable class representing a resource drop, including its type and amount
    [Serializable]
    public class ResourceDrop
    {
        public ResourceType type; // Type of resource to drop
        public int amount; // Amount of the resource to drop
    }
    
    // Reference to the EnemyManager that handles this enemy
    public EnemyManager manager;
    private EntityPathfinder pathfinder;
    private SpriteRenderer spriteRenderer;

    private Vector2 prevMoveDirections;

    private void Start()
    {
        pathfinder = GetComponent<EntityPathfinder>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        pathfinder.SetTarget(EnemyManager.instance.player.transform.position);
    }
    
    // check if the enemy can attack
    private bool CanAttack()
    {
        // the enemy can attack, if it has been "attackCooldown" seconds since the last attack
        return Time.time >= lastAttackTime + AttributeHolder.GetValue(Attribute.AttackCooldown);
    }
    
    // Override of the abstract GetMoveDirection method from the Entity class
    // Passes on the direction from the pathfinder
    protected override Vector2 GetMoveDirection()
    {
        return pathfinder.GetDirection();
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
        AudioManager.Instance.PlaySfx("EnemyDie");
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

        FindObjectOfType<GameStateManager>().currentSaveGame.resources.gold += goldAmount;

        // Mark resources as dropped to prevent dropping again
        resourcesDropped = true;
    }

    protected override void OnMove(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > 0.1)
        {
            // Flip the sprite if moving left, otherwise keep it facing right
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!CanAttack()) return;
        // If the collided object is tagged as "Player" and it has been
        // "attackCooldown" seconds since the last attack, attack the player entity
        if (other.gameObject.CompareTag("Player")) 
        {
            // Call the Attack method inherited from the Entity class
            Attack(other.gameObject.GetComponent<Entity>());
            lastAttackTime = Time.time;
        }
        else if (other.gameObject.TryGetComponent<Building>(out var building))
        {
            building.DoDamage(AttackDamage);
            lastAttackTime = Time.time;
        }
    }
}
