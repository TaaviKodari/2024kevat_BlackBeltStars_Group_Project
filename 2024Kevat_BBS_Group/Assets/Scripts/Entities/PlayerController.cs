using UnityEngine;

public class PlayerController : Entity
{
    public MasterInput input { get; private set; }
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        input = new MasterInput();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Resource"))
        {
            ResourceData data = collision.gameObject.GetComponent<ResourceData>();
            ResourceManager.Instance.AddResource(data.type, data.amount);
            Destroy(collision.gameObject);
        }
    }

    protected override void OnMove(Vector2 direction)
    {
        var moving = direction.sqrMagnitude > 0.1;
        animator.SetBool("Running", moving);
        if (moving)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    protected override Vector2 GetMoveDirection()
    {
        return input.Player.Movement.ReadValue<Vector2>();
    }

    protected override void Die()
    {
        // TODO: Game over screen
    }
}
