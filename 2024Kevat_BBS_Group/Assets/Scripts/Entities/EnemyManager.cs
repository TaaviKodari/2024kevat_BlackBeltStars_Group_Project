using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance { get; private set; }
    public GameObject player { get; private set; }
    private List<Enemy> enemies = new List<Enemy>();
    
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            player = GameObject.FindWithTag("Player");
        }
    }

    public void SpawnEnemy(Enemy enemyPrefab, Vector2 position)
    {
        var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemies.Add(enemy);
        enemy.manager = this;
    }

    public void EnemyDie(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    // Returns random position a certain distance away from player. (Donut shape)
    public Vector2 GetRandomPosition(Vector2 origin, float minRange, float maxRange)
    {
        for(int i = 0; i < 10; i++)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minRange, maxRange);
            Vector2 position = origin + dir * distance;

            //If not inside a collider, return valid position.
            if (Physics2D.OverlapCircle(position, 1f) == null)
            {
                return position;
            }
        }
        return Vector2.zero;
    }
}
