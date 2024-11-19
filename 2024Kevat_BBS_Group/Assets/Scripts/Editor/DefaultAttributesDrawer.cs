using System.Linq;
using Attributes;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DefaultAttributes))]
public class DefaultAttributesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var defaultAttributes = (DefaultAttributes)property.boxedValue;

        EditorGUI.BeginProperty(position, label, property);

        property.isExpanded = EditorGUI.Foldout(new Rect(position)
        {
            height = EditorGUIUtility.singleLineHeight
        }, property.isExpanded, new GUIContent(property.displayName), true);

        EditorGUI.indentLevel++;

        if (property.isExpanded)
        {
            foreach (var attribute in Attribute.All)
            {
                position = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight);
                var togglePos = new Rect(position.x, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
                var fieldPos = new Rect(position.x + position.width / 2, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);

                var has = defaultAttributes.Attributes.ContainsKey(attribute);
                var newHas = EditorGUI.Toggle(togglePos, new GUIContent(attribute.Id), has);

                if (newHas && !has) defaultAttributes.Attributes[attribute] = 0;
                if (!newHas && has) defaultAttributes.Attributes.Remove(attribute);

                if (newHas)
                {
                    var newValue = EditorGUI.FloatField(fieldPos, defaultAttributes.Attributes[attribute]);
                    defaultAttributes.Attributes[attribute] = newValue;
                }
            }
        }

        EditorGUI.indentLevel--;

        property.boxedValue = defaultAttributes;
        property.serializedObject.ApplyModifiedProperties();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? (1 + Attribute.All.Count()) * EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight;
    }
}