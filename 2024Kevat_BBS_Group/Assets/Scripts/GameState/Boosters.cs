using System;
using Attributes;
using UnityEngine;
using Attribute = Attributes.Attribute;

namespace GameState
{
    [Serializable]
    public struct BoosterInstance
    {
        [SerializeReference, SubclassSelector]
        public IBooster booster;
        public int gamesLeft;

        public BoosterInstance(IBooster booster)
        {
            this.booster = booster;
            gamesLeft = booster.Duration;
        }
    }

    public interface IBooster
    {
        int Duration { get; }
    }

    [Serializable]
    public struct AttributeBooster : IBooster
    {
        [AttributeRef]
        public string attribute;
        public AttributeModifierType type;
        public float amount;
        public int duration;
        public int Duration => duration;

        public AttributeModifier CreateModifier(string tag)
        {
            return new AttributeModifier
            {
                Attribute = Attribute.Find(attribute),
                Type = type,
                Amount = amount,
                Tag = tag
            };
        }
    }
}