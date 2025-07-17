using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class StageData {
    public int stars;
    public string maze;
}

[Serializable]
public class StagesData {
    public StageData[] stages;
    public event Action OnChange;
    
    public const int MAX_STAGES = 999;
    const string STAGES_DATA_KEY = "STAGES_DATA";

    StagesData() {
        stages = new StageData[1];
        stages[0] = new StageData {
            stars = 0,
        };
    }

    public static StagesData Get() {
        var json = PlayerPrefs.GetString(STAGES_DATA_KEY);
        if (json == null) {
            return new StagesData();
        }
        
        return JsonUtility.FromJson<StagesData>(json) ?? new StagesData();
    }

    public void Unlock(int current) {
        current = Mathf.Clamp(current, 1, MAX_STAGES);
        var oldStages = stages;
        stages = new StageData[current];
        for (int i = 0; i < current; i++) {
            stages[i] = new StageData {
                stars = i == current - 1 ? 0 : Random.Range(1, 4),
            };
            if (i < oldStages.Length) {
                stages[i].maze = oldStages[i].maze;
            }
        }
        OnChange?.Invoke();
        PlayerPrefs.SetString(STAGES_DATA_KEY, JsonUtility.ToJson(this));
    }

    public void Reset() {
        stages = new StageData[1];
        stages[0] = new StageData {
            stars = 0,
        };
        OnChange?.Invoke();
        PlayerPrefs.SetString(STAGES_DATA_KEY, JsonUtility.ToJson(this));
    }

    public StageData GetStage(int index) {
        return index > stages.Length ? null : stages[index - 1];
    }
}