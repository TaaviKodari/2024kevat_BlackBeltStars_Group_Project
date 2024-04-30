using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float maxTime = 5;

    private float timeTraveled;
    public GameObject Owner { private get; set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Owner) return;
        if (other.TryGetComponent<Entity>(out var entity))
        {
            entity.Damage(damage);
            Destroy(gameObject);
        } 
        else if (other.gameObject.layer == LayerMask.NameToLayer("Buildings"))
        {
            Destroy(gameObject);
        }
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
