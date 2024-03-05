using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillePlayerController : MonoBehaviour
{
    
    private float movementSpeed = 5f;
    private Rigidbody2D rb;
    private MasterInput controls;

    //public GameManager gameManager;
    public float health;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new MasterInput();
    }
    
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private bool wasLMBPressed = false; // onko left clickiä painettu tässä framessa?


    private void FixedUpdate()
    {
        Move();
        float attackValue = controls.Player.Attack.ReadValue<float>();
    

        if (attackValue > 0.5f && !wasLMBPressed)
        {
            Attack();
            wasLMBPressed = true;
        }
        else if (attackValue <= 0.5f)
        {
            wasLMBPressed = false;
        }
    }

    private void Move()
    {
        var movementDirection = controls.Player.Movement.ReadValue<Vector2>();
        rb.MovePosition(rb.position + movementDirection * (movementSpeed * Time.fixedDeltaTime));
    }

    private void Attack()
    {
        Debug.Log("Attacked!");
    }
}