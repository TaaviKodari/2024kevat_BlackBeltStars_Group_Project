using System;
using System.Collections.Generic;
using System.Linq;

namespace Attributes
{
    [Serializable]
    public class Attribute
    {
        public static readonly Attribute Speed = new("speed");
        // Maximum health of the entity
        public static readonly Attribute MaxHealth = new("max_health");
        // Amount of damage the entity can inflict
        public static readonly Attribute AttackDamage = new("damage");
        public static readonly Attribute AttackCooldown = new("attack_speed");
        // Force applied to another entity when this one attacks (knockback effect)
        public static readonly Attribute Knockback = new("knockback");

        public static readonly IEnumerable<Attribute> All = new[] { Speed, MaxHealth, AttackDamage, AttackCooldown, Knockback };

        public readonly string Id;

        private Attribute(string id)
        {
            Id = id;
        }

        public static Attribute Find(string id)
        {
            return All.First(attribute => attribute.Id == id);
        }
    }
}