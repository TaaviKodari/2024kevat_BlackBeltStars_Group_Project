using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IBuildingBlocker
{
    [SerializeField] private float maxHealth = 2f;
    [SerializeField] protected float damage = 1f;
    [SerializeField] private float knockback = 2f;
    [SerializeField] protected float speed = 2f;

    private float health;
    private float lastHitTime;
    private float lastHitDamage;

    protected Rigidbody2D rb;

    public HealthBar healthBar;

    // Virtual means the method can be overridden in a child class.
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;

        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(maxHealth);
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected abstract Vector2 GetMoveDirection();

    private void Move()
    {
        var dir = GetMoveDirection();
        rb.velocity += dir * (Time.fixedDeltaTime * speed * 50);
        OnMove(dir);
    }

    protected virtual void OnMove(Vector2 direction)
    {
    }

    public void Damage(float amount)
    {
        // Minecraft style i-frames: You can take damage during them if the new damage is larger
        if (lastHitTime > Time.time - 0.5)
        {
            amount -= lastHitDamage;
        }
        else
        {
            lastHitDamage = 0;
        }

        if (amount <= 0) return;
        health -= amount;
        FindObjectOfType<AudioManager>().Play("EnemyDamaged");
        lastHitTime = Time.time;
        lastHitDamage = amount;

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        healthBar.setHealth(health);
        
        
    }

    public void Heal(float amount)
    {
        health = Mathf.Max(health + amount, maxHealth);
        healthBar.setHealth(health);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void Attack(Entity other)
    {
        other.Damage(damage);
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity += (Vector2)(other.transform.position - transform.position).normalized * knockback;
        Debug.Log(rb.velocity);
    }
}
