using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = PlayerController.playerPos - rb.position;
        rb.MovePosition(rb.position + dir.normalized * Time.deltaTime * speed);
    }
}
