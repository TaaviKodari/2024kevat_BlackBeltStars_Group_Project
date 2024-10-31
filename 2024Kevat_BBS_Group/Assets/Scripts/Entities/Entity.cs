using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IBuildingBlocker
{
    // Time during which the entity is invulnerable after taking damage (currently set to 0)
    private const float InvulnerabilityTime = 0;
        
    // Maximum health of the entity
    [SerializeField] private float maxHealth = 2f;
    // Amount of damage the entity can inflict
    [SerializeField] protected float damage = 1f;
    // Force applied to another entity when this one attacks (knockback effect)
    [SerializeField] private float knockback = 2f;
    // Movement speed of the entity
    [SerializeField] protected float speed = 2f;

    // Current health of the entity
    private float health;
    // Time when the entity was last hit
    private float lastHitTime;
    // Amount of damage taken in the last hit
    private float lastHitDamage;

    // Reference to the Rigidbody2D component used for physics-based movement
    protected Rigidbody2D rb;

    // Reference to the HealthBar UI element that shows the entity's health
    [SerializeField]
    private HealthBar healthBar;

    // Awake is called when the script instance is being loaded
    protected virtual void Awake()
    {
        // Get the Rigidbody2D component attached to the entity
        rb = GetComponent<Rigidbody2D>();
        // Initialize health to the maximum health value
        health = maxHealth;

        // Set up the health bar with the max health and current health
        healthBar.SetHealth(maxHealth, maxHealth);
    }

    // FixedUpdate is called at a fixed interval and is used here for physics-related updates
    private void FixedUpdate()
    {
        // Call the Move method to handle movement
        Move();
    }

    // Abstract method that must be implemented by derived classes to determine movement direction
    protected abstract Vector2 GetMoveDirection();

    // Handles movement by updating the Rigidbody2D's velocity based on the movement direction
    private void Move()
    {
        // Get the movement direction from the derived class
        var dir = GetMoveDirection();
        // Apply movement based on direction, speed, and a scaling factor
        rb.velocity += dir * (Time.fixedDeltaTime * speed * 50);
        // Optional method to add additional behavior when moving
        OnMove(dir);
    }

    // Virtual method that can be overridden in derived classes to add custom behavior during movement
    protected virtual void OnMove(Vector2 direction)
    {
    }

    // Method to set health based on a multiplier (for spawning "stronger enemies")
    public void SetHealth(float healthMultiplier)
    {
        maxHealth *= healthMultiplier;
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);
    }

    // Method to set speed based on a multiplier (for spawning "stronger enemies")
    public void SetSpeed(float speedMultiplier)
    {
        speed *= speedMultiplier;
    }
    
    // Method to apply damage to the entity
    public void Damage(float amount)
    {
        // Check if the entity is within its invulnerability time frame
        if (lastHitTime > Time.time - InvulnerabilityTime)
        {
            // Reduce the damage by the amount of damage last taken during invulnerability
            amount -= lastHitDamage;
        }
        else
        {
            lastHitDamage = 0;
        }

        // If the adjusted damage is less than or equal to 0, exit the method
        if (amount <= 0) return;

        // Subtract the damage from the entity's health
        health -= amount;
        // Play the "EnemyDamaged" sound effect using the AudioManager
        AudioManager.Instance.PlayOver("EnemyDamaged");
        // Record the time and amount of damage taken
        lastHitTime = Time.time;
        lastHitDamage = amount;

        // If health falls to zero or below, the entity dies
        if (health <= 0)
        {
            health = 0;
            Die();
        }

        // Update the health bar to reflect the new health value
        healthBar.SetHealth(health, maxHealth);
    }

    // Method to heal the entity by a specified amount
    public void Heal(float amount)
    {
        // Increase health by the amount healed, but don't exceed max health
        health = Mathf.Min(health + amount, maxHealth);
        // Update the health bar to reflect the new health value
        healthBar.SetHealth(health, maxHealth);
    }

    // Virtual method that can be overridden to define custom behavior when the entity dies
    protected virtual void Die()
    {
        // Destroy the entity's GameObject when it dies
        Destroy(gameObject);
    }

    // Method for the entity to attack another entity
    protected void Attack(Entity other)
    {
        // Inflict damage on the other entity
        other.Damage(damage);

        // Apply knockback to the other entity if it has a Rigidbody2D component
        if (other.TryGetComponent<Rigidbody2D>(out var rb))
        {
            // Calculate and apply knockback force based on the direction from this entity to the other
            rb.velocity += (Vector2)(other.transform.position - transform.position).normalized * knockback;
        }
    }
}
