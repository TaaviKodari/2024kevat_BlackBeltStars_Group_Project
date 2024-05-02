using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int limit;
    public float minWait;
    public float maxWait;
    public float minRange;
    public float maxRange;
    public Enemy prefab;
    private EnemyManager manager;

    void Awake()
    {
        manager = GetComponent<EnemyManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", Random.Range(minWait, maxWait));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        if (manager.GetEnemyCount() < limit)
            manager.SpawnEnemy(prefab, manager.GetRandomPosition(manager.player.transform.position, minRange, maxRange));
        Invoke("Spawn", Random.Range(minWait, maxWait));
    }
}
