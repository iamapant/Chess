using System;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Square Stats", menuName = "Blackboard/Square Stats")]
public class SquareStats : BlackboardData {
    // public IRenderAdapter RenderAdapter;
    // public int GoldPerTurn;
    // public float GoldModifier = 1f;
    // public float FaithPerTurn;
    // public float FaithMultiplier = 1f;
    // public float ExpPerTurn;
    // public float MaxExp;
    // public float Stability;
    // public float MaxStability;
    // public float MinStability;
    // public float StabilityPerTurn;
    private void OnEnable() {
        var list = Enum.GetValues(typeof(StatTypes)).Cast<StatTypes>().ToList();
        foreach (var statType in list) {
            if (entries.All(e => e.keyName != statType.ToString())) {
                var entry = new BlackboardEntryData();
                entry.keyName = statType.ToString();
                entry.valueType = AnyValue.ValueType.Bool;
                entry.value = new AnyValue() { type = AnyValue.ValueType.Bool, boolValue = false };

                entries.Add(entry);
            }
        }
    }

    public enum StatTypes {
        GoldPerTurn,
        GoldModifier,
        FaithPerTurn,
        FaithMultiplier,
        ExpPerTurn,
        MaxExp,
        Stability,
        MaxStability,
        MinStability,
        StabilityPerTurn,
    }
}