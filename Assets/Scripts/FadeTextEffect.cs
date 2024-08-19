using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeTextEffect : MonoBehaviour
{
    public float fadeDuration = 1f;       // Duration for the fade-in and fade-out
    public float displayDuration = 2f;    // Time to display the text before fading out
    public AudioClip notificationSound;   // Sound effect for notifications

    private TextMeshProUGUI textMeshPro;
    private AudioSource audioSource;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (notificationSound != null)
        {
            audioSource.clip = notificationSound;
            audioSource.Play();
        }

        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        // Fade-in
        float elapsedTime = 0f;
        Color color = textMeshPro.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            textMeshPro.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }

        // Display text for a while
        yield return new WaitForSeconds(displayDuration);

        // Fade-out
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            textMeshPro.color = new Color(color.r, color.g, color.b, 1 - Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }

        textMeshPro.color = new Color(color.r, color.g, color.b, 0); // Ensure the text is fully transparent
        gameObject.SetActive(false); // Optionally, deactivate the object after fading out
    }
}
