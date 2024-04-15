using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BuildingManager : MonoBehaviour
{
    // Layer mask used to check if a building can be placed at a certain position using collisions
    // Buildings are excluded as they can be accurately checked using the position data
    // Initialized in Awake
    private int buildingPlacementLayerMask;
    
    [SerializeField]
    private PlayerController player;
    [FormerlySerializedAs("material")] [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material previewMaterial;
    [SerializeField]
    private Material invalidPreviewMaterial;
    [SerializeField] 
    private Material hoverMaterial;
    [SerializeField]
    private Vector2 tileSize = new Vector2(1, 1);

    private MenuController menuController;
        
    private BuildingData selectedBuilding;
    private GameObject buildingPreview;
    private GameObject hoveredBuilding;
    private readonly HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();
    private readonly Dictionary<Vector2Int, Building> positionToBuilding = new Dictionary<Vector2Int, Building>();
    private readonly HashSet<Building> buildings = new HashSet<Building>();

    public bool IsBuilding(Vector2Int pos)
    {
        return usedPositions.Contains(pos);
    }
    
    private void Awake()
    {
        menuController = GameObject.Find("Canvas").GetComponent<MenuController>();
        // Have to init this here, unity doesn't allow for layer masks to be created before awake
        buildingPlacementLayerMask = ~LayerMask.GetMask("Buildings", "Ignore Raycast");
    }

    private void Update()
    {
        HandleInput();
        UpdatePreview();
        UpdateHoveredBuilding();
        CheckToggle();
    }

    private void HandleInput()
    {
        // If the mouse is over a UI element, don't do anything. (The method has a somewhat confusing name)
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (player.input.Building.Place.IsPressed() && selectedBuilding != null)
        {
            PlaceBuilding();
        }
        if (player.input.Building.Cancel.WasReleasedThisFrame() && selectedBuilding != null)
        {
            SelectBuilding(null);
        }
        else if (player.input.Building.Destroy.IsPressed() && selectedBuilding == null)
        {
            var buildingPlacementPos = GetSelectedTile(Vector2Int.one);
            if (positionToBuilding.TryGetValue(buildingPlacementPos, out var building))
                Destroy(building.gameObject);
        }
    }

    private Vector2Int GetSelectedTile(Vector2Int size)
    {
        var camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("No main camera found");
            return Vector2Int.zero;
        }
        var mousePos = camera.ScreenToWorldPoint(player.input.Building.MousePosition.ReadValue<Vector2>());
        mousePos.x += math.frac(size.x / 2f + 0.5f);
        mousePos.y += math.frac(size.y / 2f + 0.5f);
        return WorldPosToBuildingPos(mousePos);
    }

    private Vector3 GetPlacementPos()
    {
        if (selectedBuilding == null) return Vector3.zero;
        var size = selectedBuilding.size;
        return BuildingPosToWorldPos(GetSelectedTile(size)) + new Vector2(math.frac(size.x / 2f), math.frac(size.y / 2f));
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

    [UsedImplicitly] // Assigned to buttons in the editor
    public void SelectBuilding(BuildingData building)
    {
        if (buildingPreview != null)
            Destroy(buildingPreview);
        selectedBuilding = building;
        if (selectedBuilding != null)
            CreatePreview();
    }

    private void PlaceBuilding()
    {
        var pos = GetPlacementPos();
        var building = Instantiate(selectedBuilding.prefab, pos, Quaternion.identity, transform).GetComponent<Building>();
        building.data = selectedBuilding;
        var buildingPositions = building.GetUsedPositions();
        if (!CanPlace(building))
        {
            Destroy(building.gameObject);
            return;
        }
        usedPositions.UnionWith(buildingPositions);
        buildings.Add(building);
        buildingPositions.ToList().ForEach(p => positionToBuilding.Add(p, building));

        building.name = selectedBuilding.name;
        // Apply normal material to placed buildings to ensure that they look the same even after hovering
        SetMaterial(building.gameObject, normalMaterial);
        
        foreach (var cost in building.data.costs)
        {
            ResourceManager.Instance.RemoveResource(cost.type, cost.amount);
        }
    }

    public void RemoveBuilding(Building building)
    {
        if (!buildings.Contains(building)) return;
        var buildingPositions = building.GetUsedPositions();
        usedPositions.ExceptWith(buildingPositions);
        buildings.Remove(building);
        buildingPositions.ToList().ForEach(p => positionToBuilding.Remove(p));

        foreach (var cost in building.data.costs)
        {
            ResourceManager.Instance.AddResource(cost.type, Mathf.FloorToInt(cost.amount * 0.75f));
        }
    }

    private void UpdateHoveredBuilding()
    {
        if (selectedBuilding != null)
        {
            if (hoveredBuilding != null)
            {
                SetMaterial(hoveredBuilding, normalMaterial);
                hoveredBuilding = null;
            }
            return;
        }
        
        positionToBuilding.TryGetValue(GetSelectedTile(Vector2Int.one), out var building);
        var buildingObject = building == null ? null : building.gameObject;
        if (buildingObject == hoveredBuilding) return;

        if (hoveredBuilding != null)
        {
            SetMaterial(hoveredBuilding, normalMaterial);
        }
        hoveredBuilding = buildingObject;
        if (hoveredBuilding != null)
        {
            SetMaterial(hoveredBuilding, hoverMaterial);
        }
    }

    private void CreatePreview()
    {
        var pos = GetPlacementPos();
        var preview = Instantiate(selectedBuilding.prefab, pos, Quaternion.identity, transform);
        preview.GetComponent<Building>().data = selectedBuilding;
        
        // Remove all scripts from the preview. This is a way to prevent the preview from doing anything.
        foreach (var script in preview.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (script is Building) continue; 
            Destroy(script);
        }
        // Remove all colliders from the preview. This is a way to prevent the preview from colliding with anything.
        foreach (var collider in preview.GetComponentsInChildren<Collider2D>(true))
        {
            Destroy(collider);
        }

        buildingPreview = preview;
        buildingPreview.name = "BuildingPreview";
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (buildingPreview == null) return;
        buildingPreview.transform.position = GetPlacementPos();
        // Sets the material of the preview
        var material = CanPlace(buildingPreview.GetComponent<Building>()) ? previewMaterial : invalidPreviewMaterial;
        SetMaterial(buildingPreview, material);
    }
    
    private bool CanPlace(Building building)
    {
        var costs = building.data.costs;
        if (costs.Any(cost => ResourceManager.Instance.GetResourceAmount(cost.type) < cost.amount))
        {
            return false;
        }
        
        var buildingPositions = building.GetUsedPositions();
        if (usedPositions.Overlaps(buildingPositions)) return false;
        var halfVec = new Vector2(0.5f, 0.5f);
        foreach (var pos in buildingPositions)
        {
            if (Physics2D.OverlapBox(pos + halfVec, Vector2.one, 0, buildingPlacementLayerMask) != null)
                return false;
        }
        return true;
    }

    private static void SetMaterial(GameObject obj, Material mat)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
        {
            renderer.sharedMaterial = mat;
        }
    }
    
    // TODO: move to menu controller
    private void CheckToggle()
    {
        if (!player.input.Building.ToggleBuilding.triggered) return;
        
        switch(menuController.CurrentMenuState)
        {
            case MenuController.MenuStates.None:
                menuController.CurrentMenuState = MenuController.MenuStates.Build;
                break;
            case MenuController.MenuStates.Build:
                menuController.CurrentMenuState = MenuController.MenuStates.None;
                break;
        }
    }
}