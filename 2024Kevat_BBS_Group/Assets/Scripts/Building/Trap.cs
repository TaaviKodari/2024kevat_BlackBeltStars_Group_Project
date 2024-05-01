using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Building
{
    public float damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy)) enemy.Damage(damage);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
