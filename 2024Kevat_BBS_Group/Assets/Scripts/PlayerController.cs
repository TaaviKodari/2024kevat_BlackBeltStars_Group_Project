using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    private Rigidbody2D rb;
    private MasterInput input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        rb.velocity += dir * speed; //This works. Can be changed to movepos.
    }

    void FixedUpdate()
    {
        playerMove();
    }
}
