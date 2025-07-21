using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellData {
    public HashSet<Direction> connections = new();
}

public class MazeData {
    public CellData[,] Grid { get; }
    
    readonly int width;
    readonly int height;

    readonly Direction[] directions = {
        Direction.Up,
        Direction.Down,
        Direction.Left,
        Direction.Right
    };

    public MazeData(int width, int height) {
        this.width = width;
        this.height = height;
        Grid = new CellData[this.height, this.width];
        bool[,] visited = new bool[this.height, this.width];
        
        for (int y = 0; y < this.height; y++) {
            for (int x = 0; x < this.width; x++) {
                Grid[y, x] = new CellData();
            }
        }
        
        DFS(new Vector2Int(0,0), visited);
    }

    public MazeData(CellData[,] grid) {
        width = grid.GetLength(0);
        height = grid.GetLength(1);
        Grid = grid;
    }

    void DFS(Vector2Int current, bool[,] visited) {
        visited[current.y, current.x] = true;

        var dirs = new List<Direction>(directions);
        for (int i = 0; i < dirs.Count; i++) {
            var j = Random.Range(i, dirs.Count);
            (dirs[i], dirs[j]) = (dirs[j], dirs[i]);
        }

        foreach (var dir in dirs) {
            var next = current + dir.GetOffset();
            if (!IsValid(next) || visited[next.y, next.x]) continue;
            
            Grid[current.y, current.x].connections.Add(dir);
            Grid[next.y, next.x].connections.Add(dir.GetOpposite());
            DFS(next, visited);
        }
    }

    bool IsValid(Vector2Int pos) {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }
}