using System;
using JetBrains.Annotations;
using UnityEngine;

public abstract class SquareRule : ScriptableObject {
    protected bool Equals(SquareRule other) {
        return base.Equals(other) && RuleName == other.RuleName && Equals(Square, other.Square);
    }

    public override bool Equals(object obj) {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SquareRule)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(base.GetHashCode(), RuleName, Square);
    }

    private string GetRuleNameOrDefault([CanBeNull] string name) => (string.IsNullOrEmpty(name) || name == "") ? GetType().Name : name; 

    [SerializeField] private string ruleName;

    public string RuleName {
        get => GetRuleNameOrDefault(ruleName);
        set => ruleName = value;
    }

    public Square Square;

    public Vector2Int[] Positions {
        get => CalculatePositions();
    }
    
    public Vector2Int BoardSize { get; set; }

    protected abstract Vector2Int[] CalculatePositions();

    public static bool operator ==(SquareRule rule1, SquareRule rule2) {
        if (rule1?.Positions == null) rule1 = null;
        if (rule2?.Positions == null) rule2 = null;
        return SquareRule.ReferenceEquals(rule1, rule2);
    }

    public static bool operator !=(SquareRule rule1, SquareRule rule2) => !(rule1 == rule2);

    public static implicit operator Vector2Int[](SquareRule rule) => rule.Positions;
}