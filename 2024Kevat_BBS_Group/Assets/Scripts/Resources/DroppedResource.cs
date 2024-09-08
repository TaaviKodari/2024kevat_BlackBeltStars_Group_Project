using UnityEngine;
using Random = UnityEngine.Random;

public class DroppedResource : MonoBehaviour
{
    public ResourceType type;
    public int amount;
    public float randomDiff;
    private float cooldown;

    public void Init()
    {
        GetComponent<SpriteRenderer>().sprite = type.sprite;
        GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * randomDiff;
        cooldown = 0.5f;
    }
    
    // We update resources in FixedUpdate because we directly interact with physics
    private void FixedUpdate()
    {
        // Cooldown to show what dropped before the player picks it up
        cooldown -= Time.fixedDeltaTime;
        if (cooldown > 0) return;
        
        // Calculate the distance from the player to the resource
        // If it's over 2 (2^2 = 4) then we don't move
        var distance = (Vector2)(ResourceManager.Instance.player.transform.position - transform.position);
        if (distance.sqrMagnitude > 4) return;
        
        // Add velocity to drop
        GetComponent<Rigidbody2D>().velocity += distance * (1 / (Time.fixedDeltaTime * 8));

        // If were really close to the player, commence with pickup
        if (distance.sqrMagnitude < 0.1)
        {
            ResourceManager.Instance.AddResource(type, amount);
            FindObjectOfType<AudioManager>().PlayOver("ItemPickup");
            Destroy(gameObject);
        }
    }
}
