using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Maximum number of enemies that can be spawned at once
    public int limit;
    // Minimum wait time between spawning attempts
    public float minWait;
    // Maximum wait time between spawning attempts
    public float maxWait;
    // Minimum range from the player within which enemies can spawn
    public float minRange;
    // Maximum range from the player within which enemies can spawn
    public float maxRange;
    // Prefab of the enemy to spawn
    public Enemy prefab;
    // Reference to the EnemyManager that handles enemy behavior
    private EnemyManager manager;

    // Called when the script instance is being loaded
    void Awake()
    {
        // Get the EnemyManager component attached to the same GameObject
        manager = GetComponent<EnemyManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start the spawning process by invoking the Spawn method after a random delay (currently disabled)
        // Invoke("Spawn", Random.Range(minWait, maxWait));
    }

    // Update is called once per frame (currently not used)
    void Update()
    {

    }

    // Method to spawn enemies if the limit hasn't been reached
    void Spawn()
    {
        // Check if the current number of enemies is less than the limit
        if (manager.GetEnemyCount() < limit)
        {
            // Get a random position within the specified range from the player
            Vector2 position = manager.GetRandomPosition(manager.player.transform.position, minRange, maxRange);
            // If a valid position is found, spawn the enemy at that position
            if (position != Vector2.zero)
                manager.SpawnEnemy(prefab, position, 1f, 1f);
        }

        // Schedule the next spawn attempt with a random delay
        Invoke("Spawn", Random.Range(minWait, maxWait));
    }
}