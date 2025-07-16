using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecyclableScrollView : MonoBehaviour {
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentRect;
    [SerializeField] RecyclableScrollItem itemPrefab;
    [SerializeField] int itemCount;

    [SerializeField] int buffer;
    [SerializeField] int itemsPerRow;

    LinkedList<RecyclableScrollItem> items = new();
    List<int> dataList = new();
    float itemHeight;
    float itemWidth;
    int poolSize;
    int oldFirstVisibleIndex;
    int visibleItemCount;

    void Start() {
        var testDataList = new List<int>();
        for (int i=1; i<= itemCount; i++) {
            testDataList.Add(i);
        }

        Initialize(testDataList);
    }

    void Initialize(List<int> dataList) {
        this.dataList = dataList;

        var scrollRT = scrollRect.GetComponent<RectTransform>();
        itemHeight = itemPrefab.Height;
        itemWidth = itemPrefab.Width;

        var totalRows = Mathf.CeilToInt((float)this.dataList.Count / itemsPerRow);
        var contentHeight = itemHeight * totalRows;

        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.anchorMin = new Vector2(0, 1);

        visibleItemCount = (int)(scrollRT.rect.height / itemHeight) * itemsPerRow;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);

        poolSize = visibleItemCount + (buffer * 2 * itemsPerRow);
        var index = -buffer * itemsPerRow;
        for (int i = 0; i < poolSize; i++) {
            var item = Instantiate(itemPrefab, contentRect);
            items.AddLast(item);
            item.Initialize();
            UpdateItem(item, index++);
        }
        
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    void OnScroll(Vector2 scrollPos) {
        var contentY = contentRect.anchoredPosition.y;
        var firstVisibleRow = Mathf.Max(0, Mathf.FloorToInt(contentY / (itemHeight)));
        var firstVisibleIndex = firstVisibleRow * itemsPerRow;

        if (oldFirstVisibleIndex != firstVisibleIndex) {
            var diff = (oldFirstVisibleIndex - firstVisibleIndex) / itemsPerRow;
            if (diff < 0) {
                var lastVisibleIndex = oldFirstVisibleIndex + visibleItemCount;
                var count = Mathf.Abs(diff) * itemsPerRow;
                for (int i = 0; i < count; i++) {
                    var item = items.First.Value;
                    items.RemoveFirst();
                    items.AddLast(item);
                    var newIndex = lastVisibleIndex + (buffer * itemsPerRow) + i;
                    UpdateItem(item, newIndex);
                }
            }
            else if (diff > 0) {
                var count = Mathf.Abs(diff) * itemsPerRow;
                for (int i = 0; i < count; i++) {
                    var item = items.Last.Value;
                    items.RemoveLast();
                    items.AddFirst(item);
                    var newIndex = oldFirstVisibleIndex - (buffer * itemsPerRow) - i;
                    UpdateItem(item, newIndex);
                }
            }
            oldFirstVisibleIndex = firstVisibleIndex;
        }
    }

    void UpdateItem(RecyclableScrollItem item, int index) {
        var row = 0 <= index ? index / itemsPerRow : (index - 1) / itemsPerRow;
        var col = Mathf.Abs(index) % itemsPerRow;
        var pivot = item.RectTransform.pivot;
        var totalWidth = itemsPerRow * itemWidth;
        var contentWidth = contentRect.rect.width;
        var offsetX = (contentWidth - totalWidth) / 2f;
        var y = -(row * itemHeight) - itemHeight * (1 - pivot.y);
        var x = (col * itemWidth + itemWidth * pivot.x) + offsetX;

        item.RectTransform.localPosition = new Vector3(x, y, 0);

        if (index < 0 || index >= dataList.Count) {
            item.gameObject.SetActive(false);
        }
        else {
            item.SetData(dataList[index]);
            item.gameObject.SetActive(true);
        }
    }
}