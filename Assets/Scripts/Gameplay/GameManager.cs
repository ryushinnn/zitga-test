using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    [SerializeField] Maze maze;

    protected override void OnAwake() {
        Application.targetFrameRate = 60;
    }

    public void InitializeMaze(int stageIndex) {
        var stageData = DataManager.Instance.StagesData.GetStage(stageIndex);
        if (stageData.grid != null) {
            var origin = new Vector2Int(stageData.origin.x, stageData.origin.y);
            var destination = new Vector2Int(stageData.destination.x, stageData.destination.y);
            maze.Initialize(stageData.grid, origin, destination);
        }
        else {
            maze.Initialize(out var grid, out var origin, out var destination);
            DataManager.Instance.StagesData.SetStageGridAndRoute(stageIndex, grid, origin, destination);
        }
    }

    public void Find() {
        maze.ShowPathToTarget();
    }

    public void AutoMove() {
        maze.LetBugMoveToTarget();
    }

    public void CancelMove() {
        maze.CancelBugMovement();
    }
}