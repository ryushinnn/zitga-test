using TMPro;
using UnityEngine;

public class RecyclableScrollItem : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TMP_Text labelText;
    
    public RectTransform RectTransform => rectTransform;
    public float Height => rectTransform.rect.height;
    public float Width => rectTransform.rect.width;

    public void Initialize() {
        
    }

    public void SetData(int data) {
        labelText.text = data.ToString();
    }
}