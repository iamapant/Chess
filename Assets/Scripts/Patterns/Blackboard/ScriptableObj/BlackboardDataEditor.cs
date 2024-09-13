﻿using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(BlackboardData))]
public class BlackboardDataEditor : Editor {
    ReorderableList entryList;

    void OnEnable() {
        entryList = new ReorderableList(serializedObject, serializedObject.FindProperty("entries"), true, true, true,
            true) {
            drawHeaderCallback = rect => {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight),
                    "Key");
                EditorGUI.LabelField(
                    new Rect(rect.x + rect.width * 0.3f + 10, rect.y, rect.width * 0.3f,
                        EditorGUIUtility.singleLineHeight), "Type");
                EditorGUI.LabelField(
                    new Rect(rect.x + rect.width * 0.6f + 5, rect.y, rect.width * 0.4f,
                        EditorGUIUtility.singleLineHeight), "Value");
            }
        };

        entryList.drawElementCallback = (rect, index, isActive, isFocused) => {
            var element = entryList.serializedProperty.GetArrayElementAtIndex(index);

            rect.y += 2;
            var keyName = element.FindPropertyRelative("keyName");
            var valueType = element.FindPropertyRelative("valueType");
            var value = element.FindPropertyRelative("value");

            var keyNameRect = new Rect(rect.x, rect.y, rect.width * 0.28f, EditorGUIUtility.singleLineHeight);
            var valueTypeRect = new Rect(rect.x + rect.width * 0.3f, rect.y, rect.width * 0.28f,
                EditorGUIUtility.singleLineHeight);
            var valueRect = new Rect(rect.x + rect.width * 0.6f, rect.y, rect.width * 0.4f,
                EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(keyNameRect, keyName, GUIContent.none);
            EditorGUI.PropertyField(valueTypeRect, valueType, GUIContent.none);

            switch ((AnyValue.ValueType)valueType.enumValueIndex) {
                case AnyValue.ValueType.Int:
                    var intValue = value.FindPropertyRelative("intValue");
                    EditorGUI.PropertyField(valueRect, intValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Float:
                    var floatValue = value.FindPropertyRelative("floatValue");
                    EditorGUI.PropertyField(valueRect, floatValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Bool:
                    var boolValue = value.FindPropertyRelative("boolValue");
                    EditorGUI.PropertyField(valueRect, boolValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.String:
                    var strValue = value.FindPropertyRelative("stringValue");
                    EditorGUI.PropertyField(valueRect, strValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Vector2:
                    var vector2Value = value.FindPropertyRelative("vector2Value");
                    EditorGUI.PropertyField(valueRect, vector2Value, GUIContent.none);
                    break;
                case AnyValue.ValueType.Vector2Int:
                    var vector2IntValue = value.FindPropertyRelative("vector2IntValue");
                    EditorGUI.PropertyField(valueRect, vector2IntValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Vector3:
                    var vector3Value = value.FindPropertyRelative("vector3Value");
                    EditorGUI.PropertyField(valueRect, vector3Value, GUIContent.none);
                    break;
                case AnyValue.ValueType.Vector3Int:
                    var vector3IntValue = value.FindPropertyRelative("vector3IntValue");
                    EditorGUI.PropertyField(valueRect, vector3IntValue, GUIContent.none);
                    break;
                case AnyValue.ValueType.Sprite:
                    var spriteValue = value.FindPropertyRelative("spriteValue");
                    EditorGUI.PropertyField(valueRect, spriteValue, GUIContent.none);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        };
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        entryList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}