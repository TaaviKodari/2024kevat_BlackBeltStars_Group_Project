using System;
using System.Collections.Generic;

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
        public List<BoosterInstance> boosters;
        public List<ShopItemConfig> shopItems;
        public List<bool> boughtItems;
    }

    [Serializable]
    public struct Resources
    {
        public int gold;
        public int diamonds;
    }
}