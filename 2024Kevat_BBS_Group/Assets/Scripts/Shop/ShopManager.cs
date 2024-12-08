using System.Collections;
using System.Collections.Generic;
using System;
using GameState;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class ShopManager : MonoBehaviour
{
    private GameStateManager gameStateManager;
    [SerializeField]
    private SceneTransition transition;

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

    // Random instance for generating random numbers
    private Random random;

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
        var selectedItems = new List<ShopItemConfig>();
        var possibleItems = new List<ShopItemConfig>(shopItems);
        random = new Random();

        for (int i = 0; i < 3; i++)
        {
            int randomNum = random.Next(0, possibleItems.Count);
            Debug.Log("Random Number: " + randomNum);
            selectedItems.Add(possibleItems[randomNum]);
            possibleItems.RemoveAt(randomNum);
        }

        foreach (var item in selectedItems)
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
}