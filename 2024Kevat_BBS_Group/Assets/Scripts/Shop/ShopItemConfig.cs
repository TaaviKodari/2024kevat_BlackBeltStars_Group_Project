using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/ShopItemConfig")]
public class ShopItemConfig : ScriptableObject
{
    public enum CurrencyType
    {
        Gold,
        Diamonds
    }
    public int cost;
    public string itemName;
    public Sprite itemSprite;
    public string description;
    public CurrencyType currencyType;
    public GameState.HealthBoost healthBoost;
    public GameState.SpeedBoost speedBoost;
    public GameState.DamageBoost damageBoost;
}
