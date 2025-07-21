using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectUtils {
    public static void FocusAtPoint(this ScrollRect scrollRect, Vector2 focusPoint) {
        scrollRect.normalizedPosition = scrollRect.CalculateScrollPosition(focusPoint);
    }

    public static IEnumerator FocusAtPoint(this ScrollRect scrollRect, Vector2 focusPoint, float speed) {
        return scrollRect.DoLerpToScrollPosition(scrollRect.CalculateScrollPosition(focusPoint), speed);
    }

    static IEnumerator DoLerpToScrollPosition(this ScrollRect scrollRect, Vector2 targetNormPos, float speed) {
        var initNormPos = scrollRect.normalizedPosition;

        var t = 0f;
        while (t < 1f) {
            scrollRect.normalizedPosition = Vector2.LerpUnclamped(initNormPos, targetNormPos, 1f - (1f - t).ToSqr());
            yield return null;
            t += speed * Time.deltaTime;
        }
        
        scrollRect.normalizedPosition = targetNormPos;
    }
    
    static Vector2 CalculateScrollPosition(this ScrollRect scrollRect, Vector2 focusPoint) {
        var contentSize = scrollRect.content.rect.size;
        var viewportSize = ((RectTransform)scrollRect.content.parent).rect.size;
        var scrollPos = scrollRect.normalizedPosition;

        if (contentSize.y > viewportSize.y) {
            scrollPos.y = Mathf.Clamp01((focusPoint.y - viewportSize.y / 2f) / (contentSize.y - viewportSize.y));
        }

        return scrollPos;
    }
}