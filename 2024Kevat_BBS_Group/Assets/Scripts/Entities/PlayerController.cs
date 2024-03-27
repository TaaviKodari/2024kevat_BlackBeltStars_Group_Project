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

    protected override Vector2 GetMoveDirection()
    {
        return input.Player.Movement.ReadValue<Vector2>();
    }
    
}
