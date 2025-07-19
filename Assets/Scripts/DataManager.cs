using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DataManager : Singleton<DataManager> {
    public StagesData StagesData { get; private set; }
    
    protected override void OnAwake() {
        StagesData = StagesData.Get();
        EditorConsole.Log($"{StagesData.stages.Length} stages unlocked");
    }
}