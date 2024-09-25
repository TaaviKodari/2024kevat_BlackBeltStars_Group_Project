using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
public class ResourceType : ScriptableObject
{
    public string id;
    public Sprite sprite;
    
    private void OnValidate()
    {
        id = id.ToLowerInvariant().Replace(' ', '_');
    }
}