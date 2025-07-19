using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Maze : MonoBehaviour {
    [SerializeField] Cell cellPrefab;
    [SerializeField] Bug bug;
    [SerializeField] Transform target;
    [SerializeField] LineRenderer hint;

    Cell[] cells;
    MazeData data;
    Vector2Int start;
    Vector2Int end;
    List<Vector3> waypoints;
    bool pathRevealed;
    bool bugMoved;

    const float CELL_SIZE = 0.5f;
    const int WIDTH = 10;
    const int HEIGHT = 13;

    void Awake() {
        SpawnCells();
    }

    void SpawnCells() {
        cells = new Cell[WIDTH * HEIGHT];
        var offsetX = CELL_SIZE * (WIDTH - 1) / -2f;
        var offsetY = CELL_SIZE * (HEIGHT - 1) / 2f;
        for (int i = 0; i < cells.Length; i++) {
            var cell = Instantiate(cellPrefab, transform);
            var row = i / WIDTH;
            var col = i % WIDTH;
            cell.name = $"Cell [{row},{col}]";
            cell.transform.localPosition = new Vector3(col * CELL_SIZE + offsetX, -row * CELL_SIZE + offsetY, 0);
            cell.transform.localScale = Vector3.one * CELL_SIZE;
            cells[i] = cell;
        }
    }

    public void Initialize(CellData[,] grid, Vector2Int origin, Vector2Int destination) {
        data = new MazeData(grid);
        start = origin;
        end = destination;
        GenerateMaze();
    }

    public void Initialize(out CellData[,] grid, out Vector2Int origin, out Vector2Int destination) {
        data = new MazeData(WIDTH, HEIGHT);
        start = new Vector2Int(0, 0);
        end = new Vector2Int(Random.Range(1, WIDTH), Random.Range(1, HEIGHT));
        GenerateMaze();
        
        grid = data.Grid;
        origin = start;
        destination = end;
    }

    void GenerateMaze() {
        for (int y = 0; y < HEIGHT; y++) {
            for (int x = 0; x < WIDTH; x++) {
                int index = y * WIDTH + x;
                cells[index].Initialize(data.Grid[y, x]);
            }
        }
        
        var path = MazePathFinder.Find(data.Grid, start, end);
        GetWaypoints(path);
        bug.transform.position = waypoints[0];
        target.position = waypoints[^1];
        pathRevealed = false;
        bugMoved = false;
        hint.positionCount = 0;
    }

    int GetCellIndex(Vector2Int vec) {
        return vec.y * WIDTH + vec.x;
    }

    void GetWaypoints(List<Vector2Int> path) {
        waypoints = new List<Vector3> { cells[GetCellIndex(path[0])].transform.position };

        for (int i = 1; i < path.Count - 1; i++) {
            var prev = path[i - 1];
            var cur = path[i];
            var next = path[i + 1];

            if (cur - prev != next - cur) {
                waypoints.Add(cells[GetCellIndex(cur)].transform.position);
            }
        }
        
        waypoints.Add(cells[GetCellIndex(path[^1])].transform.position);
    }

    public void ShowPathToTarget() {
        if (pathRevealed) return;

        pathRevealed = true;
        hint.positionCount = waypoints.Count;
        for (int i=0; i<waypoints.Count; i++) {
            hint.SetPosition(i, waypoints[i]);
        }
        EditorConsole.Log("Path revealed");
    }

    public void LetBugMoveToTarget() {
        if (!pathRevealed || bugMoved) return;

        bugMoved = true;
        bug.StartMove(waypoints);
        EditorConsole.Log("Bug started moving");
    }

    public void CancelBugMovement() {
        bug.StopMove();
    }
}