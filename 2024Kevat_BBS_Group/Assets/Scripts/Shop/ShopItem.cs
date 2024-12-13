using GameState;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    private ShopManager shopManager;
    [SerializeField]
    private GameStateManager gameStateManager;

    [SerializeField]
    private Image itemIconImage;
    [SerializeField]
    private TMP_Text itemNameText;
    [SerializeField]
    private TMP_Text itemPriceText;
    [SerializeField]
    private TMP_Text itemDescriptionText;
    [SerializeField]
    private Image currencyImage;
    [SerializeField]
    private Button itemButton;
    private ShopItemConfig itemConfig;

    private bool buttonEnabled;
    private int indexToDisable;

    // Method to initialize the shop item with the given config
    public void Initialize(ShopItemConfig config, Sprite currencySprite, bool buttonEnabled, int indexToDisable)
    {
        this.buttonEnabled = buttonEnabled;
        this.indexToDisable = indexToDisable;
        UpdateItemInformation(config, currencySprite);

        // Clear existing listeners and assign the button click event
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(OnItemButtonClick);
    }

    public void UpdateItemInformation(ShopItemConfig config, Sprite currencySprite)
    {
        itemConfig = config;
        itemIconImage.sprite = config.booster.Sprite;
        itemNameText.text = itemConfig.itemName;
        itemPriceText.text = itemConfig.cost.ToString();
        itemDescriptionText.text = itemConfig.itemDescription;
        currencyImage.sprite = currencySprite;
        itemButton.interactable = buttonEnabled;
    }

    private bool CanAfford(int itemCost, ShopItemConfig.CurrencyType currencyType)
    {
        return currencyType switch
        {
            ShopItemConfig.CurrencyType.Gold => gameStateManager.currentSaveGame.resources.gold >= itemCost,
            ShopItemConfig.CurrencyType.Diamonds => gameStateManager.currentSaveGame.resources.diamonds >= itemCost,
            _ => false
        };
    }

    private void OnItemButtonClick()
    {
        if (gameStateManager == null) return;

        if (CanAfford(itemConfig.cost, itemConfig.currency))
        {
            switch (itemConfig.currency)
            {
                case ShopItemConfig.CurrencyType.Gold:
                    gameStateManager.currentSaveGame.resources.gold -= itemConfig.cost;
                    break;
                case ShopItemConfig.CurrencyType.Diamonds:
                    gameStateManager.currentSaveGame.resources.diamonds -= itemConfig.cost;
                    break;
            }
            shopManager.UpdateCurrencyTextFields();
            gameStateManager.currentSaveGame.boughtItems[indexToDisable] = true;
            ApplyItemEffects();
            DarkenItem();
            gameStateManager.Save();
        }
        else
        {
            Debug.Log("Not enough currency to purchase item.");
        }
    }

    private void ApplyItemEffects()
    {
        gameStateManager.currentSaveGame.boosters.Add(new BoosterInstance(itemConfig.booster));
    }

    private void DarkenItem()
    {
        itemButton.interactable = false;
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