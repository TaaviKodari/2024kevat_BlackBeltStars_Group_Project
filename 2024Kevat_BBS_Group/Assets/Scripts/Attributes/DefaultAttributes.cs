using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Attributes
{
    [Serializable]
    public class DefaultAttributes : ISerializationCallbackReceiver
    {
        public readonly Dictionary<Attribute, float> Attributes = new();

        [SerializeField]
        private List<SerializedEntry> serializedEntries;

        [Serializable]
        private struct SerializedEntry
        {
            public string attribute;
            public float value;
        }

        public void OnBeforeSerialize()
        {
            serializedEntries = Attributes.Select(entry => new SerializedEntry
            {
                attribute = entry.Key.Id,
                value = entry.Value
            }).ToList();
        }

        public void OnAfterDeserialize()
        {
            Attributes.Clear();
            if (serializedEntries != null)
            {
                foreach (var entry in serializedEntries)
                {
                    Attributes[Attribute.Find(entry.attribute)] = entry.value;
                }
            }
            serializedEntries = null;
        }

        // Add new attributes here
        public void AddAttribute(Attribute attribute, float value)
        {
            Attributes[attribute] = value;
        }
    }
}