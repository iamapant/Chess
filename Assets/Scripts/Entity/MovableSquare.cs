using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public class MovableSquare {
    public static int AbsoluteContinuousRushLimit = 10;
    public SerializedDictionary<Vector2Int, bool> MovableDirections = new();

    public List<Square> QueryValidSquares(Entity entity) {
        List<Square> validSquares = new();
        if (!GameController.Instance.ChessBoard.TryGetSquareLocation(entity.GetSquare(), out var position))
            return new List<Square>();

        foreach (var direction in MovableDirections.Keys) {
            if (MovableDirections[direction]) {
                int continueTimes = 1;
                while (continueTimes <= AbsoluteContinuousRushLimit) {
                    var continuousDirection = direction * continueTimes;
                    var continuousPosition = position + (direction * (continueTimes - 1));

                    var path = PathFinder.CalculatePath(continuousPosition, continuousDirection + position);

                    if (!PathFinder.VerifyPath(entity, path, continuousPosition)
                        || !GameController.Instance.ChessBoard.BoardSquares.TryGetValue(continuousDirection + position,
                            out var boardSquare)) break;
                    validSquares.Add(boardSquare);
                }
            }
            else {
                var path = PathFinder.CalculatePath(position, direction + position);

                if (PathFinder.VerifyPath(entity, path, position)
                    && GameController.Instance.ChessBoard.BoardSquares.TryGetValue(direction + position,
                        out var boardSquare)) validSquares.Add(boardSquare);
            }
        }

        return validSquares;
    }
}