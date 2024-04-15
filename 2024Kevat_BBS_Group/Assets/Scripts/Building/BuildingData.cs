using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    public Vector2Int size = Vector2Int.one;
    public GameObject prefab;
    public List<ResourceCost> costs;

    [Serializable]
    public class ResourceCost
    {
        public ResourceManager.ResourceType type;
        public int amount;
    }
}
