using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeeviPlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private MasterInput input;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new MasterInput();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = input.Player.Movement.ReadValue<Vector2>();
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }
}
