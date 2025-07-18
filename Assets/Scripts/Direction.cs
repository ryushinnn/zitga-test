using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions {
    static readonly Dictionary<Direction, Direction> OPPOSITES = new() {
        { Direction.Up, Direction.Down },
        { Direction.Down, Direction.Up },
        { Direction.Left, Direction.Right },
        { Direction.Right, Direction.Left }
    };

    static readonly Dictionary<Direction, Vector2Int> OFFSETS = new() {
        { Direction.Up, new Vector2Int(0, -1) },
        { Direction.Down, new Vector2Int(0, 1) },
        { Direction.Left, new Vector2Int(-1, 0) },
        { Direction.Right, new Vector2Int(1, 0) }
    };
    
    public static Direction GetOpposite(this Direction direction) {
        return OPPOSITES[direction];
    }

    public static Vector2Int GetOffset(this Direction direction) {
        return OFFSETS[direction];
    }
}