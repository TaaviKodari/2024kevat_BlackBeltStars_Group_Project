using UnityEngine;

public class Trap : MonoBehaviour
{
    public float damage;

    [SerializeField]
    private float cooldownAmount;
    private float cooldown;

    [SerializeField]
    private SpriteRenderer cooldownOverlay;
    [SerializeField]
    private Gradient cooldownGradient;

    private Building building;

    void Awake()
    {
        cooldown = cooldownAmount;
        building = GetComponent<Building>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy) && cooldown <= 0)
        {
            enemy.Damage(damage);
            cooldown = cooldownAmount;
            building.DoDamage(1);
        }
    }

    void Update()
    {
        if (cooldown > 0) cooldown -= Time.deltaTime;
        cooldownOverlay.color = cooldownGradient.Evaluate(cooldown / cooldownAmount);
    }
}
