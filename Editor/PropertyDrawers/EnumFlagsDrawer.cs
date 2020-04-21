﻿using System;
using UnityEngine;
using UnityEditor;

namespace KazegamesKit.Editor
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnumFlagsAttribute targetAttribute = (EnumFlagsAttribute)attribute;

            Enum targetEnum = GetBaseProperty<Enum>(property);

            var propName = targetAttribute.enumName;
            if (string.IsNullOrEmpty(propName))
                propName = property.name;

            EditorGUI.BeginProperty(position, label, property);
            Enum enumNew = EditorGUI.EnumFlagsField(position, propName, targetEnum);
            property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
            EditorGUI.EndProperty();
        }

        static T GetBaseProperty<T>(SerializedProperty sp)
        {
            var separatedPaths = sp.propertyPath.Split('.');

            var reflectionTarget = sp.serializedObject.targetObject as object;

            foreach (var path in separatedPaths)
            {
                var fieldInfo = reflectionTarget.GetType().GetField(path);
                reflectionTarget = fieldInfo.GetValue(reflectionTarget);
            }
            return (T)reflectionTarget;
        }
    }
}
