using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
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
        [SerializeReference, SubclassSelector, CanBeNull]
        public List<IMapModifier> modifiers;
        [SerializeReference, SubclassSelector]
        public IMapGoal goal;
        public int seed;
    }
    
    public interface IMapModifier
    {
        void Describe(StringBuilder builder);
    }

    [Serializable]
    public class ObstacleCountMapModifier : IMapModifier
    {
        public string obstacleType;
        public float factor;

        public ObstacleCountMapModifier(string obstacleType, float factor)
        {
            this.obstacleType = obstacleType;
            this.factor = factor;
        }

        public void Describe(StringBuilder builder)
        {
            var color = factor > 1 ? "468232" : "a53030";
            var name = VariantManager.Instance.TerrainObstacles[obstacleType].name;
            builder.AppendFormat("<color=#{0}>{1:0.##}x</color> {2} Count", color, factor, name);
        }
    }

    [Serializable]
    public class GoldAmountMapModifier : IMapModifier
    {
        public float factor;

        public GoldAmountMapModifier(float factor)
        {
            this.factor = factor;
        }

        public void Describe(StringBuilder builder)
        {
            var color = factor > 1 ? "468232" : "a53030";
            builder.AppendFormat("<color=#{0}>{1:0.##}x</color> Gold", color, factor);
        }
    }

    public interface IMapGoal
    {
        string Describe();
    }

    [Serializable]
    public class KillAntsMapGoal : IMapGoal
    {
        public int amount;

        public KillAntsMapGoal(int amount)
        {
            this.amount = amount;
        }

        public string Describe()
        {
            return $"Kill {amount} ants";
        }
    }

    [Serializable]
    public class SurviveWavesMapGoal : IMapGoal
    {
        public int amount;

        public SurviveWavesMapGoal(int amount)
        {
            this.amount = amount;
        }

        public string Describe()
        {
            return $"Survive {amount} waves";
        }
    }
}