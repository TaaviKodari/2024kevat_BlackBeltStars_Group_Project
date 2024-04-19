using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData : MonoBehaviour
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
        GetComponent<Rigidbody2D>().velocity = new Vector2(
            UnityEngine.Random.Range(0, randomDiff), UnityEngine.Random.Range(0, randomDiff));
    }
}
