using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    public Vector2Int size = Vector2Int.one;
    public GameObject prefab;
}
