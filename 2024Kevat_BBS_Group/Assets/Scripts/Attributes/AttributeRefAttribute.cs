using System;
using UnityEngine;

namespace Attributes
{
    // This attribute enables a custom drawer for an attribute selector dropdown
    [AttributeUsage(AttributeTargets.Field)]
    public class AttributeRefAttribute : PropertyAttribute
    {
    }
}