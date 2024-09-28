using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class SquareRule {
    private string GetRuleNameOrDefault([CanBeNull] string name) =>
        (string.IsNullOrEmpty(name) || name == "") ? GetType().Name : name;

    [SerializeField] private string ruleName;

    public string RuleName {
        get => GetRuleNameOrDefault(ruleName);
        set => ruleName = value;
    }

    public Square Square;

    public Vector2Int[] Positions => CalculatePositions();

    public Vector2Int BoardSize { get; set; }

    public virtual Vector2Int[] CalculatePositions() {
        if (Square is null || BoardSize == Vector2Int.zero) throw new NullReferenceException();

        return Rule();
    }

    protected abstract Vector2Int[] Rule();

    public static implicit operator Vector2Int[](SquareRule rule) => rule.Positions;
}

[Serializable]
public class CastleRule : SquareRule {
    [SerializeField] [Min(1)]private int height = 2;
    [SerializeField] private CastlePosition position;
    [SerializeField] private bool isWidthLimited;
    [SerializeField] private int widthLimit = 10;

    public enum CastlePosition {
        Top,
        Bottom,
        Middle,
        TopAndBottom
    }

    protected override Vector2Int[] Rule() {
        var castleWidth = BoardSize.x <= 8 ? BoardSize.x
            : !isWidthLimited ? BoardSize.x - (BoardSize.x % 2)
            : BoardSize.x - (BoardSize.x % 2) <= widthLimit ? BoardSize.x - (BoardSize.x % 2)
            : widthLimit;
        return CalculatePositions(new Vector2Int(castleWidth, height), position, BoardSize);
    }

    Vector2Int[] CalculatePositions(Vector2Int castleSize, CastlePosition castlePosition, Vector2Int boardSize) {
        List<Vector2Int> positions = new();

        int horizontalStart = boardSize.x <= castleSize.x
            ? 0
            : boardSize.x / 2 - castleSize.x;

        for (int x = horizontalStart + 1; x <= (boardSize.x > castleSize.x ? boardSize.x : castleSize.x); x++) {
            for (int y = 1; y <= (boardSize.y > castleSize.y ? boardSize.y : castleSize.y); y++) {
                positions.Add(new Vector2Int(x, y));
            }
        }

        switch (castlePosition) {
            case CastlePosition.Top:
                positions.ForEach(e => e.y = e.y + (boardSize.y - castleSize.y));
                return positions.ToArray();
            case CastlePosition.Bottom:
                return positions.ToArray();
            case CastlePosition.Middle:
                positions.ForEach(e => e.y = e.y + (boardSize.y / 2 - castleSize.y / 2));
                return positions.ToArray();
            case CastlePosition.TopAndBottom:
                var _ = positions;
                _.ForEach(e => e.y = e.y + (boardSize.y - castleSize.y));
                return positions.Union(_).ToArray();
            default:
                throw new NotImplementedException();
        }
    }
}

[Serializable]
public class WallRule : SquareRule {
    [SerializeField] private Vector2Int origin;
    [SerializeField] private bool horizontalWall;
    [SerializeField] private bool verticalWall;

    protected override Vector2Int[] Rule() {
        var list = new List<Vector2Int> { origin };

        if (horizontalWall) {
            for (int i = 1; i <= BoardSize.x; i++) {
                if (!list.Contains(new Vector2Int(i, origin.y))) list.Add(new Vector2Int(i, origin.y));
            }
        }

        if (verticalWall) {
            for (int i = 1; i <= BoardSize.y; i++) {
                if (!list.Contains(new Vector2Int(origin.x, i))) list.Add(new Vector2Int(origin.x, i));
            }
        }

        return list.ToArray();
    }
}

[Serializable]
public class LineRule : SquareRule {
    [SerializeField] private Vector2Int start;
    [SerializeField] private Vector2Int end;

    protected override Vector2Int[] Rule() => PathFinder.CalculatePath(start, end, 1, false).ToArray();
}