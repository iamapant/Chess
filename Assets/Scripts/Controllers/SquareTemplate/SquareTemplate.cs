using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class SquareTemplate {
    [SerializeField] public RenderSupplier Rendering;
    public Action<Square, Entity> EntityEnter;
    public Action<Square, Entity> EntityExit;
    public Action<Square> ValueChanged;
    protected SquareTemplateController Controller;

    public void Initialize(Square square) {
        OnInitialize(square); 
        Controller = GameController.Instance.SquareTemplateController;
        EntityEnter += OnEntityEnter;
        EntityExit += OnEntityExit;
        ValueChanged += OnValueChanged;
    }
    public void Update(Square square) { OnUpdate(square); }
    public void TurnChange(Square square) { OnTurnChange(square); }
    public void Begin(Square square) { OnBegin(square); }
    public void End(Square square) { OnEnd(square); Reset(square); }
    public void Reset(Square square) { OnReset(square); }
    
    protected abstract void OnEntityEnter(Square square, Entity entity);
    protected abstract void OnEntityExit(Square square, Entity entity);
    protected abstract void OnValueChanged(Square square);
    protected abstract void OnInitialize(Square square);
    protected abstract void OnUpdate(Square square);
    protected abstract void OnTurnChange(Square square);
    protected abstract void OnBegin(Square square);
    protected abstract void OnEnd(Square square);
    protected abstract void OnReset(Square square);
}