using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public class CellData {
    public HashSet<Direction> connections = new();
}

public class MazeData {
    public CellData[,] Grid { get; private set; }
    
    int width;
    int height;

    readonly Vector2Int[] directionOffsets = {
        new(0, -1), // Up
        new(0, 1),  // Down
        new(-1, 0), // Left
        new(1, 0)   // Right
    };

    readonly Direction[] directions = {
        Direction.Up,
        Direction.Down,
        Direction.Left,
        Direction.Right
    };
    
    readonly Dictionary<Direction, Direction> opposites = new() {
        { Direction.Up, Direction.Down },
        { Direction.Down, Direction.Up },
        { Direction.Left, Direction.Right },
        { Direction.Right, Direction.Left }
    };

    MazeData() {
        
    }

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

    void DFS(Vector2Int current, bool[,] visited) {
        visited[current.y, current.x] = true;

        var dirs = new List<Direction>(directions);
        Shuffle(dirs);

        foreach (var dir in dirs) {
            var next = current + directionOffsets[(int)dir];
            if (!IsValid(next) || visited[next.y, next.x]) continue;
            
            Grid[current.y, current.x].connections.Add(dir);
            Grid[next.y, next.x].connections.Add(opposites[dir]);
            DFS(next, visited);
        }
    }

    bool IsValid(Vector2Int pos) {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    void Shuffle<T>(List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            var j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}