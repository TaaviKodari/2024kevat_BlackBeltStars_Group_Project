using System.Collections;
using System.Collections.Generic;
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

    private void PlayerMove()
    {
        Vector2 dir = input.Player.Movement.ReadValue<Vector2>();
        rb.MovePosition(rb.position + dir * Time.deltaTime * speed);
    }

    void FixedUpdate()
    {
        PlayerMove();
    }
}
