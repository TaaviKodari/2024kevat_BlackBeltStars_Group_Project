using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    public bool blocksProjectiles = true;
    public Vector2Int size = Vector2Int.one;
    public Vector2 offset = Vector2.zero;
    public GameObject prefab;
    public List<ResourceCost> costs;

    [Serializable]
    public class ResourceCost
    {
        public ResourceType type;
        public int amount;
    }
}
