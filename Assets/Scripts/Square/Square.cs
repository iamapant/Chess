using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Square : MonoBehaviour, IVisitable {
    public EntityMediator Mediator;

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
    }

    private void Start() {
        var mediator = gameObject.GetComponent<EntityMediator>();
        if (!mediator) {
            mediator = gameObject.AddComponent<EntityMediator>();
        }

        Mediator = mediator;
        RenderSupplier.Render();
    }

    private void OnEnable() {
        GameController.Instance.TurnManager.UpdateTurn += OnUpdateTurn;
    }

    private void OnDisable() {
        GameController.Instance.TurnManager.UpdateTurn -= OnUpdateTurn;
    }

    public void Accept(IVisitor visitor) {
        if (visitor is EntityPayload payload) payload.Content(this);
    }

    protected virtual void OnUpdateTurn(Turn turn) { }
}