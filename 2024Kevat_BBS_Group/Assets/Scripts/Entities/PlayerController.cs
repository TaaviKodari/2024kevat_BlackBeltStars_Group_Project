using UnityEngine;

public class PlayerController : Entity
{
    public MasterInput input { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        input = new MasterInput();
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

    protected override Vector2 GetMoveDirection()
    {
        return input.Player.Movement.ReadValue<Vector2>();
    }

    protected override void Die()
    {
        // TODO: Game over screen
    }
}
