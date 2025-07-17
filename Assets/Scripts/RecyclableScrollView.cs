using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecyclableScrollView : MonoBehaviour {
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentRect;
    [SerializeField] RecyclableScrollItem itemPrefab;
    
    int itemCount;
    LinkedList<RecyclableScrollItem> items = new();
    List<int> dataList = new();
    float itemHeight;
    float itemWidth;
    int poolSize;
    int oldFirstVisibleIndex;
    int visibleItemCount;
    int debt;
    bool shiftFirstRowToLeft;
    
    const int BUFFER = 2;
    const int ITEMS_PER_ROW = 4;

    void Start() {
        itemCount = StagesData.MAX_STAGES;
        var testDataList = new List<int>();
        debt = (ITEMS_PER_ROW - itemCount % ITEMS_PER_ROW) % ITEMS_PER_ROW;
        var groupIndex = (itemCount + debt) / ITEMS_PER_ROW - 1;
        for (int i = itemCount + debt; i >= 1; i -= 4) {
            var group = new List<int>();
            for (int j = 0; j < 4 && (i - j) >= 1; j++) {
                if (i - j <= itemCount) {
                    group.Add(i-j);
                }
            }

            if (groupIndex % 2 == 0) {
                group.Sort();
            }
            
            testDataList.AddRange(group);
            groupIndex--;
        }

        Initialize(testDataList);
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    void Initialize(List<int> dataList) {
        this.dataList = dataList;

        var scrollRT = scrollRect.GetComponent<RectTransform>();
        itemHeight = itemPrefab.Height;
        itemWidth = itemPrefab.Width;

        // var totalRows = Mathf.CeilToInt((float)this.dataList.Count / itemsPerRow);
        var totalRows = (this.dataList.Count + debt) / ITEMS_PER_ROW;
        var contentHeight = itemHeight * totalRows;
        shiftFirstRowToLeft = totalRows % 2 == 1;

        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.anchorMin = new Vector2(0, 1);

        visibleItemCount = (int)(scrollRT.rect.height / itemHeight) * ITEMS_PER_ROW;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
        
        poolSize = visibleItemCount + (BUFFER * 2 * ITEMS_PER_ROW);
        var index = -BUFFER * ITEMS_PER_ROW;
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
        var firstVisibleIndex = firstVisibleRow * ITEMS_PER_ROW;

        if (oldFirstVisibleIndex != firstVisibleIndex) {
            var diff = (oldFirstVisibleIndex - firstVisibleIndex) / ITEMS_PER_ROW;
            if (diff < 0) {
                var lastVisibleIndex = oldFirstVisibleIndex + visibleItemCount;
                var count = Mathf.Abs(diff) * ITEMS_PER_ROW;
                for (int i = 0; i < count; i++) {
                    var item = items.First.Value;
                    items.RemoveFirst();
                    items.AddLast(item);
                    var newIndex = lastVisibleIndex + (BUFFER * ITEMS_PER_ROW) + i;
                    UpdateItem(item, newIndex);
                }
            }
            else if (diff > 0) {
                var count = Mathf.Abs(diff) * ITEMS_PER_ROW;
                for (int i = 0; i < count; i++) {
                    var item = items.Last.Value;
                    items.RemoveLast();
                    items.AddFirst(item);
                    var newIndex = oldFirstVisibleIndex - (BUFFER * ITEMS_PER_ROW) - i;
                    UpdateItem(item, newIndex);
                }
            }
            oldFirstVisibleIndex = firstVisibleIndex;
        }
    }

    void UpdateItem(RecyclableScrollItem item, int index) {
        var row = 0 <= index + debt ? (index + debt) / ITEMS_PER_ROW : (index + debt - 1) / ITEMS_PER_ROW;
        var col = row == 0 && shiftFirstRowToLeft ? Mathf.Abs(index) % ITEMS_PER_ROW : Mathf.Abs(index + debt) % ITEMS_PER_ROW;
        var pivot = item.RectTransform.pivot;
        var totalWidth = ITEMS_PER_ROW * itemWidth;
        var contentWidth = contentRect.rect.width;
        var offsetX = (contentWidth - totalWidth) / 2f;
        var y = -(row * itemHeight) - itemHeight * (1 - pivot.y);
        var x = (col * itemWidth + itemWidth * pivot.x) + offsetX;

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
}