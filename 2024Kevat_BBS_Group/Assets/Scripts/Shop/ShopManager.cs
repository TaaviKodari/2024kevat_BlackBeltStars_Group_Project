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

    // List of predetermined spawn points
    [SerializeField]
    private List<Transform> spawnPoints;

    // Reference to the parent transform where shop items will be instantiated
    [SerializeField]
    private Transform shopItemParent;

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
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (i >= spawnPoints.Count)
            {
                Debug.LogWarning("No more spawn points available for shop items.");
                break;
            }

            // Instantiate the shop item prefab at the predetermined spawn point
            var shopItemObject = Instantiate(shopItemPrefab, spawnPoints[i].position, spawnPoints[i].rotation, shopItemParent);

            // Get the ShopItem component and initialize it
            var shopItem = shopItemObject.GetComponent<ShopItem>();
            var currencySprite = GetSprite(shopItems[i].currencyType);
            shopItem.Initialize(shopItems[i], currencySprite);
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