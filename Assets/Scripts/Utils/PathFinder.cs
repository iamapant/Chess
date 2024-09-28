using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathFinder {
    public static Stack<Vector2Int>
        CalculatePath(Vector2Int origin, Vector2Int destination, int depth = 1, bool skipTileIfAvailable = true) {
        // Debug.Log($"Depth: {depth}");
        // Form 3x3 grid around the origin
        var neighbors = GetNeighbors(origin, depth).Except(GetNeighbors(origin, depth - 1)).ToList();
        // If the destination is inside the grid, return the destination
        if (neighbors.Contains(destination)) {
            // Debug.Log($"Found destination: {destination}");
            var stack = new Stack<Vector2Int>();
            stack.Push(destination);
            return stack;
        }

        // Find the point closest to the destination 
        float distance = Vector2Int.Distance(origin, destination);
        // Debug.Log($"Distance from origin {origin} to destination: {distance}");
        List<(Vector2Int Position, float Distance)> result = new();

        foreach (var neighbor in neighbors) {
            var dist = Vector2Int.Distance(origin, neighbor) + Vector2Int.Distance(destination, neighbor);
            // Debug.Log($"Distance from {neighbor} to destination: {dist}");
            result.Add((neighbor, dist));
        }

        // If the 2 closest points have the same score, then expand the grid until a point is selected 
        result = result.OrderBy(e => e.Distance).ToList();
        result = result.Where(e => Mathf.Approximately(e.Distance, result.First().Distance)).ToList();
        ;

        // string log = "";
        // foreach (var vector2Int in result) { log += $"Position: {vector2Int.Position}|Distance: {vector2Int.Distance} , "; }
        //
        // Debug.Log($"{result.Count} closest neighbor are: {log}");

        if (result.Count == 1) {
            var list = CalculatePath(result.First().Position, destination, depth);
            list.Push(result.First().Position);
            return list;
        }

        else {
            var list = CalculatePath(origin, destination, depth + 1);

            if (!skipTileIfAvailable) { result.ForEach(e => list.Push(e.Position)); }

            return list;
        }
        // else Debug.LogWarning($"Cannot get closest neighbor: {result.Count}");

        List<Vector2Int> GetNeighbors(Vector2Int origin, int depth) {
            List<Vector2Int> neighbors = new();
            if (depth == 0) return neighbors;

            for (int x = -depth; x <= depth; x++) {
                for (int y = -depth; y <= depth; y++) {
                    if (x == 0 && y == 0) continue;
                    if (neighbors.Contains(origin + new Vector2Int(x, y))) continue;
                    neighbors.Add(origin + new Vector2Int(x, y));
                }
            }

            return neighbors;
        }
    }
    

    public static bool VerifyPath(Entity entity, Stack<Vector2Int> path, Vector2Int origin) {
        while (path.Count > 0) {
            var location = path.Pop();
            if (!GameController.Instance.ChessBoard.BoardSquares.TryGetValue(location + origin,
                    out var boardSquare))
                return false;

            if (!EntityMovementManager.Peek(entity, boardSquare)) return false;

            if (path.Count == 0) return true;
        }

        return false;
    }
}