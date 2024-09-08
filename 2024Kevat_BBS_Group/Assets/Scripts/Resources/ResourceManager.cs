using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    private readonly Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    public List<ResourceType> allResourceTypes = new List<ResourceType>();
    public PlayerController player { get; private set; }

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<PlayerController>();
        foreach (var resource in allResourceTypes)
        {
            resources[resource] = 0;
        }
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
        if (!resources.TryGetValue(type, out var available))
        {
            throw new KeyNotFoundException($"Unknown resource type: {type}");
        }
        if (available < amount)
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
        foreach (var resource in allResourceTypes)
        {
            AddResource(resource, 10);
        }
    }
}
