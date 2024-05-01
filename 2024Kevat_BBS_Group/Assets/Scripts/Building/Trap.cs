using UnityEngine;

public class Trap : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.Damage(damage);
        }
    }
}
