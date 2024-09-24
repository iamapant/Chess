using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IVisitable {
    protected Square Square;

    protected EntityMovementManager Movement;

    void Update() {
        Movement?.FixedUpdate(this);
    }

    public void SetMovement(EntityMovementManager movement) {
        Movement = movement;
    }

    private void OnEnable() { GameController.Instance.TurnManager.UpdateTurn += OnUpdateTurn; }

    private void OnDisable() { GameController.Instance.TurnManager.UpdateTurn -= OnUpdateTurn; }

    private void Start() {
        var square = GetComponentInParent<Square>();
    }

    public List<Modifier> Modifiers=> GetComponents<Modifier>().ToList(); 

    public void AddModifier<T>(T modifier) where T : Modifier {
        if (gameObject.TryGetComponent(typeof(Modifier), out var mod))
            RemoveModifier(mod as Modifier);

        gameObject.AddComponent(typeof(T));
        Modifiers.FirstOrDefault(e => e as T != null)?.Initialize(this);

        var payload = new EntityPayload();
        payload.Content += modifier.OnRegistered;
        Square.Mediator.Broadcast(this, payload, modifier.Condition);
    }

    public void RemoveModifier<T>(T modifier) where T : Modifier {
        var payload = new EntityPayload();
        payload.Content += modifier.OnDeregistered;
        Square.Mediator.Broadcast(this, payload, modifier.Condition);

        Destroy(modifier);
    }


    public void Accept(IVisitor visitor) {
        var payload = visitor as EntityPayload;
        payload?.Content(this);
    }
    
    public void MoveSquare(Square square) {
        if (!MovePrecondition(square)) return;
        
        this.Square?.Mediator.Deregister(this);
        this.Square = square;
        
        square.Mediator.Register(this);
    
        OnMove(square);
    }

    protected virtual bool MovePrecondition(Square newSquare) {
        if (newSquare == null) return false;

        return true;
    }

    public abstract void OnMove(Square square);

    void OnDestroy() => Square.Mediator.Deregister(this);

    public Dictionary<Func<Entity, bool>, EntityPayload> OnRegistered() {
        var dict = new Dictionary<Func<Entity, bool>, EntityPayload>();
        foreach (var modifier in Modifiers) {
            if (!dict.TryGetValue(modifier.Condition, out var payload)) {
                payload = new EntityPayload();
                dict.Add(modifier.Condition, payload);
            }

            payload.Content += modifier.OnRegistered;
        }

        return dict;
    }

    public Dictionary<Func<Entity, bool>, EntityPayload> OnDeregistered() {
        var dict = new Dictionary<Func<Entity, bool>, EntityPayload>();
        foreach (var modifier in Modifiers) {
            if (!dict.TryGetValue(modifier.Condition, out var payload)) {
                payload = new EntityPayload();
                dict.Add(modifier.Condition, payload);
            }

            payload.Content += modifier.OnDeregistered;
        }

        return dict;
    }

    protected virtual void OnUpdateTurn(Turn turn) { }
}