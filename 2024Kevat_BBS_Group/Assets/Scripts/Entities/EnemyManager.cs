using System.Collections;
using System.Collections.Generic;
using GameState;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Singleton instance to ensure only one EnemyManager exists
    public static EnemyManager instance { get; private set; }
    // Reference to the player GameObject
    public GameObject player { get; private set; }
    // List to keep track of all enemies currently in the game
    public List<Enemy> enemies = new List<Enemy>();

    // Called when the script instance is being loaded
    void Awake()
    {
        // Ensure only one instance of EnemyManager exists (Singleton pattern)
        if (instance != null && instance != this)
        {
            Destroy(this); // Destroy this instance if another instance already exists
        }
        else
        {
            instance = this; // Assign this instance as the Singleton instance
            // Find the player GameObject by its tag
            player = GameObject.FindWithTag("Player");
        }
    }

    // Spawns a new enemy at the given position
    public void SpawnEnemy(Enemy enemyPrefab, Vector2 position)
    {
        // Instantiate the enemy prefab at the given position with no rotation as a child of the manager
        var enemy = Instantiate(enemyPrefab, position, Quaternion.identity, transform);
        // Add the new enemy to the list of enemies
        enemies.Add(enemy);
        // Set the enemy's manager reference to this instance of EnemyManager
        enemy.manager = this;
    }

    // Removes the enemy from the list when it dies
    public void EnemyDie(Enemy enemy)
    {
        // Remove the enemy from the enemies list
        enemies.Remove(enemy);
        LiveGameTracker.Instance.AddKilledAnt();
    }

    // Returns the number of enemies currently in the game
    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    // Returns a random position a certain distance from the player, avoiding obstacles (donut-shaped area)
    public Vector2 GetRandomPosition(Vector2 origin, float minRange, float maxRange)
    {
        // Try up to 10 times to find a valid position
        for (int i = 0; i < 10; i++)
        {
            // Generate a random direction and distance within the specified range
            Vector2 dir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minRange, maxRange);
            Vector2 position = origin + dir * distance;

            // Check if the position is not inside a collider
            bool isValid = Physics2D.OverlapCircle(position, 1f) == null;

            // Check if the position is too close to a campfire
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5);
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<Campfire>() != null)
                {
                    isValid = false;
                    break; // Break out of the loop if the position is near a campfire
                }
            }

            // If the position is valid, return it
            if (isValid)
            {
                return position;
            }
        }
        // Return Vector2.zero if no valid position is found after 10 tries
        return Vector2.zero;
    }
}
