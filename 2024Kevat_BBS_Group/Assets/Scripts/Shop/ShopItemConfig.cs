using GameState;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/ShopItemConfig")]
public class ShopItemConfig : ScriptableObject
{
    public enum CurrencyType
    {
        Gold,
        Diamonds
    }

    public string itemName;
    public string itemDescription;
    public CurrencyType currency;
    public int cost;

    [SerializeReference, SubclassSelector]
    public IBooster booster;
}
