using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float health = 2f;
    [SerializeField] protected float damage = 1f;

    [SerializeField] protected float speed = 2f;

    protected Rigidbody2D rb;

    // Virtual means the method can be overridden in a child class.
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
