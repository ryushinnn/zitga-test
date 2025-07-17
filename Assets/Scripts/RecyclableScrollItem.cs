using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecyclableScrollItem : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TMP_Text labelText;
    [SerializeField] Image[] starImages;
    [SerializeField] Image lockImage;

    int currentStageIndex;
    StageData stageData;
    
    public RectTransform RectTransform => rectTransform;
    public float Height => rectTransform.rect.height;
    public float Width => rectTransform.rect.width;

    public void SetData(int stageIndex) {
        currentStageIndex = stageIndex;
        labelText.text = currentStageIndex == 1 ? "Tutorial" : currentStageIndex.ToString();
        Refresh();
        DataManager.Instance.StagesData.OnChange += Refresh;
    }

    public void MarkAsUnused() {
        DataManager.Instance.StagesData.OnChange -= Refresh;
    }

    void Refresh() {
        stageData = DataManager.Instance.StagesData.GetStage(currentStageIndex);
        for (int i = 0; i < starImages.Length; i++) {
            starImages[i].enabled = stageData != null && i < stageData.stars;
        }
        lockImage.enabled = stageData == null;
    }
}