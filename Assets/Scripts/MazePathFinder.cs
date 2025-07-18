using System.Collections.Generic;
using UnityEngine;

public static class MazePathFinder {
    public static List<Vector2Int> Find(CellData[,] grid, Vector2Int start, Vector2Int end) {
        var openSet = new PriorityQueue<Vector2Int, int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, int>();
        
        openSet.Enqueue(start, 0);
        gScore[start] = 0;

        while (openSet.Count > 0) {
            var current = openSet.Dequeue();

            if (current == end) {
                return ReconstructPath(cameFrom, current);
            }

            foreach (var dir in grid[current.y, current.x].connections) {
                var neighbor = current + dir.GetOffset();
                int tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    var fScore = tentativeGScore + Heuristic(neighbor, end);
                    openSet.Enqueue(neighbor, fScore);
                }
            }
        }

        return null;
    }
    
    static int Heuristic(Vector2Int a, Vector2Int b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current) {
        var path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        return path;
    }
}