using System;
using System.Collections.Generic;
using System.Linq;
using GameState;
using JetBrains.Annotations;
using Sound;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    // Layer mask used to check if a building can be placed at a certain position using collisions
    // Buildings are excluded as they can be accurately checked using the position data
    // Initialized in Awake
    private int buildingPlacementLayerMask;
    
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Vector2 tileSize = new(1, 1);
    
    private InGameManager gameManager;
    
    private readonly HashSet<Vector2Int> usedPositions = new();
    private readonly Dictionary<Vector2Int, Building> positionToBuilding = new();
    private readonly HashSet<Building> buildings = new();

    private void Awake()
    {
        gameManager = FindObjectOfType<InGameManager>();
        // Have to init this here, unity doesn't allow for layer masks to be created before awake
        buildingPlacementLayerMask = ~LayerMask.GetMask("Buildings", "Ignore Raycast");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.5f);
        foreach (var usedPosition in usedPositions)
        {
            Gizmos.DrawCube(BuildingPosToWorldPos(usedPosition) + new Vector2(0.5f, 0.5f), Vector3.one);
        }
    }
    
    public bool HasBuildingAt(Vector2Int pos)
    {
        return usedPositions.Contains(pos);
    }

    [CanBeNull]
    public Building GetBuildingAt(Vector2Int pos)
    {
        positionToBuilding.TryGetValue(pos, out var building);
        return building;
    }
    
    public Vector2Int WorldPosToBuildingPos(Vector2 worldPos)
    {
        var x = Mathf.FloorToInt(worldPos.x / tileSize.x);
        var y = Mathf.FloorToInt(worldPos.y / tileSize.y);
        return new Vector2Int(x, y);
    }
    
    public Vector2 BuildingPosToWorldPos(Vector2Int pos)
    {
        var x = pos.x * tileSize.x;
        var y = pos.y * tileSize.y;
        return new Vector2(x, y);
    }

    // Builds given building if possible (player has enough resources and building does not overlap with other buildings)
    public void TryAddBuilding(Building building)
    {
        var buildingPositions = building.GetUsedPositions();
        if (!CanPlace(building))
        {
            Destroy(building.gameObject);
            if(gameManager.Input.Building.Place.triggered)
            {
                AudioManager.Instance.PlaySfx("CantPlace");
            }
            return;
        }
        usedPositions.UnionWith(buildingPositions);
        buildings.Add(building);
        buildingPositions.ToList().ForEach(p => positionToBuilding.Add(p, building));
        AudioManager.Instance.PlaySfx("PlaceSound");
        foreach (var cost in building.data.costs)
        {
            ResourceManager.Instance.RemoveResource(cost.type, cost.amount);
        }
    }

    // Checks is it possible to build given building (player has enough resources and building does not overlap with other buildings)
    public bool CanPlace(Building building)
    {
        var costs = building.data.costs;
        if (costs.Any(cost => ResourceManager.Instance.GetResourceAmount(cost.type) < cost.amount))
        {
            return false;
        }
        
        var buildingPositions = building.GetUsedPositions();
        if (usedPositions.Overlaps(buildingPositions)) return false;
        var halfVec = new Vector2(0.5f, 0.5f);
        // Get all overlapping colliders and make sure none have a IBuildingBlocker component
        return buildingPositions.SelectMany(pos => Physics2D.OverlapBoxAll(pos + halfVec, Vector2.one * 0.95f, 0, buildingPlacementLayerMask))
            .All(collider2d => collider2d.GetComponent<IBuildingBlocker>() == null);
    }

    // Returns 75% of the price of a building to the player
    public static void ReturnBuildingResources(Building building)
    {
        foreach (var cost in building.data.costs)
        {
            ResourceManager.Instance.AddResource(cost.type, Mathf.FloorToInt(cost.amount * 0.75f));
        }
    }

    public void RemoveBuilding(Building building)
    {
        if (!buildings.Contains(building)) return;
        var buildingPositions = building.GetUsedPositions();
        usedPositions.ExceptWith(buildingPositions);
        buildings.Remove(building);
        buildingPositions.ToList().ForEach(p => positionToBuilding.Remove(p));
    }

    public bool IsVertical(Vector3 pos)
    {
        var distance = pos - player.transform.position;
        var xDiff = Mathf.Abs(distance.x);
        var yDiff = Mathf.Abs(distance.y);
        return xDiff > yDiff;
    }
}