using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// IngredientDrawerUIE
[CustomPropertyDrawer(typeof(Stat))]
public class StatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Optionally, draw a label for the entire property.
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float padding = 5f;
        float spacing = 5f;
        float totalWidth = position.width - padding * 2;

        // Distribute space between the fields. Adjust percentages as needed.
        float valueWidth = totalWidth * 0.4f;
        float minWidth = totalWidth * 0.3f;
        float maxWidth = totalWidth * 0.3f;

        // Define rectangles for each field.
        Rect valueRect = new Rect(position.x + padding, position.y, valueWidth - spacing, EditorGUIUtility.singleLineHeight);
        Rect minRect = new Rect(valueRect.xMax + spacing, position.y, minWidth - spacing, EditorGUIUtility.singleLineHeight);
        Rect maxRect = new Rect(minRect.xMax + spacing, position.y, maxWidth, EditorGUIUtility.singleLineHeight);

        // Draw the fields. Use the exact names from your Stat struct.
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);
        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
        EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
