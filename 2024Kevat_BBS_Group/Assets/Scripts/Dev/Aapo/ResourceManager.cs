using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour;
public class ResourceManager2
{
    // resurssit ja määrät
    private Dictionary<ResourceType, int> resources;

    // erityypit
    public enum ResourceType
    {
        Wood,
        Stone,
        Iron
        
    }

   
    public ResourceManager2()
    {
        resources = new Dictionary<ResourceType, int>();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            resources.Add(type, 0);
        }
    }
    //lisäys tietty
    public void AddResource(ResourceType type, int amount)
    {
        if (!resources.TryGetValue(type, out _))
        {
            throw new KeyNotFoundException($"Unknown resource type: {type}");
        }

        resources[type] += amount;
    }

    // poisto tiettymäärä
    public void RemoveResource(ResourceType type, int amount)
    {
        
        if (resources.ContainsKey(type))
        {
            if (resources[type] >= amount)
            {
                resources[type] -= amount;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Not enough resources of type " + type);
            }
        }
        else
        {
            throw new KeyNotFoundException("Unknown resource type: " + type);
        }
    }

    
    // paljonko nyt
    
    public int GetResourceAmount(ResourceType type)
    {
        
        if (resources.ContainsKey(type))
        {
            return resources[type];
        }
        else
        {
            throw new KeyNotFoundException("Unknown resource type: " + type);
        }
    }
}
