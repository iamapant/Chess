using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Square : Modifiable, IVisitable {
    [HideInInspector]public EntityMediator Mediator;

    [Header("Rendering")]
    [SerializeReference, SubclassSelector] RenderSupplier RenderSupplier; 

    public Vector2Int position {
        get {
            if (chessBoard != null) {
                foreach (var pair in chessBoard.BoardSquares) {
                    if (pair.Value == this) return pair.Key;
                }
            }

            throw new Exception("Cannot get position from board");
        }
    }
    
    public Entity[] Entities => GetComponentsInChildren<Entity>();

    protected ChessBoard chessBoard;

    public virtual void Initialize(ChessBoard chessBoard) {
        this.chessBoard = chessBoard;
    }

    private void Awake() {
        RenderSupplier.RenderObject = gameObject;
        Mediator = gameObject.AddComponent<EntityMediator>();
        // Modifiers.ForEach(e => e.Initialize(this));
    }

    private void Start() {
        RenderSupplier.Render();
    }

    private void OnEnable() {
        if (GameController.Instance.TurnManager != null) GameController.Instance.TurnManager.UpdateTurn += OnUpdateTurn;
    }

    private void OnDisable() {
        if (GameController.Instance.TurnManager != null) GameController.Instance.TurnManager.UpdateTurn -= OnUpdateTurn;
    }

    protected virtual void OnUpdateTurn(Turn turn) { }

    public override void Accept(IVisitor visitor) {
        var payload = visitor as EntityPayload;
        payload?.Content(this);
    }
    
    public override void AddModifier<T>(){
        if (gameObject.TryGetComponent(typeof(T), out var mod))
            RemoveModifier(mod as Modifier);

        T modifier = (T)gameObject.AddComponent(typeof(T));
        // if (modifier == null) throw new NullReferenceException(modifier.GetType().Name);
        // modifier.Initialize(this);

        var payload = new EntityPayload();
        payload.Content += modifier.Registered;
        
        Mediator.Broadcast(this, payload, modifier.Condition);
    }
    
    public override void AddModifier<T>(T modifier){
        if (gameObject.TryGetComponent(typeof(T), out var mod))
            RemoveModifier(mod as Modifier);

        T m = (T)gameObject.AddComponent(typeof(T));
        // if (modifier == null) throw new NullReferenceException(modifier.GetType().Name);
        // modifier.Initialize(this);

        try {
            foreach (var property in typeof(T).GetProperties()) {
                property.SetValue(m, property.GetValue(modifier));
            }

            foreach (var field in typeof(T).GetFields()) {
                field.SetValue(m, field.GetValue(modifier));
            }
        }
        catch (Exception ex) {
            Debug.Log(ex);
        }

        var payload = new EntityPayload();
        payload.Content += m.Registered;
        
        Mediator.Broadcast(this, payload, m.Condition);
    }

    public override void RemoveModifier<T>(T modifier) {
        var payload = new EntityPayload();
        payload.Content += modifier.Deregistered;
        Mediator.Broadcast(this, payload, modifier.Condition);

        Destroy(modifier);
    }
}