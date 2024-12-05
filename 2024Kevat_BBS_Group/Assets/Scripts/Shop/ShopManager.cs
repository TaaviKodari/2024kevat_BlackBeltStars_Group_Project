using System.Collections;
using System.Collections.Generic;
using GameState;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private GameStateManager gameStateManager;
    [SerializeField]
    private SceneTransition transition;

    
    private int index;

    // List of shop item configurations
    [SerializeField]
    private List<ShopItemConfig> shopItems;

    // Dictionary to map currency types to their sprites
    private Dictionary<ShopItemConfig.CurrencyType, Sprite> currencySprites;

    // Sprites for each currency type (assign these in the Inspector)
    [SerializeField]
    private Sprite goldSprite;
    [SerializeField]
    private Sprite diamondSprite;

    // Reference to the shop item prefab
    [SerializeField]
    private GameObject shopItemPrefab;

    // Reference to the currency text objects
    [SerializeField]
    private TMP_Text diamondText;
    [SerializeField]
    private TMP_Text goldText;
    
    void Start()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
        InitializeCurrencySprites();
        InitializeShop();
        UpdateCurrencyTextFields();
        
    }

    public void UpdateCurrencyTextFields()
    {
        if (gameStateManager != null)
        {
            goldText.text = gameStateManager.currentSaveGame.resources.gold.ToString();
            diamondText.text = gameStateManager.currentSaveGame.resources.diamonds.ToString();
        }
    }
    
    // Method to initialize the shop items
    private void InitializeShop()
    {
        foreach (var item in shopItems)
        {
            // Instantiate the shop item prefab at the predetermined spawn point
            var shopItemObject = Instantiate(shopItemPrefab, Vector3.zero, Quaternion.identity, transform);

            // Get the ShopItem component and initialize it
            var shopItem = shopItemObject.GetComponent<ShopItem>();
            var currencySprite = GetSprite(item.currencyType);
            shopItem.Initialize(item, currencySprite);
        }
    }

    // Method to initialize the currency sprites dictionary
    private void InitializeCurrencySprites()
    {
        currencySprites = new Dictionary<ShopItemConfig.CurrencyType, Sprite>
        {
            { ShopItemConfig.CurrencyType.Gold, goldSprite },
            { ShopItemConfig.CurrencyType.Diamonds, diamondSprite }
        };
    }

    // Method to get the sprite for a given currency type
    private Sprite GetSprite(ShopItemConfig.CurrencyType currencyType)
    {
        if (currencySprites.TryGetValue(currencyType, out var sprite))
        {
            return sprite;
        }
        return null;
    }

    // Method to get the ShopItemConfig and corresponding sprite
    public (ShopItemConfig, Sprite) GetShopItemConfigAndSprite()
    {
        if (index < 0 || index >= shopItems.Count)
        {
            return (null, null);
        }
        var config = shopItems[index];
        var sprite = GetSprite(config.currencyType);
        index++; // Increment the index to get the next item in the next call
        return (config, sprite);
    }
}