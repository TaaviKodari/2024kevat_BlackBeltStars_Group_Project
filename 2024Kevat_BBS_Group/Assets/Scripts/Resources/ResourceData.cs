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

    public void SetTexture()
    {
        GetComponent<SpriteRenderer>().sprite = resourceTextures.Find(t => t.type == type).sprite;
    }
}
