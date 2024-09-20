using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeTextEffect : MonoBehaviour
{
    public float fadeDuration = 1f;      
    public float displayDuration = 2f;    
    public AudioClip notificationSound;   

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
        if(audioSource != null)
            audioSource.volume = 0.3f;

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
