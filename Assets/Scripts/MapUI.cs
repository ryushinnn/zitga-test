using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapUI : MonoBehaviour {
    [SerializeField] Button unlockRandomButton;
    [SerializeField] Button resetButton;

    void Start() {
        unlockRandomButton.onClick.AddListener(UnlockRandomStage);
        resetButton.onClick.AddListener(ResetStages);
    }

    void UnlockRandomStage() {
        var rand = Random.Range(1, 20);
        Debug.Log($"unlock {rand}");
        DataManager.Instance.StagesData.Unlock(rand);
    }

    void ResetStages() {
        DataManager.Instance.StagesData.Reset();
    }
}