using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingConnector : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite alone;
    public Sprite lr;
    public Sprite tb;
    public Sprite lt;
    public Sprite rt;
    public Sprite lb;
    public Sprite rb;
    public Sprite lrt;
    public Sprite lrb;
    public Sprite ltb;
    public Sprite rtb;
    public Sprite lrtb;
    private SpriteRenderer sr;
    private BuildingManager buildingManager;
    private Building building;
    private new PolygonCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponentInChildren<PolygonCollider2D>();
        buildingManager = FindObjectOfType<BuildingManager>();
        building = GetComponent<Building>();
    }

    // Update is called once per frame
    void Update()
    {
        Connect();
    }

    private void Connect()
    {
        int minX = building.GetUsedPositions().Min((Vector2Int pos) => pos.x);
        int maxX = building.GetUsedPositions().Max((Vector2Int pos) => pos.x);
        int minY = building.GetUsedPositions().Min((Vector2Int pos) => pos.y);
        int maxY = building.GetUsedPositions().Max((Vector2Int pos) => pos.y);

        bool l = buildingManager.IsBuilding(new Vector2Int(minX - 1, minY));
        bool r = buildingManager.IsBuilding(new Vector2Int(maxX + 1, minY));
        bool t = buildingManager.IsBuilding(new Vector2Int(minX, maxY + 1));
        bool b = buildingManager.IsBuilding(new Vector2Int(minX, minY - 1));

        sr.sprite = GetSprite(l, r, t, b);
        UpdateCollider(sr.sprite);
    }

    // Unity doesn't update the collider when the sprite is changed at runtime
    // and doesn't even provide a method to do so, so we have to do it manually
    private void UpdateCollider(Sprite sprite)
    {
        collider.pathCount = sprite.GetPhysicsShapeCount();
        
        // Reuse the list
        var path = new List<Vector2>();
        for (var i = 0; i < collider.pathCount; i++) {
            path.Clear();
            sprite.GetPhysicsShape(i, path);
            collider.SetPath(i, path.ToArray());
        }
    }

    private Sprite GetSprite(bool l, bool r, bool t, bool b)
    {
        if (l && r && t && b) return lrtb;

        if (l && r && t) return lrt;
        if (l && r && b) return lrb;
        if (l && t && b) return ltb;
        if (r && t && b) return rtb;

        if (l && t) return lt;
        if (r && t) return rt;
        if (l && b) return lb;
        if (r && b) return rb;

        if (l || r) return lr;
        if (t || b) return tb;
    
        return alone;
    }
}
