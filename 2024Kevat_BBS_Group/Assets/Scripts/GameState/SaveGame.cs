using System;

namespace GameState
{
    [Serializable]
    public struct SaveGame
    {
        [NonSerialized]
        public string SaveName;
        public Resources resources;
        public Maps maps;
    }

    [Serializable]
    public struct Resources
    {
        public int gold;
        public int diamonds;
    }
}