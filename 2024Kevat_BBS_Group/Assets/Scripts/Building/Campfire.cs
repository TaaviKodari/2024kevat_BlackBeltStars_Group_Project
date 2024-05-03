using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public float healWait;
    public float healAmount;
    private float lastHealed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
        foreach (var collider in colliders)
        {
            if (Time.time - lastHealed >= healWait && collider.TryGetComponent<PlayerController>(out var player))
            {
                player.Heal(healAmount);
                lastHealed = Time.time;
            }
        }
    }
}
