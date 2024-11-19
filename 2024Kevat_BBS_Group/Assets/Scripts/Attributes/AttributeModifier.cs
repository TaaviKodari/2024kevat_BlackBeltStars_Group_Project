namespace Attributes
{
    public struct AttributeModifier
    {
        public string Tag;
        public Attribute Attribute;
        public AttributeModifierType Type;
        public float Amount;
    }

    public enum AttributeModifierType
    {
        Add, Multiply
    }
}