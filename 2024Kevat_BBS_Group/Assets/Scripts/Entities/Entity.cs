using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float maxHealth = 2f;
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected float speed = 2f;

    private float health;
    private float lastHitTime;
    private float lastHitDamage;
    
    protected Rigidbody2D rb;

    // Virtual means the method can be overridden in a child class.
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
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
        lastHitTime = Time.time;
        lastHitDamage = amount;
        
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void Heal(float amount)
    {
        health = Mathf.Max(health + amount, maxHealth);
    }
    
    protected virtual void Die()
    {
        Destroy(this);
    } 
}
