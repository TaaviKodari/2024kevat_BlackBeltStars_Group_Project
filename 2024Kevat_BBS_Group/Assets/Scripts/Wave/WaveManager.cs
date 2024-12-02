using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameState;
using UnityEngine;
using TMPro;
using AtomicConsole;

public class WaveManager : MonoBehaviour
{
    public WaveConfig[] waves; // Array of wave configurations
    
    [AtomicSet(name:"SetWave")]
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

    // Counter for alive enemies, used to mark waves as completed
    private readonly Dictionary<int, int> enemiesAlive = new();

    void Awake()
    { 
        timer = 0f;                                     // in the beginning, set the timer to 0
        currentWave = 0;                                // game starts from round 1
    }

    void Update()
    {
        // update the in-game text boxes
        waveTimerText.text = "Next wave in: " + (timeBetweenWaves - timer).ToString("F2");
        waveText.text = "Current wave: " + currentWave;
        
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
        if (currentWave != 0)
        {
            InGameManager.Instance.AddWaveSurvived();
        }

        if (currentWave < waves.Length)
        {
            var currentConfig = waves[currentWave];
            if (currentConfig.isMiniBossWave)
            {
                Debug.Log("MiniBossWave");
                //TODO: SpawnMiniBoss();
            }
            else
            {
                SpawnEnemies(currentConfig, currentWave);
            }
            currentWave++;
        }
        else
        {
            Debug.Log("No waves left, 'freeplay'");
            GenerateScalingWave();

        }
    }

    void GenerateScalingWave()
    {
        int additionalEnemies = currentWave + 1;
        float healthMultiplier = 1 + currentWave * 0.1f;
        float speedMultiplier = 1 + currentWave * 0.05f;

        // Create a temporary WaveConfig for scaling waves
        WaveConfig scalingConfig = ScriptableObject.CreateInstance<WaveConfig>();
        scalingConfig.enemyTypes = new Enemy[] { enemyPrefab1 }; // using the test enemy prefab for now
        scalingConfig.enemyCount = additionalEnemies;
        scalingConfig.healthMultiplier = healthMultiplier;
        scalingConfig.speedMultiplier = speedMultiplier;
        scalingConfig.isMiniBossWave = false;

        SpawnEnemies(scalingConfig, currentWave);

        currentWave++;
    }

    private void MarkEnemyKilled(int wave)
    {
        if (!enemiesAlive.ContainsKey(wave))
        {
            Debug.LogWarning("Somehow more enemies have died than were alive, not good", this);
            return;
        }

        enemiesAlive[wave] -= 1;
        if (enemiesAlive[wave] != 0) return;
        enemiesAlive.Remove(wave);
        InGameManager.Instance.AddWaveSurvived();
    }

    private void SpawnEnemies(WaveConfig config, int wave)
    {
        foreach (var enemy in config.enemyTypes)
        {
            for (var i = 0; i < config.enemyCount; i++)
            {
                var position = enemyManager.GetRandomPosition(enemyManager.player.transform.position, minRange, maxRange);
                var spawned = enemyManager.SpawnEnemy(enemy, position, config.healthMultiplier, config.speedMultiplier);
                spawned.onKilled.AddListener(() => MarkEnemyKilled(wave));

                enemiesAlive.TryAdd(wave, 0);
                enemiesAlive[wave]++;
            }
        }
    }
}
