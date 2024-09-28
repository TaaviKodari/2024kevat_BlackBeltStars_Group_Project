using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    public bool blocksProjectiles = true;
    [SerializeField]
    private Vector2Int size = Vector2Int.one;
    [SerializeField]
    private Vector2Int verticalSize = Vector2Int.zero;
    public Vector2 offset = Vector2.zero;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject verticalPrefab; // if not null then building rotates
    public List<ResourceCost> costs;
    public int durability;

    public bool HasSplitPrefab()
    {
        return verticalPrefab != null;
    }

    public GameObject GetPrefab(bool vertical)
    {
        if (HasSplitPrefab())
        {
            return vertical ? verticalPrefab : prefab;
        }
        return prefab;
    }

    public Vector2Int GetSize(bool vertical)
    {
        if (HasSplitPrefab())
        {
            return vertical ? verticalSize : size;
        }
        return size;
    }

    [Serializable]
    public class ResourceCost
    {
        public ResourceType type;
        public int amount;
    }
}
