#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShopItemConfig))]
public class ShopItemConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ShopItemConfig config = (ShopItemConfig)target;
        EditorGUI.BeginChangeCheck();

        config.cost = EditorGUILayout.IntField("Cost", config.cost);
        config.itemName = EditorGUILayout.TextField("Item Name", config.itemName);
        config.description = EditorGUILayout.TextField("Item Description", config.description);
        config.itemSprite = (Sprite)EditorGUILayout.ObjectField("Item Sprite", config.itemSprite, typeof(Sprite), false);
        config.currencyType = (ShopItemConfig.CurrencyType)EditorGUILayout.EnumPopup("Currency Type", config.currencyType);

        // fields for HealthBoost properties
        config.healthBoost.multiplier = EditorGUILayout.FloatField("Health Boost Multiplier", config.healthBoost.multiplier);
        config.healthBoost.duration = EditorGUILayout.IntField("Health Boost Duration", config.healthBoost.duration);
        
        // fields for SpeedBoost properties
        config.speedBoost.multiplier = EditorGUILayout.FloatField("Speed Boost Multiplier", config.speedBoost.multiplier);
        config.speedBoost.duration = EditorGUILayout.IntField("Speed Boost Duration", config.speedBoost.duration);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets(); // Ensure changes are saved
        }
    }
}
#endif