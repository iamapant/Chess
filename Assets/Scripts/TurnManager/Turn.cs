using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turn {
    protected TurnManager _manager;

    public Turn(TurnManager manager) {
        _manager = manager;
    }

    public virtual void Initialize() {}
    
    public virtual void Update(){}
    
    public int TurnNumber {
        get => _manager.TurnNumber;
    }

    public abstract void StartTurn();

    public abstract void EndTurn();
}

public class FactionTurn : Turn {
    public Faction Faction { get; init; }

    public FactionTurn(Faction faction, TurnManager manager) : base(manager) {
        Faction = faction;
    }

    public override void StartTurn() { throw new NotImplementedException(); }
    public override void EndTurn() { throw new NotImplementedException(); }
}