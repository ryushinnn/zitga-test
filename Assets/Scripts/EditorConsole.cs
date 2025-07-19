using UnityEngine;

public static class EditorConsole {
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context = null)
    {
        if (context) {
            Debug.Log(message, context);
        } else {
            Debug.Log(message);
        }
    }
}