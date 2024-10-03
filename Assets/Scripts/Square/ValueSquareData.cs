using UnityEngine;

[CreateAssetMenu(fileName = "SquareData", menuName = "Data/SquareData")]
public class ValueSquareData : ScriptableObject, ISquareData {
    
    [SerializeField] protected float _goldPerTurn;
    [SerializeField] protected float _goldMultiplier;
    [SerializeField] protected float _faithPerTurn;
    [SerializeField] protected float _faithMultiplier;
    [SerializeField] protected float _expPerTurn;
    [SerializeField] protected float _maxExp;
    [SerializeField] protected float _stability;
    [SerializeField] protected float _maxStability;
    [SerializeField] protected float _minStability;
    [SerializeField] protected float _stabilityPerTurn;
    public float GoldPerTurn { get => _goldPerTurn; } 
    public float GoldMultiplier {get => _goldMultiplier; }
    public float FaithPerTurn {get => _faithPerTurn; }
    public float FaithMultiplier {get => _faithMultiplier; }
    public float ExpPerTurn {get => _expPerTurn; }
    public float MaxExp {get => _maxExp; }
    public float Stability {get => _stability; }
    public float MaxStability {get => _maxStability; }
    public float MinStability {get => _minStability; }
    public float StabilityPerTurn {get => _stabilityPerTurn; }
}