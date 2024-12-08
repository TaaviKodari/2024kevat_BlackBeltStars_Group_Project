using System;
using System.Collections.Generic;
using AtomicConsole;

namespace GameState
{
    [Serializable]
    public struct SaveGame
    {
        [NonSerialized]
        public string SaveName;
        public string name;
        public Resources resources;
        public Maps maps;
        public Inventory inventory;
        public List<ShopItemConfig> shopItems;
    }

    [Serializable]
    public struct Resources
    {
        public int gold;
        public int diamonds;
    }
    
    [Serializable]
    public struct HealthBoost
    {
        public float multiplier;
        public int duration;
    }
    
    [Serializable]
    public struct SpeedBoost
    {
        public float multiplier;
        public int duration;
    }
    
    [Serializable]
    public struct DamageBoost
    {
        public int fixedAmount;
        public float multiplier;
        public int duration;
    }
    
    [Serializable]
    public struct Inventory
    {
        public List<SpeedBoost> speedBoosts;
        public List<HealthBoost> healthBoosts;
        public List<DamageBoost> damageBoosts;
    }
}