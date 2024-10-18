using System.Collections.Generic;

public static class MovableSquare {
    public static int ContinuousRushLimit = 10;

    public static List<Square> QueryValidSquares(Entity entity) {
        List<Square> validSquares = new();
        if (!GameController.Instance.ChessBoard.TryGetLocation(entity.GetSquare(), out var position))
            return new List<Square>();

        foreach (var direction in entity.MovableDirections.Keys) {
            if (entity.MovableDirections[direction]) {
                int continueTimes = 1;
                while (continueTimes <= ContinuousRushLimit) {
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