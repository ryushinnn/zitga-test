using System;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField] MapUI mapUI;
    [SerializeField] MainUI mainUI;

    public static MapUI Map { get; private set; }
    public static MainUI Main { get; private set; }

    void Awake() {
        Map = mapUI;
        Main = mainUI;

        Map.Open();
        Main.Close();
    }
}