using UnityEngine;

public static class CanvasGroupExt {

    public static void Hide(this CanvasGroup canvas) {
        canvas.alpha = 0f;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
    }

    public static void Show(this CanvasGroup canvas) {
        canvas.alpha = 1f;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
    }
}