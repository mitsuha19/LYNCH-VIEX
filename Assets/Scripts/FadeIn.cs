using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;

    private bool isFadingIn = false;

    void Update()
    {
        if (isFadingIn)
        {
            canvasGroup.alpha += Time.deltaTime / fadeDuration;

            if (canvasGroup.alpha >= 1f)
            {
                canvasGroup.alpha = 1f;
                isFadingIn = false;
            }
        }
    }

    public void StartFadeIn()
    {
        isFadingIn = true;
        canvasGroup.alpha = 0;
    }
}
