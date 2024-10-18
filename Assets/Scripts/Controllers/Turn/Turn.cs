using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Turn {
    protected TurnManager _manager;
    
    public virtual void Initialize(TurnManager manager) => _manager = manager;
    
    public virtual void Update(){}

    public abstract void StartTurn();

    public abstract void EndTurn();
}

[Serializable]
public class FactionTurn : Turn {
    [SerializeField] public Faction Faction;

    public override void StartTurn() { throw new NotImplementedException(); }
    public override void EndTurn() { throw new NotImplementedException(); }
}