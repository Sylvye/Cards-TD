using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(Stat))]
public class StatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the global property label and update the remaining position.
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Spacing between columns.
        float spacing = 5f;
        float totalWidth = position.width;

        // Define custom ratios for each column:
        float valueRatio = 0.5f;
        float minRatio = 0.25f;
        float maxRatio = 0.25f;
        float totalSpacing = spacing * 2;
        float valueColWidth = (totalWidth - totalSpacing) * valueRatio;
        float minColWidth = (totalWidth - totalSpacing) * minRatio;
        float maxColWidth = (totalWidth - totalSpacing) * maxRatio;

        // Define column rectangles.
        Rect valueCol = new Rect(position.x, position.y, valueColWidth, EditorGUIUtility.singleLineHeight);
        Rect minCol = new Rect(valueCol.xMax + spacing, position.y, minColWidth, EditorGUIUtility.singleLineHeight);
        Rect maxCol = new Rect(minCol.xMax + spacing, position.y, maxColWidth, EditorGUIUtility.singleLineHeight);

        // Get the serialized properties.
        SerializedProperty valueProp = property.FindPropertyRelative("value");
        SerializedProperty minProp = property.FindPropertyRelative("min");
        SerializedProperty maxProp = property.FindPropertyRelative("max");

        // --- Value Column ---
        // Reserve a fixed width for the "Value" label.
        float valueLabelWidth = 40f;
        Rect valueLabelRect = new Rect(valueCol.x, valueCol.y, valueLabelWidth, valueCol.height);
        Rect valueFieldRect = new Rect(valueLabelRect.xMax, valueCol.y, valueCol.width - valueLabelWidth, valueCol.height);
        EditorGUI.LabelField(valueLabelRect, "Value");
        EditorGUI.PropertyField(valueFieldRect, valueProp, GUIContent.none);

        // --- Min Column ---
        // Reserve a smaller width for the "Min" label so that more space goes to the text field.
        float minLabelWidth = 30f;
        Rect minLabelRect = new Rect(minCol.x, minCol.y, minLabelWidth, minCol.height);
        Rect minFieldRect = new Rect(minLabelRect.xMax, minCol.y, minCol.width - minLabelWidth, minCol.height);
        EditorGUI.LabelField(minLabelRect, "Min");
        minProp.stringValue = EditorGUI.TextField(minFieldRect, minProp.stringValue);

        // --- Max Column ---
        // Do the same for the "Max" field.
        float maxLabelWidth = 30f;
        Rect maxLabelRect = new Rect(maxCol.x, maxCol.y, maxLabelWidth, maxCol.height);
        Rect maxFieldRect = new Rect(maxLabelRect.xMax, maxCol.y, maxCol.width - maxLabelWidth, maxCol.height);
        EditorGUI.LabelField(maxLabelRect, "Max");
        maxProp.stringValue = EditorGUI.TextField(maxFieldRect, maxProp.stringValue);

        EditorGUI.EndProperty();
    }
}
