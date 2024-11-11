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
        public Resources resources;
        public Maps maps;
        public Inventory inventory;
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
    public struct Inventory
    {
        public List<SpeedBoost> speedBoosts;
        public List<HealthBoost> healthBoosts;
    }
}