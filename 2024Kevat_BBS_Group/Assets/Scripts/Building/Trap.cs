using UnityEngine;

public class Trap : MonoBehaviour
{
    public float damage;

    [SerializeField]
    private float cooldownAmount;
    private float cooldown;

    void Awake()
    {
        cooldown = cooldownAmount;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy) && cooldown <= 0)
        {
            enemy.Damage(damage);
            cooldown = cooldownAmount;
        }
    }

    void Update()
    {
        if (cooldown > 0) cooldown -= Time.deltaTime;
    }
}
