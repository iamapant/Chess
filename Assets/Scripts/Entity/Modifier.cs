using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Modifier : MonoBehaviour {
    public Func<Entity, bool> Condition = null;
    protected Entity Entity;
    protected bool IsInitialized = false;
    protected int InitializedTurnNumber;
    protected Turn InitializedTurn;

    public void Initialize(Entity entity) {
        this.Entity = entity;
        IsInitialized = true;
        InitializedTurn = GameController.Instance.TurnManager.CurrentTurn;
        InitializedTurnNumber = GameController.Instance.TurnManager.TurnNumber;

        OnInitialize();
    }

    private void OnEnable() { GameController.Instance.TurnManager.UpdateTurn += OnUpdateTurn; }

    private void OnDisable() { GameController.Instance.TurnManager.UpdateTurn -= OnUpdateTurn; }

    protected virtual void OnUpdateTurn(Turn turn) { }

    protected abstract void OnInitialize();
    protected abstract void OnDestroy();

    public void OnRegistered<T>(T visitable) where T : IVisitable {
        if (!IsInitialized)
            throw new Exception("This modifier has not been initialized.");

        var entity = visitable as Entity;
        var square = visitable as Square;

        if (entity != null) { OnRegisteredAsEntity(entity); }

        if (square != null) { OnRegisteredAsSquare(square); }
    }

    public void OnDeregistered<T>(T visitable) where T : IVisitable {
        if (!IsInitialized)
            throw new Exception("This modifier has not been initialized.");

        var entity = visitable as Entity;
        var square = visitable as Square;

        if (entity != null) { OnDeregisteredAsEntity(entity); }

        if (square != null) { OnDeregisteredAsSquare(square); }
    }

    protected virtual void OnDeregisteredAsSquare(Square square) { }

    protected virtual void OnDeregisteredAsEntity(Entity entity) { }

    protected virtual void OnRegisteredAsEntity(Entity entity) { }

    protected virtual void OnRegisteredAsSquare(Square square) { }
}