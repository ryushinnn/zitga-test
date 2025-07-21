using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapUI_StageRow : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [SerializeField] MapUI_Stage[] stages;
    [SerializeField] HorizontalLayoutGroup horizontalLg;
    [SerializeField] Image lineVLeft, lineVRight;
    
    public RectTransform RectTransform => rectTransform;
    public float Height => rectTransform.rect.height;
    public float Width => rectTransform.rect.width;

    public void SetData(List<int> stageIndexes) {
        for (int i = 0; i < stages.Length; i++) {
            if (i < stageIndexes.Count) {
                stages[i].SetData(stageIndexes[i]);
                stages[i].gameObject.SetActive(true);
            }
            else {
                stages[i].MarkAsUnused();
                stages[i].gameObject.SetActive(false);
            }
        }

        var fromLeft = stageIndexes[0] / 4 % 2 == 0;
        horizontalLg.childAlignment = fromLeft ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
        horizontalLg.reverseArrangement = !fromLeft;
        lineVLeft.enabled = fromLeft && stageIndexes[0] != 1;
        lineVRight.enabled = !fromLeft && stageIndexes[0] != 1;
    }
    
    public void MarkAsUnused() {
        foreach (var stage in stages) {
            stage.MarkAsUnused();
        }
    }
}