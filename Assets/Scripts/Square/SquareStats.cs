using Scriptable.Scripts;
using UnityEngine;


[CreateAssetMenu(fileName = "Square Stats", menuName = "Blackboard/Square Stats")]
public class SquareStats : BlackboardDataWithDefault {
    protected override void OnGenerateDefaultValues() {
        defaultValues = new() {
            { "GoldPerTurn", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 3 } },
            { "GoldModifier", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 1 } },
            { "FaithPerTurn", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 3 } },
            { "FaithMultiplier", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 1 } },
            { "ExpPerTurn", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 5 } },
            { "MaxExp", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 100 } },
            { "Stability", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 20 } },
            { "MaxStability", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 100 } },
            { "MinStability", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 1 } },
            { "StabilityPerTurn", new AnyValue() { type = AnyValue.ValueType.Float, floatValue = 2 } }
        };
    }
    
    
}