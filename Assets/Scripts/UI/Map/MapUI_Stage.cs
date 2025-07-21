using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI_Stage : MonoBehaviour {
    [SerializeField] TMP_Text indexText;
    [SerializeField] Image tutorial;
    [SerializeField] Image[] starImages;
    [SerializeField] Image lockImage;
    [SerializeField] Button selfButton;

    int stageIndex;
    StageData stageData;
    bool available;
    
    void Awake() {
        selfButton.onClick.AddListener(StartGame);
    }

    public void SetData(int stageIndex) {
        this.stageIndex = stageIndex;
        indexText.text = this.stageIndex.ToString();
        indexText.enabled = this.stageIndex != 1;
        tutorial.enabled = this.stageIndex == 1;
        Refresh();
        DataManager.Instance.StagesData.OnChange += Refresh;
    }

    public void MarkAsUnused() {
        DataManager.Instance.StagesData.OnChange -= Refresh;
    }

    void Refresh() {
        stageData = DataManager.Instance.StagesData.GetStage(stageIndex);
        for (int i = 0; i < starImages.Length; i++) {
            starImages[i].enabled = stageData != null && i < stageData.stars;
        }
        lockImage.enabled = stageData == null;
        available = stageData != null;
    }

    void StartGame() {
        if (!available) return;
        
        UIManager.Map.Close();
        UIManager.Main.Open();
        GameManager.Instance.InitializeMaze(stageIndex);
    }
}