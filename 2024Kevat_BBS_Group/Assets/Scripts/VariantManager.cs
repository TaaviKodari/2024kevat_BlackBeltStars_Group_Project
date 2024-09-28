using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Tooltip("Contains lists of all instances of some objects")]
[CreateAssetMenu(fileName = "VariantManager", menuName = "ScriptableObjects/VariantManager", order = 2)]
public class VariantManager : ScriptableObject
{
    public static VariantManager Instance { get; private set; }

    [SerializeField]
    private List<ResourceType> resourceTypes;
    [NonSerialized]
    public readonly Dictionary<string, ResourceType> ResourceTypes = new();
        
    [SerializeField]
    private List<TerrainObstacle> terrainObstacles;
    [NonSerialized]
    public readonly Dictionary<string, TerrainObstacle> TerrainObstacles = new();

    public void Init()
    {
        if (Instance != null) return; 
        Instance = this;
        foreach (var resourceType in resourceTypes)
        {
            ResourceTypes[resourceType.id] = resourceType;
        }
        foreach (var resourceType in terrainObstacles)
        {
            TerrainObstacles[resourceType.id] = resourceType;
        }
    }
}