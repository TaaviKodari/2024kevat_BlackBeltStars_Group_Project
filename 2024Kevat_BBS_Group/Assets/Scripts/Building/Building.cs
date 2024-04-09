using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class Building : MonoBehaviour
{
    public BuildingData data;
    private BuildingManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<BuildingManager>();
    }
    
    private void OnDestroy()
    {
        manager.RemoveBuilding(this);
    }
    
    // Gets the position in the center of this building. Can be off by 0.5 for buildings with an even size
    private Vector2Int GetPosition()
    {
        return manager.WorldPosToBuildingPos(transform.position);
    }

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
} 
