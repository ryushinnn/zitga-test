using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapUI : BaseUI {
    [SerializeField] Button unlockRandomButton;
    [SerializeField] Button resetButton;
    [SerializeField] MapUI_StageScrollView stageScrollView;

    void Awake() {
        unlockRandomButton.onClick.AddListener(UnlockRandomStage);
        resetButton.onClick.AddListener(ResetStages);
    }

    void Start() {
        InitializeStagesScrollView();
    }

    void UnlockRandomStage() {
        var rand = Random.Range(1, StagesData.MAX_STAGES + 1);
        EditorConsole.Log($"{rand} stages unlocked");
        DataManager.Instance.StagesData.Unlock(rand);
        stageScrollView.FocusAtStage(rand);
    }

    void ResetStages() {
        DataManager.Instance.StagesData.Reset();
        stageScrollView.FocusAtStage(1);
    }

    void InitializeStagesScrollView() {
        stageScrollView.Initialize(StagesData.MAX_STAGES);
        StartCoroutine(DoScrollToCurrent());
    }

    IEnumerator DoScrollToCurrent() {
        yield return new WaitForEndOfFrame();
        stageScrollView.FocusAtStage(DataManager.Instance.StagesData.stages.Length);
    }
}