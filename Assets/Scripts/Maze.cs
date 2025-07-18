using System;
using UnityEngine;

public class Maze : MonoBehaviour {
    [SerializeField] Cell cellPrefab;

    Cell[] cells;

    const float CELL_SIZE = 0.5f;
    const int WIDTH = 10;
    const int HEIGHT = 13;

    void Start() {
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

        var data = new MazeData(WIDTH, HEIGHT);
        var debug = "";
        for (int y = 0; y < HEIGHT; y++) {
            string debugLine = "";
            for (int x = 0; x < WIDTH; x++) {
                var c = data.Grid[y, x];
                debugLine += "[";
                debugLine += c.connections.Contains(Direction.Up) ? "U" : " ";
                debugLine += c.connections.Contains(Direction.Down) ? "D" : " ";
                debugLine += c.connections.Contains(Direction.Left) ? "L" : " ";
                debugLine += c.connections.Contains(Direction.Right) ? "R" : " ";
                debugLine += "]";

                int index = y * WIDTH + x;
                cells[index].Initialize(data.Grid[y, x]);
            }

            debug += debugLine + "\n";
        }

        Debug.Log(debug);
    }
}