using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int size = Vector2Int.one;
    private BuildingManager manager;

    private void OnDestroy()
    {
        if (manager != null)
        {
            manager.RemoveBuilding(this);
        }
    }
    
    private Vector2Int GetPosition()
    {
        var position = transform.position;
        return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }

    public HashSet<Vector2Int> GetUsedPositions()
    {
        var positions = new HashSet<Vector2Int>();
        var basePos = GetPosition() - size / 2;
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                positions.Add(new Vector2Int(x, y) + basePos);
            }
        }
        return positions;
    }
    
    public void SetManager(BuildingManager manager)
    {
        if (this.manager != null)
        {
            Debug.LogError("Manager already set");
        }
        this.manager = manager;
    }
}
