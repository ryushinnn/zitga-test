using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : BaseUI {
    [SerializeField] Button openMapButton;

    void Awake() {
        openMapButton.onClick.AddListener(OpenMap);
    }

    void OpenMap() {
        Close();
        UIManager.Map.Open();
    }
}