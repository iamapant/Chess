using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : Modifiable, IVisitable, IAllowedEntry{
    protected Square Square;
    public Faction Faction;

    #region Rendering

    [Space(20)] [Header("Rendering")] [SerializeReference, SubclassSelector]
    RenderSupplier RenderSupplier;

    #endregion

    #region Move
    [Header("Move")] [SerializeField] protected float MoveSpeed = 1f;
    [SerializeReference, SubclassSelector] private MoveSpeedCalculator MoveSpeedCalculator;
    protected EntityMovementManager Movement = new NoMovement();

    public void SetMovement(EntityMovementManager movement) { Movement = movement; }

    public void RemoveMovement() => SetMovement(new NoMovement());

    #endregion

    void Update() { Movement?.FixedUpdate(this); }

    private void OnEnable() { if (GameController.Instance.TurnManager != null) GameController.Instance.TurnManager.UpdateTurn += OnUpdateTurn; }

    private void OnDisable() { if (GameController.Instance.TurnManager != null) GameController.Instance.TurnManager.UpdateTurn -= OnUpdateTurn; }

    private void Awake() {
        RenderSupplier.RenderObject = gameObject;
        // Modifiers.ForEach(e => e.Initialize(this));
    }

    private void Start() {
        var square = GetComponentInParent<Square>();
        RenderSupplier.Render();
    }

    public override void AddModifier<T>() {
        if (gameObject.TryGetComponent(typeof(T), out var mod))
            RemoveModifier(mod as T);
    
        T modifier = (T)gameObject.AddComponent(typeof(T));
        // if (modifier == null) throw new NullReferenceException(modifier.GetType().Name);
        // modifier.Initialize(this);
    
        var payload = new EntityPayload();
        payload.Content += modifier.Registered;
        
        Square?.Mediator.Broadcast(this, payload, modifier.Condition);
    }

    public override void AddModifier<T>(T modifier) {
        if (gameObject.TryGetComponent(typeof(T), out var mod))
            RemoveModifier(mod as T);

        T m = (T)gameObject.AddComponent(typeof(T));
        // if (modifier == null) throw new NullReferenceException(modifier.GetType().Name);
        // modifier.Initialize(this);

        var payload = new EntityPayload();
        payload.Content += modifier.Registered;
        
        Square?.Mediator.Broadcast(this, payload, modifier.Condition);
    }

    public override void RemoveModifier<T>(T modifier) {
        var payload = new EntityPayload();
        payload.Content += modifier.Deregistered;
        Square?.Mediator.Broadcast(this, payload, modifier.Condition);

        Destroy(modifier);
    }


    public override void Accept(IVisitor visitor) {
        var payload = visitor as EntityPayload;
        payload?.Content(this);
    }

    public void MoveSquare(Square square) {
        if (!MovePrecondition(square)) return;

        this.Square?.Mediator.Deregister(this);
        this.Square = square;

        square.Mediator.Register(this);
        Movement = new StayInSquare(square, MoveSpeed, new LerpSpeedCalculator());

        OnMove(square);
    }

    public Square GetSquare() => Square;

    protected virtual bool MovePrecondition(Square newSquare) {
        if (newSquare == null) return false;

        return true;
    }

    public abstract void OnMove(Square square);

    void OnDestroy() => Square?.Mediator?.Deregister(this);
    //
    // public override Dictionary<Func<Entity, bool>, EntityPayload> OnRegistered() {
    //     var dict = new Dictionary<Func<Entity, bool>, EntityPayload>();
    //     foreach (var modifier in Modifiers) {
    //         if (!dict.TryGetValue(modifier.Condition, out var payload)) {
    //             payload = new EntityPayload();
    //             dict.Add(modifier.Condition, payload);
    //         }
    //
    //         payload.Content += modifier.Registered;
    //     }
    //
    //     return dict;
    // }
    //
    // public override Dictionary<Func<Entity, bool>, EntityPayload> OnDeregistered() {
    //     var dict = new Dictionary<Func<Entity, bool>, EntityPayload>();
    //     foreach (var modifier in Modifiers) {
    //         if (!dict.TryGetValue(modifier.Condition, out var payload)) {
    //             payload = new EntityPayload();
    //             dict.Add(modifier.Condition, payload);
    //         }
    //
    //         payload.Content += modifier.Deregistered;
    //     }
    //
    //     return dict;
    // }


    public abstract bool IsAllowed(Entity entity);
    
    protected virtual void OnUpdateTurn(Turn turn) { }
}