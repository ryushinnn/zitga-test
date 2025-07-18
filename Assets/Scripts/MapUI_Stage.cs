using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI_Stage : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TMP_Text labelText;
    [SerializeField] Image[] starImages;
    [SerializeField] Image lockImage;
    [SerializeField] Button selfButton;

    int stageIndex;
    StageData stageData;
    bool available;
    
    public RectTransform RectTransform => rectTransform;
    public float Height => rectTransform.rect.height;
    public float Width => rectTransform.rect.width;

    void Awake() {
        selfButton.onClick.AddListener(StartGame);
    }

    public void SetData(int stageIndex) {
        this.stageIndex = stageIndex;
        labelText.text = this.stageIndex == 1 ? "Tutorial" : this.stageIndex.ToString();
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