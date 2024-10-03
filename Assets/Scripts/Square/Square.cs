using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Square : MonoBehaviour, IVisitable {
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

    public void Accept(IVisitor visitor) {
        var payload = visitor as EntityPayload;
        payload?.Content(this);
    }
}