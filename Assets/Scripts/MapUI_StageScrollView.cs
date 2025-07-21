using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI_StageScrollView : MonoBehaviour {
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentRect;
    [SerializeField] MapUI_StageRow itemPrefab;
    
    LinkedList<MapUI_StageRow> items = new();
    List<List<int>> dataList = new();
    float itemHeight;
    float itemWidth;
    int poolSize;
    int oldFirstVisibleIndex;
    int visibleItemCount;
    
    const int BUFFER = 2;
    const int STAGES_PER_ITEM = 4;

    public void Initialize(int stageCount) {
        var r = stageCount % STAGES_PER_ITEM;
        var perfectStageCount = stageCount + (r == 0 ? 0 : STAGES_PER_ITEM - r);
        for (int i = perfectStageCount; i >= 1; i -= STAGES_PER_ITEM) {
            var sequence = new List<int>();
            for (int j = STAGES_PER_ITEM - 1; j >= 0; j--) {
                if (i - j <= stageCount) {
                    sequence.Add(i-j);
                }
            }
            
            dataList.Add(sequence);
        }

        SpawnItems();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    void SpawnItems() {
        var scrollRT = scrollRect.GetComponent<RectTransform>();
        itemHeight = itemPrefab.Height;
        itemWidth = itemPrefab.Width;

        var totalRows = dataList.Count;
        var contentHeight = itemHeight * totalRows;

        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.anchorMin = new Vector2(0, 1);

        visibleItemCount = (int)(scrollRT.rect.height / itemHeight);
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
        
        poolSize = visibleItemCount + (BUFFER * 2);
        var index = -BUFFER;
        for (int i = 0; i < poolSize; i++) {
            var item = Instantiate(itemPrefab, contentRect);
            items.AddLast(item);
            UpdateItem(item, index++);
        }
        
        scrollRect.onValueChanged.AddListener(OnScroll);
    }
    
    void OnScroll(Vector2 scrollPos) {
        var contentY = contentRect.anchoredPosition.y;
        var firstVisibleRow = Mathf.Max(0, Mathf.FloorToInt(contentY / (itemHeight)));
        var firstVisibleIndex = firstVisibleRow;

        if (oldFirstVisibleIndex != firstVisibleIndex) {
            var diff = (oldFirstVisibleIndex - firstVisibleIndex);
            if (diff < 0) {
                var lastVisibleIndex = oldFirstVisibleIndex + visibleItemCount;
                var count = Mathf.Abs(diff);
                for (int i = 0; i < count; i++) {
                    var item = items.First.Value;
                    items.RemoveFirst();
                    items.AddLast(item);
                    var newIndex = lastVisibleIndex + (BUFFER) + i;
                    UpdateItem(item, newIndex);
                }
            }
            else if (diff > 0) {
                var count = Mathf.Abs(diff);
                for (int i = 0; i < count; i++) {
                    var item = items.Last.Value;
                    items.RemoveLast();
                    items.AddFirst(item);
                    var newIndex = oldFirstVisibleIndex - (BUFFER) - i;
                    UpdateItem(item, newIndex);
                }
            }
            oldFirstVisibleIndex = firstVisibleIndex;
        }
        
        RearrangeItemsInHierarchy();
    }

    void UpdateItem(MapUI_StageRow item, int index) {
        var row = 0 <= index ? index : index - 1;
        var pivot = item.RectTransform.pivot;
        var totalWidth = itemWidth;
        var contentWidth = contentRect.rect.width;
        var offsetX = (contentWidth - totalWidth) / 2f;
        var y = -(row * itemHeight) - itemHeight * (1 - pivot.y);
        var x = (itemWidth * pivot.x) + offsetX;

        item.RectTransform.localPosition = new Vector3(x, y, 0);

        if (index < 0 || index >= dataList.Count) {
            item.MarkAsUnused();
            item.gameObject.SetActive(false);
        }
        else {
            item.SetData(dataList[index]);
            item.gameObject.SetActive(true);
        }
    }

    void RearrangeItemsInHierarchy() {
        var index = 0;
        foreach (var item in items) {
            item.transform.SetSiblingIndex(index++);
        }
    }
}