using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    [Serializable]
    public struct Maps
    {
        public MapStats map1;
        public MapStats map2;
        public MapStats map3;
    }
    
    [Serializable]
    public struct MapStats
    {
        [SerializeReference]
        public List<IMapModifier> modifiers;
    }
    
    public interface IMapModifier
    {
    }

    [Serializable]
    public class ObstacleCountMapModifier : IMapModifier
    {
        public int obstacleType;
        public float factor;
    }

    [Serializable]
    public class GoldAmountMapModifier : IMapModifier
    {
        public float factor;
    }
}