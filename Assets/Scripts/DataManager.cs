using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DataManager : Singleton<DataManager> {
    public StagesData StagesData { get; private set; }
    
    protected override void OnAwake() {
        StagesData = StagesData.Get();
        Debug.Log($"last: {StagesData.stages.Length}");
    }

    void Start() {
        
    }
}