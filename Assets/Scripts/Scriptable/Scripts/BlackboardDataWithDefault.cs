using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Scriptable.Scripts {
    public abstract class BlackboardDataWithDefault : BlackboardData {
        protected Dictionary<string, AnyValue> defaultValues = new ();

        private void Awake() {
            GenerateDefaultValues();
        }

        private void Reset() {
            GenerateDefaultValues();
        }

        private void OnEnable() {
            GenerateDefaultValues();
        }

        private void OnValidate() {
            GenerateDefaultValues();
        }

        private  void GenerateDefaultValues() {
            OnGenerateDefaultValues();
            foreach (var defaults in defaultValues) {
                if (entries.All(e => e.keyName != defaults.Key)) {
                    Debug.Log($"Add {defaults.Key} as default values for type {GetType()}"); 
                    var entry = new BlackboardEntryData();
                    entry.keyName = defaults.Key;
                    entry.valueType = defaults.Value.type;
                    entry.value = defaults.Value;

                    entries.Add(entry);
                }
            }
        }

        protected abstract void OnGenerateDefaultValues();

        AnyValue.ValueType ToValueType(Type type) {
            switch(type) {
                case not null when type == typeof(bool): return AnyValue.ValueType.Bool;
                case not null when type == typeof(int): return AnyValue.ValueType.Int;
                case not null when type == typeof(float): return AnyValue.ValueType.Float;
                case not null when type == typeof(string): return AnyValue.ValueType.String;
                case not null when type == typeof(Sprite): return AnyValue.ValueType.Sprite;
                case not null when type == typeof(Vector2): return AnyValue.ValueType.Vector2;
                case not null when type == typeof(Vector2Int): return AnyValue.ValueType.Vector2Int;
                case not null when type == typeof(Vector3): return AnyValue.ValueType.Vector3;
                case not null when type == typeof(Vector3Int): return AnyValue.ValueType.Vector3Int;
                default: return AnyValue.ValueType.Bool;
            }
        }
    }
}