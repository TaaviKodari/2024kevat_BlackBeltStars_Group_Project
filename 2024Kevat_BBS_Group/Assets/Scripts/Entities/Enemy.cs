using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Rigidbody2D player;

    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = player.position - rb.position;
        rb.MovePosition(rb.position + dir.normalized * Time.deltaTime * speed);
    }
}
