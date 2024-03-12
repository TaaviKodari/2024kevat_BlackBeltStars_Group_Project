using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    public MasterInput input { get; private set; }
    public static Vector2 playerPos { get; private set; } = Vector2.zero;

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

    private void playerMove()
    {
        Vector2 dir = input.Player.Movement.ReadValue<Vector2>();
        rb.velocity += dir * speed;
        playerPos = rb.position;
    }

    void FixedUpdate()
    {
        playerMove();
    }
}
