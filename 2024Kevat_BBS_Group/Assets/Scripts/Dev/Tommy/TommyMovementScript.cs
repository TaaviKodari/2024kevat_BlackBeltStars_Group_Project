using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommyMovementScript : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    private Rigidbody2D rb;
    private MasterInput input;

    // Start is called before the first frame update
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
        rb.velocity += dir * speed;
    }

    void FixedUpdate()
    {
        playerMove();
    }
}
