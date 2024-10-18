using System.Collections;
using System.Collections.Generic;
using GameState;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public int currentWave; // the current wave of the game
    
    private float timer; // used to keep track of time between rounds
    
    public float timeBetweenWaves; // the time between spawning waves
    
    public EnemyManager enemyManager; // reference to the EnemyManager to actually spawn the enemies
    
    public int enemyLimit; // limiting the total number of enemies
    
    public Enemy enemyPrefab1; // prefab for the test enemy
    
    public int minRange; // min distance from the player, where the enemy can spawn
    public int maxRange; // max distance from the player, where the enemy can spawn
    
    public TMP_Text waveTimerText; // value of the in-game text field for the time untill next wave
    public TMP_Text waveText; // value of the in-game text field for the current wave number

    void Awake()
    { 
        timer = 0f;                                     // in the beginning, set the timer to 0
        currentWave = 0;                                // game starts from round 1
    }

    void Update()
    {
        // update the in-game text boxes
        waveTimerText.text = "Next wave in: "+((timeBetweenWaves - timer).ToString("F2"));
        waveText.text = "Current wave: " + (currentWave).ToString();
        
        timer += Time.deltaTime; // update the timer
        
        if (timer >= timeBetweenWaves)
        {
            timer = 0f; // reset the timer
            StartWave(); // start the wave
        }
    }
    
    // begins spawning enemies and changes the wave number by 1
    void StartWave()
    {
        currentWave++; // change the round 
        SpawnEnemies(enemyPrefab1); // in the future when there's more enemies, spawn a random prefab?

        if (currentWave != 0)
        {
            LiveGameTracker.Instance.AddWaveSurvived();
        }
    }

    // if round is n, spawn n enemies
    private void SpawnEnemies(Enemy prefab)
    {
        for (int i = 1; i <= currentWave; i++) // untill i reaches the wave number, do the following
        {
            if (enemyManager.GetEnemyCount() < enemyLimit) // if there isn't too many enemies already
            {
                // Get a random position within the specified range from the player
                Vector2 position =
                    enemyManager.GetRandomPosition(enemyManager.player.transform.position, minRange, maxRange);
                // If a valid position is found, spawn the enemy at that position
                if (position != Vector2.zero)
                    enemyManager.SpawnEnemy(prefab, position);
            }
        }
    }
}
