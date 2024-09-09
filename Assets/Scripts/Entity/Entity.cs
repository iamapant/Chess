using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Mediator;
using Mediator.EntityMediator;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public abstract class Entity : MonoBehaviour, IVisitable {
    protected Square Square;

    private void OnEnable() {
        GameController.Instance.TurnManager.UpdateTurn += OnUpdateTurn;
    }

    private void OnDisable() {
        GameController.Instance.TurnManager.UpdateTurn -= OnUpdateTurn;
    }

    private void Start() {
        var square = GetComponentInParent<Square>();
        if (square) Initialize(square);
        else throw new Exception("Entity cannot initialize because it doesn't belong to a square");
    }

    public List<Modifier> Modifiers {
        get { return GetComponents<Modifier>().ToList(); }
    }

    public void AddModifier<T>(T modifier) where T : Modifier {
        if (gameObject.TryGetComponent(typeof(Modifier), out var mod)) 
            RemoveModifier(mod as Modifier);

        gameObject.AddComponent(typeof(T));
        Modifiers.FirstOrDefault(e => e as T != null)?.Initialize(this);
        
        var payload = new EntityPayload();
        payload.Content += modifier.OnRegistered;
        Square.Broadcast(this, payload, modifier.Condition);
    }

    public void RemoveModifier<T>(T modifier) where T : Modifier {
        var payload = new EntityPayload();
        payload.Content += modifier.OnDeregistered;
        Square.Broadcast(this, payload, modifier.Condition);
        
        Destroy(modifier);
    }
    
    

    public void Accept(IVisitor visitor) {
        var payload = visitor as EntityPayload;
        payload?.Content(this);
    }

    public virtual void Initialize(Square square) {
        this.Square = square;

        square.Register(this);
    }

    // public void MoveSquare(Square square) {
    //     this.square.Deregister(this);
    //     this.square = square;
    //     
    //     square.Register(this);
    //
    //     OnMove(square);
    // }
    //
    // public abstract void OnMove(Square square);

    void OnDestroy() => Square.Deregister(this);

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