using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turn : MonoBehaviour {
    TurnManager _manager;
    bool _isInitialized = false;
    public Type Type;

    public int TurnNumber {
        get => _manager.TurnNumber;
    }

    public void Initialize(TurnManager manager) {
        _manager = manager;
        Type = GetType();
        _isInitialized = true;  
    }

    public virtual void StartTurn() {
        if (!_isInitialized) throw new Exception("TurnManager is not initialized");
        if (_manager == null) throw new Exception("Turn manager is null");
    }
    
    public virtual void EndTurn() {
        if (!_isInitialized) throw new Exception("TurnManager is not initialized");
        if (_manager == null) throw new Exception("Turn manager is null");
    }
}