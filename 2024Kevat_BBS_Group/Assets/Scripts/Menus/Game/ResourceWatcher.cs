using TMPro;
using UnityEngine;

public class ResourceWatcher : MonoBehaviour
{
    [SerializeField]
    private ResourceType resource;

    private TMP_Text tmpText;

    private void Awake()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        tmpText.text = ResourceManager.Instance.GetResourceAmount(resource).ToString();
    }
}