using System;
using System.Collections;
using System.Collections.Generic;
using GameState;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    private ShopManager shopManager;
    [SerializeField]
    private GameStateManager gameStateManager;

    [SerializeField]
    private TMPro.TMP_Text itemNameText;
    [SerializeField]
    private TMPro.TMP_Text itemPriceText;
    [SerializeField]
    private TMPro.TMP_Text itemDescriptionText;
    [SerializeField]
    private Image currencyImage;
    [SerializeField]
    private Button itemButton;

    private ShopItemConfig itemConfig;

    // Method to initialize the shop item with the given config
    public void Initialize(ShopItemConfig config, Sprite currencySprite)
    {
        UpdateItemInformation(config, currencySprite);

        // Clear existing listeners and assign the button click event
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(OnItemButtonClick);
    }

    public void UpdateItemInformation(ShopItemConfig config, Sprite currencySprite)
    {
        itemConfig = config;
        itemNameText.text = itemConfig.itemName;
        itemPriceText.text = itemConfig.cost.ToString();
        itemDescriptionText.text = itemConfig.description;
        currencyImage.sprite = currencySprite;
    }

    private bool CanAfford(int itemCost, ShopItemConfig.CurrencyType currencyType)
    {
        if (currencyType == ShopItemConfig.CurrencyType.Gold)
        {
            return gameStateManager.currentSaveGame.resources.gold >= itemCost;
        }
        else if (currencyType == ShopItemConfig.CurrencyType.Diamonds)
        {
            return gameStateManager.currentSaveGame.resources.diamonds >= itemCost;
        }
        return false;
    }

    private void OnItemButtonClick()
    {
        if (gameStateManager != null)
        {
            if (CanAfford(itemConfig.cost, itemConfig.currencyType))
            {
                if (itemConfig.currencyType == ShopItemConfig.CurrencyType.Gold)
                {
                    gameStateManager.currentSaveGame.resources.gold -= itemConfig.cost;
                }
                else if (itemConfig.currencyType == ShopItemConfig.CurrencyType.Diamonds)
                {
                    gameStateManager.currentSaveGame.resources.diamonds -= itemConfig.cost;
                }
                shopManager.UpdateCurrencyTextFields();
                ApplyItemEffects();
                gameStateManager.Save();
            }
            else
            {
                Debug.Log("Not enough currency to purchase item.");
                return;
            }
        }
    }

    private void ApplyItemEffects()
    {
        if (itemConfig.healthBoost.multiplier > 0 && itemConfig.healthBoost.duration > 0)
        {
            gameStateManager.currentSaveGame.inventory.healthBoosts.Add(itemConfig.healthBoost);
            Debug.Log("Added HealthBoost to inventory: " + itemConfig.healthBoost.multiplier + "x for " + itemConfig.healthBoost.duration + " games");
        }
        if (itemConfig.speedBoost.multiplier > 0 && itemConfig.speedBoost.duration > 0)
        {
            gameStateManager.currentSaveGame.inventory.speedBoosts.Add(itemConfig.speedBoost);
            Debug.Log("Added SpeedBoost to inventory: " + itemConfig.speedBoost.multiplier + "x for " + itemConfig.speedBoost.duration + " games");
        }
        if ((itemConfig.damageBoost.multiplier >= 1 || itemConfig.damageBoost.fixedAmount >= 1) && itemConfig.damageBoost.duration > 0)
        {
            gameStateManager.currentSaveGame.inventory.damageBoosts.Add(itemConfig.damageBoost);
            Debug.Log("Added DamageBoost to inventory: " + itemConfig.damageBoost.multiplier + "x & +" + itemConfig.damageBoost.fixedAmount + " dmg for " + itemConfig.damageBoost.duration + " games");
        }
    }

    private void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        gameStateManager = FindObjectOfType<GameStateManager>();
        if (gameStateManager == null)
        {
            Debug.LogError("GameStateManager not found in the scene.");
        }
    }
}