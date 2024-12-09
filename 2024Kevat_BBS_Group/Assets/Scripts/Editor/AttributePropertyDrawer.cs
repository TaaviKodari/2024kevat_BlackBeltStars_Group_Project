using System.Globalization;
using System.Linq;
using Attributes;
using UnityEditor;
using UnityEngine;
using Attribute = Attributes.Attribute;

[CustomPropertyDrawer(typeof(AttributeRefAttribute))]
public class AttributePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attributes = Attribute.All.ToList();
        var index = attributes.IndexOf(Attribute.All.FirstOrDefault(it => it.Id == property.stringValue));

        EditorGUI.BeginProperty(position, label, property);
        var selected = EditorGUI.Popup(position, label, index, attributes.Select(it => new GUIContent(CleanName(it.Id))).ToArray());

        if (selected == -1) selected = 0;
        property.stringValue = attributes[selected].Id;
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }

    private string CleanName(string name)
    {
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.Replace('_', ' '));
    }
}

