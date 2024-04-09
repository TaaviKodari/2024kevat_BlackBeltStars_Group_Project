using UnityEngine;
using UnityEngine.UI;

public class ResourceWatcher : MonoBehaviour
{
    [SerializeField]
    private ResourceManager.ResourceType resource;

    private Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = ResourceManager.Instance.GetResourceAmount(resource).ToString();
    }
}