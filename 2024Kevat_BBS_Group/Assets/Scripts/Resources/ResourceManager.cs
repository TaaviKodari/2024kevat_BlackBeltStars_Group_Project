using System;
using System.Collections.Generic;
using AtomicConsole;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    private readonly Dictionary<ResourceType, int> resources = new();
    public PlayerController player { get; private set; }
    public DroppedResource resourcePrefab;
    
    [AtomicSet(name:"ResourceMultiplier",group:"Resources",description:"Multiplier of the output of AddResources command, can't be zero")] public int resourceMultiplier=1;
    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        // We do this in start because the variant manager might not be ready in awake yet
        foreach (var resource in VariantManager.Instance.ResourceTypes.Values)
        {
            resources[resource] = 0;
        }
    }

    // Spawns a resource in the world
    public void SpawnResource(ResourceType type, int amount, Vector2 position)
    {
        // Instantiate the resourcePrefab at the specified position as a child of the manager
        var droppedResource = Instantiate(resourcePrefab, position, Quaternion.identity, transform);
        // Set the type and amount of the resource drop
        droppedResource.type = type;
        droppedResource.amount = amount;
        // Initialize the resource (e.g., setting up initial velocity)
        droppedResource.Init();
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
    [AtomicCommand(name: "AddResources",group:"Resources",description:"Adds resources to the player (default: 10, increase multiplier with ResourceMultiplier)")]
    public void IncrementResources()
    {
        if(resourceMultiplier==0)
        {resourceMultiplier=1; AtomicConsole.Engine.AtomicConsoleEngine.print("Resource multiplier was set to 1 because it was "+resourceMultiplier);}
        foreach (var resource in VariantManager.Instance.ResourceTypes.Values)
        {
            AddResource(resource, 10*resourceMultiplier);
        }
    }
}
