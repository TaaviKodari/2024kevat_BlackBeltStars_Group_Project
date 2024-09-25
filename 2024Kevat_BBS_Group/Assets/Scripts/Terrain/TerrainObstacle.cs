using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Terrain Obstacle", menuName = "ScriptableObjects/Terrain Obstacle", order = 1)]
public class TerrainObstacle : ScriptableObject
{
    public string id;
    public TerrainDecorator.Placement defaultPlacement;

    private void OnValidate()
    {
        id = id.ToLowerInvariant().Replace(' ', '_');
    }
}