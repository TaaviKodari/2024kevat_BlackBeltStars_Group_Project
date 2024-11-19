using System;
using System.Collections.Generic;

namespace Attributes
{
    public class AttributeHolder
    {
        private readonly IDictionary<Attribute, float> baseAttributes;
        private readonly Dictionary<Attribute, List<AttributeModifier>> modifiers = new();

        public AttributeHolder(IDictionary<Attribute, float> baseAttributes)
        {
            this.baseAttributes = baseAttributes;
        }

        public float GetValue(Attribute attribute)
        {
            if (!baseAttributes.TryGetValue(attribute, out var value))
            {
                throw new InvalidOperationException($"Attribute {attribute.Id} is not present in holder");
            }

            foreach (var modifier in modifiers.GetValueOrDefault(attribute, new List<AttributeModifier>()))
            {
                switch (modifier.Type)
                {
                    case AttributeModifierType.Add:
                        value += modifier.Amount;
                        break;
                    case AttributeModifierType.Multiply:
                        value *= modifier.Amount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return value;
        }

        public void AddModifier(AttributeModifier modifier)
        {
            var attribute = modifier.Attribute;
            if (!baseAttributes.ContainsKey(attribute))
            {
                throw new InvalidOperationException($"Attribute {attribute.Id} is not present in holder");
            }

            if (!modifiers.ContainsKey(attribute))
            {
                modifiers[attribute] = new List<AttributeModifier>();
            }

            var modifierList = modifiers[attribute];
            modifierList.RemoveAll(oldModifier => oldModifier.Tag == modifier.Tag);
            modifierList.Add(modifier);
        }

        public void RemoveModifier(Attribute attribute, string tag)
        {
            if (!baseAttributes.ContainsKey(attribute))
            {
                throw new InvalidOperationException($"Attribute {attribute.Id} is not present in holder");
            }

            if (!modifiers.TryGetValue(attribute, out var modifierList)) return;
            modifierList.RemoveAll(oldModifier => oldModifier.Tag == tag);
        }
    }
}