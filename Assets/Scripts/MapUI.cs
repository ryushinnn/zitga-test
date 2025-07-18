using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapUI : BaseUI {
    [SerializeField] Button unlockRandomButton;
    [SerializeField] Button resetButton;
    [SerializeField] MapUI_StagesScrollView stagesScrollView;

    void Awake() {
        unlockRandomButton.onClick.AddListener(UnlockRandomStage);
        resetButton.onClick.AddListener(ResetStages);
    }

    void Start() {
        InitializeStagesScrollView(StagesData.MAX_STAGES);
    }

    void UnlockRandomStage() {
        var rand = Random.Range(1, 20);
        Debug.Log($"unlock {rand}");
        DataManager.Instance.StagesData.Unlock(rand);
    }

    void ResetStages() {
        DataManager.Instance.StagesData.Reset();
    }

    void InitializeStagesScrollView(int itemCount) {
        stagesScrollView.Initialize(itemCount);
    }
}