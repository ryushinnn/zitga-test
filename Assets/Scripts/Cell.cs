using UnityEngine;

public class Cell : MonoBehaviour {
    [SerializeField] SpriteRenderer[] walls;

    public void Initialize(CellData data) {
        for (int i = 0; i < 4; i++) {
            var dir = (Direction)i;
            walls[i].enabled = !data.connections.Contains(dir);
        }
    }
}