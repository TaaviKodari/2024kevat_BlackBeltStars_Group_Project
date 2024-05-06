using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float maxTime = 5;

    private float timeTraveled;
    public GameObject Owner { private get; set; }

    private void OnEnable()
    {
        CheckInBuildings();
    }

    private void CheckInBuildings()
    {
        var colliders = new List<Collider2D>();
        GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), colliders);
        if (colliders.Any(it => it.TryGetComponent<Building>(out var building) && building.data.blocksProjectiles))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Owner) return;
        if (other.TryGetComponent<Entity>(out var entity))
        {
            entity.Damage(damage);
            Destroy(gameObject);
        } 
        else if (other.TryGetComponent<Building>(out var building) && building.data.blocksProjectiles)
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
