using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Square : MonoBehaviour, IVisitable {
    [FormerlySerializedAs("EntityMediator")]
    public EntityMediator Mediator;

    public Sprite Sprite;
    protected SpriteRenderer SpriteRenderer;

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
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if (SpriteRenderer) SpriteRenderer.sprite = Sprite;
    }

    private void Start() {
        var mediator = gameObject.GetComponent<EntityMediator>();
        if (!mediator) {
            mediator = gameObject.AddComponent<EntityMediator>();
        }

        Mediator = mediator;
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