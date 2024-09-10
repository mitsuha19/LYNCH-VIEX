using System.Collections;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f;           // Speed of typing effect
    public AudioClip[] typingSounds;            // Array of sound effects for typing
    public float delayBeforeDeactivation = 2f;  // Time to wait before deactivating the object

    private TextMeshProUGUI textMeshPro;
    private AudioSource audioSource;
    private string fullText;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        fullText = textMeshPro.text;
        textMeshPro.text = "";
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            textMeshPro.text += letter;

            if (typingSounds.Length > 0)
            {
                // Play a random typing sound
                AudioClip randomSound = typingSounds[Random.Range(0, typingSounds.Length)];
                audioSource.PlayOneShot(randomSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait for a few seconds before deactivating the object
        yield return new WaitForSeconds(delayBeforeDeactivation);
        gameObject.SetActive(false);
    }
}
