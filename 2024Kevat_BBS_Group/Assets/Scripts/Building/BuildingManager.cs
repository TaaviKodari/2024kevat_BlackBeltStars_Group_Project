using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    // Layer mask used to check if a building can be placed at a certain position using collisions
    // Buildings are excluded as they can be accurately checked using the position data
    // Initialized in Awake
    private int buildingPlacementLayerMask;
    
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Material previewMaterial;
    [SerializeField]
    private Material invalidPreviewMaterial;
    
    private Building selectedBuilding;
    private GameObject buildingPreview;
    private readonly HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();
    private readonly Dictionary<Vector2Int, Building> buildings = new Dictionary<Vector2Int, Building>();
    
    private void Awake()
    {
        selectedBuilding = null;
        // Have to init this here, unity doesn't allow for layer masks to be created before awake
        buildingPlacementLayerMask = ~LayerMask.GetMask("Buildings", "Ignore Raycast");
    }

    private void Update()
    {
        HandleInput();
        UpdatePreview();
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
            var buildingPlacementPos = GetBuildingPlacementPos();
            if (buildings.TryGetValue(buildingPlacementPos, out var building))
                Destroy(building.gameObject);
        }
    }

    private Vector3 GetPlacementPos()
    {
        var camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("No main camera found");
            return Vector3.zero;
        }

        var buildingSize = selectedBuilding != null ? selectedBuilding.size : Vector2Int.one;
        var pos = camera.ScreenToWorldPoint(player.input.Building.MousePosition.ReadValue<Vector2>());
        pos.z = 0;
        pos.x = math.round(pos.x + buildingSize.x / 2f) - buildingSize.x / 2f;
        pos.y = math.round(pos.y + buildingSize.y / 2f) - buildingSize.y / 2f;
        return pos; 
    }

    private Vector2Int GetBuildingPlacementPos()
    {
        var pos = GetPlacementPos();
        return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }

    [UsedImplicitly] // Assigned to buttons in the editor
    public void SelectBuilding(Building prefab)
    {
        if (buildingPreview != null)
            Destroy(buildingPreview);
        selectedBuilding = prefab;
        if (selectedBuilding != null)
            CreatePreview();
    }

    private void PlaceBuilding()
    {
        var pos = GetPlacementPos();
        var building = Instantiate(selectedBuilding, pos, Quaternion.identity, transform);
        var buildingPositions = building.GetUsedPositions();
        if (!CanPlace(building))
        {
            Destroy(building.gameObject);
            return;
        }
        usedPositions.UnionWith(buildingPositions);
        buildingPositions.ToList().ForEach(p => buildings.Add(p, building));
        
        building.name = selectedBuilding.name;
        building.SetManager(this);
    }

    public void RemoveBuilding(Building building)
    {
        var buildingPositions = building.GetUsedPositions();
        usedPositions.ExceptWith(buildingPositions);
        buildingPositions.ToList().ForEach(p => buildings.Remove(p));
    }

    private void CreatePreview()
    {
        var pos = GetPlacementPos();
        var preview = Instantiate(selectedBuilding, pos, Quaternion.identity, transform);
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

        buildingPreview = preview.gameObject;
        buildingPreview.name = "BuildingPreview";
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (buildingPreview == null) return;
        buildingPreview.transform.position = GetPlacementPos();
        // Sets the material of the preview
        var material = CanPlace(buildingPreview.GetComponent<Building>()) ? previewMaterial : invalidPreviewMaterial;
        foreach (var renderer in buildingPreview.GetComponentsInChildren<Renderer>(true))
            renderer.sharedMaterial = material;
    }
    
    private bool CanPlace(Building building)
    {
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
}
