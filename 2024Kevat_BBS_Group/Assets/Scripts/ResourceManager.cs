using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    private readonly Dictionary<ResourceType, int> resources;

    public enum ResourceType
    {
        Wood,
        Stone,
        Iron
    }
    
    public ResourceManager()
    {
        resources = new Dictionary<ResourceType, int>();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            resources.Add(type, 0);
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    
    public void AddResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
        {
            throw new KeyNotFoundException($"Unknown resource type: {type}");
        }

        resources[type] += amount;
    }

    public void RemoveResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
        {
            throw new KeyNotFoundException($"Unknown resource type: {type}");
        }
        if (resources[type] < amount)
        {
            throw new ArgumentOutOfRangeException($"Not enough resources of type {type}");
        }

        resources[type] -= amount;
    }
    
    public int GetResourceAmount(ResourceType type)
    {
        if (resources.TryGetValue(type, out var amount))
        {
            return amount;
        }

        throw new KeyNotFoundException($"Unknown resource type: {type}");
    }

    // Temporary method for use in UI. Button callbacks can't deal with enums
    public void IncrementResources()
    {
        AddResource(ResourceType.Wood, 13);
        AddResource(ResourceType.Stone, 7);
        AddResource(ResourceType.Iron, 17);
    }
}
