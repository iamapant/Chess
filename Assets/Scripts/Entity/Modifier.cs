using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Modifiable : MonoBehaviour, IVisitable {
    private void OnValidate() {
        if (gameObject.GetComponents<Modifiable>().Length > 1) {
            Debug.LogError($"Cannot have more than 1 controlling component attached to {gameObject.name}");
            // if (Application.isPlaying) Destroy(gameObject.GetComponent(this.GetType()));
            // else {
                UnityEditor.EditorApplication.delayCall += () => {
                    DestroyImmediate(gameObject.GetComponent(this.GetType()));
                };
            // }
        }
    }

    public abstract void Accept(IVisitor visitor);

    public abstract void AddModifier<T>() where T : Modifier;
    public abstract void AddModifier<T>(T modifier) where T : Modifier;
    public abstract void RemoveModifier<T>(T modifier) where T : Modifier;

    public Dictionary<Func<Entity, bool>, EntityPayload> OnRegistered() {
        var dict = new Dictionary<Func<Entity, bool>, EntityPayload>();
        foreach (var modifier in Modifiers) {
            if (!dict.TryGetValue(modifier.Condition, out var payload)) {
                payload = new EntityPayload();
                dict.Add(modifier.Condition, payload);
            }

            payload.Content += modifier.Registered;
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

            payload.Content += modifier.Deregistered;
        }

        return dict;
    }

    public List<Modifier> Modifiers => GetComponents<Modifier>().ToList();
}

public abstract class Modifier : MonoBehaviour {
    public virtual Func<Entity, bool> Condition {
        get => entity => entity != Modifiable;
    }

    public enum AttachedTo {
        Square,
        Entity,
        Both
    }

    private void OnValidate() {
        if (!gameObject.TryGetComponent<Modifiable>(out _)) WarningAndRemove();
        switch (Attach) {
            case AttachedTo.Square:
                if (!gameObject.TryGetComponent<Square>(out _)) WarningAndRemove(typeof(Square));
                break;
            case AttachedTo.Entity:
                if (!gameObject.TryGetComponent<Entity>(out _)) WarningAndRemove(typeof(Entity));
                break;
            case AttachedTo.Both:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        [ExecuteAlways]
        void WarningAndRemove([CanBeNull] Type type = null) {
            Debug.LogError( type != null 
                ? $"Modifier {GetType()} cannot be attached to {gameObject.name} of type {typeof(Modifiable).Name}."
                : $"Modifier {GetType()} requires Component either of type {nameof(Entity)} or {nameof(Square)} to function.");
            // if (Application.isPlaying) Destroy(gameObject.GetComponent(this.GetType()));
            // else {
                UnityEditor.EditorApplication.delayCall += () => {
                    DestroyImmediate(gameObject.GetComponent(this.GetType()));
                };
            // }
        }
    }

    public virtual AttachedTo Attach { get; } = AttachedTo.Both;

    protected Modifiable Modifiable;
    protected bool IsInitialized = false;
    protected int InitializedTurnNumber;
    protected Turn InitializedTurn;

    public void Awake() {
        this.Modifiable = gameObject.GetComponent<Modifiable>();
        if (!Validate()) {
            Debug.LogError($"{GetType().Name} can't be attached to {gameObject.name}");
            Destroy(this);
            return;
        }

        IsInitialized = true;
        InitializedTurn = GameController.Instance.TurnManager.CurrentTurn;
        InitializedTurnNumber = GameController.Instance.TurnManager.TurnNumber;

        OnInitialize();

        bool Validate() => Attach switch {
            AttachedTo.Square => Modifiable is Square,
            AttachedTo.Entity => Modifiable is Entity,
            AttachedTo.Both => true,
            _ => throw new ArgumentOutOfRangeException()
        };
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

    protected virtual void OnDestroy() {
        Modifiable.RemoveModifier(this);
    }

    public virtual void Registered(IVisitable visitable) {
        if (!IsInitialized) {
            // throw new Exception("This modifier has not been initialized.");
            return;
        }

        if (visitable is Entity entity) OnRegisteredAsEntity(entity);
        if (visitable is Square square) OnRegisteredAsSquare(square);
    }

    public virtual void Deregistered(IVisitable visitable) {
        if (!IsInitialized) {
            // throw new Exception("This modifier has not been initialized.");
            return;
        }

        if (visitable is Entity entity && entity != null) OnDeregisteredAsEntity(entity);
        if (visitable is Square square && square != null) OnDeregisteredAsSquare(square);
    }

    protected abstract void OnDeregisteredAsEntity(Entity entity);
    protected abstract void OnDeregisteredAsSquare(Square square);
    protected abstract void OnRegisteredAsEntity(Entity entity);
    protected abstract void OnRegisteredAsSquare(Square square);
}