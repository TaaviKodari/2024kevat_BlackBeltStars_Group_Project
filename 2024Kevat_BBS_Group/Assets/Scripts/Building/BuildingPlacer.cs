﻿using JetBrains.Annotations;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    private BuildingManager manager;

    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material previewMaterial;
    [SerializeField]
    private Material invalidPreviewMaterial;
    [SerializeField] 
    private Material hoverMaterial;

    [SerializeField]
    private BuildingData wallBuildData;
    [SerializeField]
    private BuildingData gateBuildData;
    [SerializeField]
    private BuildingData trapBuildData;
    [SerializeField]
    private BuildingData campfireBuildData;
    [SerializeField]
    private BuildingData arrowTowerBuildData;
    
    
    private BuildingData selectedBuilding;
    private GameObject buildingPreview;
    private GameObject hoveredBuilding;

    private bool isLinePlacing;
    private Vector2Int lineStartPos;
    private GameObject[] previewDots;

    [SerializeField]
    private GameObject previewDotPrefab;
    [SerializeField]
    private int previewDotsLimit = 25;

    private void Awake()
    {
        manager = FindObjectOfType<BuildingManager>();
        FindObjectOfType<PathfindingManager>();

        previewDots = new GameObject[previewDotsLimit];
        for (int i = 0; i < previewDotsLimit; i++)
        {
            previewDots[i] = Instantiate(previewDotPrefab, transform);
            previewDots[i].SetActive(false);
        }
    }

    private void Update()
    {
        HandleInput();
        UpdatePreview();
        UpdateHoveredBuilding();
        HandleBuildingSelectionHotkeys();
    }

    private void HandleInput()
    {
        // If the mouse is over a UI element, don't do anything. (The method has a somewhat confusing name)
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (player.input.Building.Place.WasPressedThisFrame() && selectedBuilding == wallBuildData && isShiftPressed)
        {
            isLinePlacing = true;
            lineStartPos = GetSelectedTile(Vector2Int.one);
            UpdateLinePlacementPreview();
        } else if (player.input.Building.Place.WasReleasedThisFrame() && isLinePlacing)
        {
            if (isShiftPressed)
            {
                PlaceWallLine();
            }
            CancelLinePlacement();
        } else if (!isShiftPressed && isLinePlacing)
        {
            CancelLinePlacement();
        } else if (player.input.Building.Place.IsPressed() && selectedBuilding != null && !isLinePlacing)
        {
            PlaceBuilding();
        }

        if (isLinePlacing)
        {
            UpdateLinePlacementPreview();
        }

        if (player.input.Building.Cancel.WasReleasedThisFrame() && selectedBuilding != null)
        {
            SelectBuilding(null);
        }
        else if (player.input.Building.Destroy.IsPressed() && selectedBuilding == null)
        {
            var buildingPlacementPos = GetSelectedTile(Vector2Int.one);
            var building = manager.GetBuildingAt(buildingPlacementPos);
            if (building != null)
            {
                BuildingManager.ReturnBuildingResources(building);
                Destroy(building.gameObject);
            }
        }
    }
    
    private void CancelLinePlacement()
    {
        isLinePlacing = false;
        foreach (var dot in previewDots)
        {
            dot.SetActive(false);
        }
    }

    private void UpdateLinePlacementPreview()
    {
        if (!isLinePlacing) return;

        var currentPos = GetSelectedTile(Vector2Int.one);
        var (linePoints, _) = CalculateWallLine(lineStartPos, currentPos);

        // update preview dots
        for (int i = 0; i < previewDots.Length; i++)
        {
            if (i < linePoints.Count)
            {
                previewDots[i].SetActive(true);
                previewDots[i].transform.position = manager.BuildingPosToWorldPos(linePoints[i]) + new Vector2(0.5f, 0.5f);
            }
            else
            {
                previewDots[i].SetActive(false);
            }
        }
    }

    private (List<Vector2Int> points, Vector2Int corner) CalculateWallLine(Vector2Int start, Vector2Int end)
    {
        var points = new List<Vector2Int>();
        var corner = new Vector2Int(start.x, end.y);
    
        points.Capacity = Math.Abs(end.y - start.y) + Math.Abs(end.x - start.x);

        int verticalStep = Math.Sign(end.y - start.y);
        if (verticalStep != 0)
        {
            for (int y = start.y; y != end.y + verticalStep; y += verticalStep)
            {
                points.Add(new Vector2Int(start.x, y));
            }
        }
        else
        {
            points.Add(start);
        }

        int horizontalStep = Math.Sign(end.x - start.x);
        if (horizontalStep != 0)
        {
            for (int x = start.x + horizontalStep; x != end.x + horizontalStep; x += horizontalStep)
            {
                points.Add(new Vector2Int(x, end.y));
            }
        }

        return (points, corner);
    }

    private void PlaceWallLine()
    {
        var currentPos = GetSelectedTile(Vector2Int.one);
        var (linePoints, _) = CalculateWallLine(lineStartPos, currentPos);

        foreach (var point in linePoints)
        {
            var worldPos = manager.BuildingPosToWorldPos(point) + new Vector2(0.5f, 0.5f);
            var building = Instantiate(wallBuildData.GetPrefab(IsPlacingVertical()), worldPos, Quaternion.identity, transform)
                .GetComponent<Building>();
            
            building.data = wallBuildData;
            building.name = wallBuildData.name;
            SetMaterial(building.gameObject, normalMaterial);
            manager.TryAddBuilding(building);
        }
    }

    private Vector3 GetPlacementPos()
    {
        if (selectedBuilding == null) return Vector3.zero;
        var size = selectedBuilding.GetSize(IsPlacingVertical());
        return manager.BuildingPosToWorldPos(GetSelectedTile(size)) + new Vector2(math.frac(size.x / 2f), math.frac(size.y / 2f)) + selectedBuilding.offset;
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
        return manager.WorldPosToBuildingPos(mousePos);
    }
    
    private void PlaceBuilding()
    {
        var pos = GetPlacementPos();
        var building = Instantiate(selectedBuilding.GetPrefab(IsPlacingVertical()), pos, Quaternion.identity, transform).GetComponent<Building>();
        building.data = selectedBuilding;
        building.name = selectedBuilding.name;
        // Apply normal material to placed buildings to ensure that they look the same even after hovering
        SetMaterial(building.gameObject, normalMaterial);
        manager.TryAddBuilding(building);
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


    // Allows the player to select a building by pressing the corresponding key
    private void HandleBuildingSelectionHotkeys() {
        var buildingHotkeys = new (bool wasPressed, BuildingData buildData)[] {
            (player.input.Building.SelectWall.WasPressedThisFrame(), wallBuildData),
            (player.input.Building.SelectGate.WasPressedThisFrame(), gateBuildData),
            (player.input.Building.SelectTrap.WasPressedThisFrame(), trapBuildData),
            (player.input.Building.SelectCampfire.WasPressedThisFrame(), campfireBuildData),
            (player.input.Building.SelectArrowTower.WasPressedThisFrame(), arrowTowerBuildData)
        };

        foreach (var (wasPressed, buildData) in buildingHotkeys) {
            if (wasPressed) {
                if (selectedBuilding == buildData) {
                    SelectBuilding(null);
                } else {
                    SelectBuilding(buildData);
                }
            }
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
        
        var building = manager.GetBuildingAt(GetSelectedTile(Vector2Int.one));
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
        var preview = Instantiate(selectedBuilding.GetPrefab(IsPlacingVertical()), pos, Quaternion.identity, transform);
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
        var pos = GetPlacementPos();
        buildingPreview.transform.position = pos;

        var building = buildingPreview.GetComponent<Building>();
        if (building.data.HasSplitPrefab() && building.isVertical != IsPlacingVertical())
        {
            Destroy(buildingPreview);
            CreatePreview();
        }

        // Sets the material of the preview
        var material = manager.CanPlace(buildingPreview.GetComponent<Building>()) ? previewMaterial : invalidPreviewMaterial;
        SetMaterial(buildingPreview, material);
    }

    private static void SetMaterial(GameObject obj, Material mat)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
        {
            renderer.sharedMaterial = mat;
        }
    }

    public BuildingData GetBuilding()
    {
        return selectedBuilding;
    }

    private bool IsPlacingVertical()
    {
        return manager.IsVertical(manager.BuildingPosToWorldPos(GetSelectedTile(Vector2Int.one)));
    }
}