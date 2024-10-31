#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveConfig))]
public class WaveConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WaveConfig config = (WaveConfig)target;

        config.enemyCount = EditorGUILayout.IntField("Enemy Count", config.enemyCount);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyTypes"), true);
        config.healthMultiplier = EditorGUILayout.FloatField("Health Multiplier", config.healthMultiplier);
        config.speedMultiplier = EditorGUILayout.FloatField("Speed Multiplier", config.speedMultiplier);
        config.isMiniBossWave = EditorGUILayout.Toggle("Is Mini-Boss Wave", config.isMiniBossWave);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif