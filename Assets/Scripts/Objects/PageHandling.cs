using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageHandling : MonoBehaviour
{
    public GameObject Page1;
    public GameObject Page2;
    public GameObject Page3;
    public GameObject Diary;

    public AudioClip pageTurnSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource != null)
            audioSource.volume = 0.5f;
    }

    void pageTurnSFX()
    {
        audioSource.PlayOneShot(pageTurnSound);
    }

    public void ToPage1()
    { 
        Page1.SetActive(true);
        Page2.SetActive(false);
        Page3.SetActive(false);
        pageTurnSFX();
    }

    public void ToPage2()
    {
        Page1.SetActive(false);
        Page2.SetActive(true);
        Page3.SetActive(false);
        pageTurnSFX();
    }

    public void ToPage3()
    {
        Page1.SetActive(false);
        Page2.SetActive(false);
        Page3.SetActive(true);
        pageTurnSFX();
    }

    public void ExitDiary()
    {
        Diary.SetActive(false);
    }
}
