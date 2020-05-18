using System;
using UnityEngine;
using UnityEditor;

namespace KazegamesKit.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxFloat))]
    public class MinMaxFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect labMinRect = new Rect(position.x, position.y, 30, position.height);
            Rect minRect = new Rect(position.x + 35, position.y, 60, position.height);
            Rect labMaxRect = new Rect(position.x + 105, position.y, 30, position.height);
            Rect maxRect = new Rect(position.x + 140, position.y, 60, position.height);

            // Draw fields
            EditorGUI.LabelField(labMinRect, new GUIContent("Min"));
            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
            EditorGUI.LabelField(labMaxRect, new GUIContent("Max"));
            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}