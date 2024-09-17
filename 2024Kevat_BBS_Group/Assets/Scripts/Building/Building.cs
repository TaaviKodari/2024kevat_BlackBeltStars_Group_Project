using System;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData data;
    private BuildingManager manager;
    private float durability;

    private void Awake()
    {
        manager = FindObjectOfType<BuildingManager>();
    }

    private void Start()
    {
        durability = data.durability;
    }
    
    private void OnDestroy()
    {
        manager.RemoveBuilding(this);
    }
    
    // Gets the position in the center of this building. Can be off by 0.5 for buildings with an even size
    private Vector2Int GetPosition()
    {
        return manager.WorldPosToBuildingPos((Vector2) transform.position - data.offset);
    }

    // Returns positions used by this building so no other buildings can build that overlap this building
    public HashSet<Vector2Int> GetUsedPositions()
    {
        var positions = new HashSet<Vector2Int>();
        var basePos = GetPosition() - data.size / 2;
        for (var x = 0; x < data.size.x; x++)
        {
            for (var y = 0; y < data.size.y; y++)
            {
                positions.Add(new Vector2Int(x + basePos.x, y + basePos.y));
            }
        }
        return positions;
    }

    public void DoDamage(float amount)
    {
        durability = Mathf.Max(durability - amount, 0); // Prevent going below zero
        if (durability == 0) Destroy(gameObject);
    }
}
