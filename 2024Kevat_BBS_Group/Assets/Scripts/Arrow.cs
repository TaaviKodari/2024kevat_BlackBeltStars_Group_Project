using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float Damage { private get; set; }
    [SerializeField]
    private float maxTime = 5;

    private float timeTraveled;
    private bool hasHit;
    public GameObject Owner { private get; set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (other.gameObject == Owner) return;
        if (!other.TryGetComponent<Entity>(out var entity)) return;
        entity.Damage(Damage);
        hasHit = true;
        Destroy(gameObject);
    }

    private void Update()
    {
        timeTraveled += Time.deltaTime;
        if (timeTraveled >= maxTime)
        {
            Destroy(gameObject);
        }
    }
}
