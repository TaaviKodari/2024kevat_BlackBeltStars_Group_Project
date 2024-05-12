using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Resource : MonoBehaviour
{
    public List<ResourceTexture> resourceTextures;
    [Serializable]
    public class ResourceTexture
    {
        public ResourceManager.ResourceType type;
        public Sprite sprite;
    }
    public ResourceManager.ResourceType type;
    public int amount;
    public float randomDiff;

    public void Init()
    {
        GetComponent<SpriteRenderer>().sprite = resourceTextures.Find(t => t.type == type).sprite;
        GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * randomDiff;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ResourceManager.Instance.AddResource(type, amount);
            FindObjectOfType<AudioManager>().PlayOver("ItemPickup");
            Destroy(gameObject);
        }
    }
}
