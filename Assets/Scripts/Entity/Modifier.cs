using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Modifier : MonoBehaviour {
    public virtual Func<Entity, bool> Condition {
        get => entity => entity != Entity;
    }

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

    private void OnEnable() {
        if (GameController.Instance?.TurnManager != null)
            GameController.Instance.TurnManager.UpdateTurn += OnUpdateTurn;
    }

    private void OnDisable() {
        if (GameController.Instance?.TurnManager != null)
            GameController.Instance.TurnManager.UpdateTurn -= OnUpdateTurn;
    }

    protected virtual void OnUpdateTurn(Turn turn) { }

    protected abstract void OnInitialize();
    protected abstract void OnDestroy();

    public virtual void Registered(IVisitable visitable) {
        if (!IsInitialized)
            throw new Exception("This modifier has not been initialized.");
        if (visitable is Entity entity) OnRegisteredAsEntity(entity);
        if (visitable is Square square) OnRegisteredAsSquare(square);
    }

    public virtual void Deregistered(IVisitable visitable) {
        if (!IsInitialized)
            throw new Exception("This modifier has not been initialized.");

        if (visitable is Entity entity && entity != null) OnDeregisteredAsEntity(entity);
        if (visitable is Square square && square != null) OnDeregisteredAsSquare(square);
    }

    protected abstract void OnDeregisteredAsEntity(Entity entity);
    protected abstract void OnDeregisteredAsSquare(Square square);
    protected abstract void OnRegisteredAsEntity(Entity entity);
    protected abstract void OnRegisteredAsSquare(Square square);
}