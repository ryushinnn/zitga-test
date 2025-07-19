using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : BaseUI {
    [SerializeField] Button openMapButton;
    [SerializeField] Button findButton;
    [SerializeField] Button moveButton;

    void Awake() {
        openMapButton.onClick.AddListener(OpenMap);
        findButton.onClick.AddListener(Find);
        moveButton.onClick.AddListener(AutoMove);
    }

    void OpenMap() {
        Close();
        UIManager.Map.Open();
        GameManager.Instance.CancelMove();
    }

    void Find() {
        GameManager.Instance.Find();
    }

    void AutoMove() {
        GameManager.Instance.AutoMove();
    }
}