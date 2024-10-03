using System;


public interface ISquareData {
    float GoldPerTurn { get; } 
    float GoldMultiplier {get; }
    float FaithPerTurn {get; }
    float FaithMultiplier {get; }
    float ExpPerTurn {get; }
    float MaxExp {get; }
    float Stability {get; }
    float MaxStability {get; }
    float MinStability {get; }
    float StabilityPerTurn {get; }
    

}