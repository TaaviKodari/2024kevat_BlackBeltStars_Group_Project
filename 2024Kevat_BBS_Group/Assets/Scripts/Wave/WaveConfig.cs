using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Waves/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    public int enemyCount;
    public Enemy[] enemyTypes;
    public float healthMultiplier;
    public float speedMultiplier;
    public bool isMiniBossWave;
}
