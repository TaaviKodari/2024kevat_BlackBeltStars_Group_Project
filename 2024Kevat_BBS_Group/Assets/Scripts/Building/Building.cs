using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData data;
    [CanBeNull, SerializeField]
    private HealthBar healthBar;
    private BuildingManager manager;
    private float durability;
    public bool isVertical;
    private Vector2Int Size => data.GetSize(isVertical);

    private void Awake()
    {
        manager = FindObjectOfType<BuildingManager>();
    }

    private void Start()
    {
        durability = data.durability;
        FindObjectOfType<PathfindingManager>().UpdateChunks((Vector2) transform.position - data.offset, Size);
        if (healthBar != null)
        {
            healthBar.SetHealth(durability, data.durability);
        }
    }
    
    private void OnDestroy()
    {
        manager.RemoveBuilding(this);
        FindObjectOfType<PathfindingManager>().UpdateChunks((Vector2) transform.position - data.offset, Size);
    }
    
    // Gets the position in the center of this building. Can be off by 0.5 for buildings with an even size
    public Vector2Int GetPosition()
    {
        return manager.WorldPosToBuildingPos((Vector2) transform.position - data.offset);
    }

    // Returns positions used by this building so no other buildings can build that overlap this building
    public HashSet<Vector2Int> GetUsedPositions()
    {
        var positions = new HashSet<Vector2Int>();
        var basePos = GetPosition() - Size / 2;
        for (var x = 0; x < Size.x; x++)
        {
            for (var y = 0; y < Size.y; y++)
            {
                positions.Add(new Vector2Int(x + basePos.x, y + basePos.y));
            }
        }
        return positions;
    }

    public void DoDamage(float amount)
    {
        durability = Mathf.Max(durability - amount, 0); // Prevent going below zero
        if (healthBar != null)
        {
            healthBar.SetHealth(durability, data.durability);
        }
        if (durability == 0) Destroy(gameObject);
    }
}
