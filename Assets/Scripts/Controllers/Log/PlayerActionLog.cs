using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PlayerActionLog : GameActionLog {
    protected Player Player { get; init; }

    public PlayerActionLog(Player player) : base() {
        Player = player;
    }
}

public class MoveAction : PlayerActionLog {
    public Entity Entity { get; init; }
    public Vector2Int From { get; init; }
    public Vector2Int To { get; init; }

    public MoveAction(Player player, Entity entity, Vector2Int from, Vector2Int to) : base(player) {
        Entity = entity;
        From = from;
        To = to;
    }

    public override string ToString() 
    => $"Player {Player} moves {Entity.name} from {From} to {To}";

    protected override void OnRevert() {
        if (GameController.Instance.ChessBoard.TryGetSquare(From, out var square)) {
            GameController.Instance.EntityMovementController.MoveToSquare(Entity, square);
        }
    }
}

public class BuyAction : PlayerActionLog {
    ShoppingItem Item { get; init; }
    Dictionary<string, float> Resource { get; init; }

    public BuyAction(ShoppingItem item, Dictionary<string, float> resource, Player player) : base(player) {
        Item = item;
        Resource = resource;
    }

    public override string ToString()
        => $"Player {Player}: Buy {Item} for {string.Join(", ", Resource.Select(r => $"{r.Key}: {r.Value}"))}";

    protected override void OnRevert() {
        foreach (var key in Resource.Keys) {
        }
    }
}