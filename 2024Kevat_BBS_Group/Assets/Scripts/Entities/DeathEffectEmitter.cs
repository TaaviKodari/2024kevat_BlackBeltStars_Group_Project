using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class DeathEffectEmitter : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabs;

    private void Start()
    {
        GetComponent<Entity>().onKilled.AddListener(() =>
        {
            foreach (var prefab in prefabs)
            {
                Instantiate(prefab, transform.position, transform.rotation);
            }
        });
    }
}