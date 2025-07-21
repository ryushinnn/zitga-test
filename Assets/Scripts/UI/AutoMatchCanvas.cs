using UnityEngine;
using UnityEngine.UI;

public class AutoMatchCanvas : MonoBehaviour {
    const int DEFAULT_WIDTH = 1080;
    const int DEFAULT_HEIGHT = 1920;

    void Awake() {
        float currentRatio = (float)Screen.width / Screen.height;
        float defaultRatio = (float)DEFAULT_WIDTH / DEFAULT_HEIGHT;
        if (currentRatio > defaultRatio) {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        } else {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
    }
}