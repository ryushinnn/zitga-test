using System;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class StageData {
    public int stars;
    public CellData[,] grid;
    public XY origin;
    public XY destination;

    public StageData(int stars = 0, CellData[,] grid = null, XY origin = null, XY destination = null) {
        this.stars = stars;
        this.grid = grid;
        this.origin = origin;
        this.destination = destination;
    }
}

public class XY {
    public int x;
    public int y;

    public XY(Vector2Int vec) {
        x = vec.x;
        y = vec.y;
    }
}

[Serializable]
public class StagesData {
    public StageData[] stages;
    public event Action OnChange;
    
    public const int MAX_STAGES = 999;
    const string STAGES_DATA_KEY = "STAGES_DATA";

    StagesData() {
        stages = new StageData[1];
        stages[0] = new StageData();
    }

    public static StagesData Get() {
        var json = PlayerPrefs.GetString(STAGES_DATA_KEY);
        if (json == null) {
            return new StagesData();
        }
        
        return JsonConvert.DeserializeObject<StagesData>(json) ?? new StagesData();
    }

    public void Unlock(int index) {
        index = Mathf.Clamp(index, 1, MAX_STAGES);
        var oldStages = stages;
        stages = new StageData[index];
        for (int i = 0; i < index; i++) {
            var stars = i == index - 1 ? 0 : Random.Range(1, 4);
            var grid = i < oldStages.Length ? oldStages[i].grid : null;
            var origin = i < oldStages.Length ? oldStages[i].origin : null;
            var destination = i < oldStages.Length ? oldStages[i].destination : null;
            stages[i] = new StageData(stars, grid, origin, destination);
        }
        OnChange?.Invoke();
        PlayerPrefs.SetString(STAGES_DATA_KEY, JsonConvert.SerializeObject(this));
    }

    public void Reset() {
        stages = new StageData[1];
        stages[0] = new StageData();
        OnChange?.Invoke();
        PlayerPrefs.SetString(STAGES_DATA_KEY, JsonConvert.SerializeObject(this));
    }

    public void SetStageGridAndRoute(int index, CellData[,] grid, Vector2Int origin, Vector2Int destination) {
        index = Mathf.Clamp(index, 1, MAX_STAGES);
        stages[index - 1].grid = grid;
        stages[index - 1].origin = new XY(origin);
        stages[index - 1].destination = new XY(destination);
        PlayerPrefs.SetString(STAGES_DATA_KEY, JsonConvert.SerializeObject(this));
    }

    public StageData GetStage(int index) {
        return index > stages.Length ? null : stages[index - 1];
    }
}