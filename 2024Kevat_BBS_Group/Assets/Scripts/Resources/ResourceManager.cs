using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    private readonly Dictionary<ResourceType, int> resources = new();
    public PlayerController player { get; private set; }
    public DroppedResource resourcePrefab;

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


    // Temp method for hotkey for the function below

    private float addResourcesDelay = 0.1f;
    private float resourceAddTimer = 0f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            resourceAddTimer += Time.deltaTime;

            if (resourceAddTimer >= addResourcesDelay)
            {
                IncrementResources();
                resourceAddTimer = 0f;
            }
        }
        else
        {
            resourceAddTimer = 0f;
        }
    }

    // Temporary method for use in UI. Button callbacks can't deal with enums
    public void IncrementResources()
    {
        foreach (var resource in VariantManager.Instance.ResourceTypes.Values)
        {
            AddResource(resource, 10);
        }
    }
}
