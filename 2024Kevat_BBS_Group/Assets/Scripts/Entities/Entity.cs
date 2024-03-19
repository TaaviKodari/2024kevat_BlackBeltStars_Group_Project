using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float health = 2f;
    [SerializeField] protected float damage = 1f;

    [SerializeField] protected float speed = 2f;

    protected Rigidbody2D rb;

    // Virtual means the method can be overridden in a child class.
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
}
