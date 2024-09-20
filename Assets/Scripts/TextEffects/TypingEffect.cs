using System.Collections;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f;
    public AudioClip[] typingSounds;
    public float delayBeforeDeactivation = 2f;

    private TextMeshProUGUI textMeshPro;
    private AudioSource audioSource;
    private string fullText;
    private Coroutine typingCoroutine;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (audioSource != null)
            audioSource.volume = 0.5f;
        fullText = textMeshPro.text;
    }

    void OnEnable()
    {
        textMeshPro.text = "";
        typingCoroutine = StartCoroutine(TypeText());
    }

    void OnDisable()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        textMeshPro.text = "";
    }

    private IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            textMeshPro.text += letter;

            if (typingSounds.Length > 0)
            {
                AudioClip randomSound = typingSounds[Random.Range(0, typingSounds.Length)];
                audioSource.PlayOneShot(randomSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(delayBeforeDeactivation);

        gameObject.SetActive(false);
    }
}
