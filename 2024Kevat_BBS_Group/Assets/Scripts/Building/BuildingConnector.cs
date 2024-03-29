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

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
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
