using System;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

// Handles spawning of naturally occuring objects
public class TerrainDecorator : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    [Tooltip("The size of the chunks in which things are generated")]
    private Vector2Int regionSize = new Vector2Int(100, 100);
    [SerializeField]
    private int worldSeed = 1;
    [SerializeField]
    [Tooltip("A list of objects to place")]
    private List<Placement> placements;

    private readonly ISet<Vector2Int> generatedRegions = new HashSet<Vector2Int>();
    private Vector2Int prevRegion;

    private void Awake()
    {
        // Set the region to a bogus value to ensure it gets updated next frame
        prevRegion = new Vector2Int(int.MaxValue, int.MaxValue);
    }

    private void Update()
    {
        // If the player has entered a new region we update the regions around them
        var currentRegion = GetCurrentRegion();
        if (currentRegion == prevRegion) return;
        prevRegion = currentRegion;
        UpdateRegions();
    }

    // Used to update the placed objects when changing values in editor at runtime
    // Makes it easier to fine tune placements
    private void OnValidate()
    {
        if (!Application.isPlaying || !Application.isEditor) return;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        generatedRegions.Clear();
        prevRegion = new Vector2Int(int.MaxValue, int.MaxValue);
    }

    // Loops over the regions in a 3x3 area around the player and generates them if nessecary
    private void UpdateRegions()
    {
        for (var xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (var yOffset = -1; yOffset <= 1; yOffset++)
            {
                var region = prevRegion + new Vector2Int(xOffset, yOffset);
                if (!generatedRegions.Contains(region))
                {
                    GenerateRegion(region);
                }
            }
        }
    }

    // Populates a region with objects
    private void GenerateRegion(Vector2Int region)
    {
        var random = new Random(GetRegionSeed(region));
        
        foreach (var placement in placements)
        {
            for (var xIndex = 0; xIndex < placement.gridSize; xIndex++)
            {
                for (var yIndex = 0; yIndex < placement.gridSize; yIndex++)
                {
                    // Compute position for a object
                    var x = ((xIndex + random.NextFloat(-placement.offset, placement.offset)) / placement.gridSize + region.x) * regionSize.x;
                    var y = ((yIndex + random.NextFloat(-placement.offset, placement.offset)) / placement.gridSize + region.y) * regionSize.y;
                    
                    // Check the noise for if were allowed to place it
                    if (Mathf.PerlinNoise(x / placement.noiseScale, y / placement.noiseScale) > placement.noiseCutoff) continue;
                    
                    // Place the object
                    Instantiate(placement.prefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                }
            }
        }
        
        // Mark the region as generated to not do work twice
        generatedRegions.Add(region);
    }

    // Computes the region the player is currently standing in
    private Vector2Int GetCurrentRegion()
    {
        return new Vector2Int(Mathf.FloorToInt(player.position.x / regionSize.x), Mathf.FloorToInt(player.position.y / regionSize.y));
    }

    // Hashes a region position along with the world seed to get a unique region seed,
    private uint GetRegionSeed(Vector2Int region)
    {
        var seed = worldSeed;
        seed = 290993 * seed + region.x;
        seed = 290993 * seed + region.y;
        return (uint)seed;
    }
    
    [Serializable]
    public struct Placement
    {
        [Tooltip("The prefab that will be placed")]
        public Transform prefab;
        [Tooltip("How many objects should be placed per axis in each region")]
        public int gridSize;
        [Tooltip("The max distance between an objects grid position and final position")]
        [Range(0, 1)]
        public float offset;
        [Tooltip("A scaling factor for the perlin noise used for filtering objects based on region")]
        public float noiseScale;
        [Tooltip("Limits how low the noise can get a a position before objects stop spawning there. Set to 1 to ignore noise.")]
        [Range(0, 1)]
        public float noiseCutoff;
    }
}
