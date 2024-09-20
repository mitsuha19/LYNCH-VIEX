using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;

    private bool isFadingOut = false;

    void Update()
    {
        if (isFadingOut)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeDuration;

            if (canvasGroup.alpha <= 0f)
            {
                canvasGroup.alpha = 0f;
                isFadingOut = false;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

            }
        }
    }

    public void StartFadeOut()
    {
        isFadingOut = true;
    }
}
