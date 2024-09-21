using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public float healWait;
    public float healAmount;
    private float lastHealed;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out var player) && Time.time - lastHealed >= healWait)
        {
            player.Heal(healAmount);
            lastHealed = Time.time;
        }
    }
}
