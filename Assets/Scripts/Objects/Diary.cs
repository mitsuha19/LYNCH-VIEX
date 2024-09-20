using UnityEngine;
using System.Collections;

public static class DiaryTutorial
{
    public static bool tutorialShown = false;

    public static void MarkTutorialAsShown()
    {
        tutorialShown = true;
    }
}

public class Diary : MonoBehaviour, IInteractable
{
    public CanvasGroup readText;
    public GameObject diaryUIObject;
    public AudioClip readSound;
    public AudioClip exitSound;
    private AudioSource audioSource;
    public FirstPersonController playerController;
    public GameObject tutorialText;
    public MeshRenderer diaryMesh;
    public MeshCollider diaryMeshCollider;
    public GameObject glowing;

    public GameObject mono1;  
    public GameObject mono2; 

    private bool monoDone = false; 

    private bool isInInventory = false;  
    private bool isDiaryVisible = false;

    void Start()
    {
        diaryUIObject.SetActive(false);
        audioSource = gameObject.AddComponent<AudioSource>();

        if (DiaryTutorial.tutorialShown)
        {
            tutorialText.SetActive(false);
        }

        mono1.SetActive(false);
        mono2.SetActive(false);
    }

    void Update()
    {
        if (isInInventory && Input.GetKeyDown(KeyCode.Tab))
        {
            if (isDiaryVisible)
            {
                HideDiaryUI();
            }
            else
            {
                ShowDiaryUI();
            }
        }
    }

    public void Interact()
    {
        diaryMesh.enabled = false;
        diaryMeshCollider.enabled = false;
        glowing.SetActive(false);

        if (!monoDone)
        {
            StartCoroutine(PlayMonologues());
        }

        if (!isInInventory)
        {
            isInInventory = true;
            ShowDiaryUI();

            if (!DiaryTutorial.tutorialShown)
            {
                tutorialText.SetActive(true);
                DiaryTutorial.MarkTutorialAsShown();
            }
        }
        else
        {
            if (isDiaryVisible)
            {
                HideDiaryUI();
            }
            else
            {
                ShowDiaryUI();
            }
        }
    }

    private IEnumerator PlayMonologues()
    {
        mono1.SetActive(true);
        yield return new WaitForSeconds(5f);  

        mono1.SetActive(false);
        mono2.SetActive(true);
        yield return new WaitForSeconds(5f);  

        mono2.SetActive(false);

        monoDone = true;
    }

    public void ShowDiaryUI()
    {
        diaryUIObject.SetActive(true);
        isDiaryVisible = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.enabled = false;

        audioSource.PlayOneShot(readSound);
    }

    public void HideDiaryUI()
    {
        audioSource.volume = 0.5f;
        diaryUIObject.SetActive(false);
        isDiaryVisible = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.enabled = true;

        audioSource.PlayOneShot(exitSound);
    }
}
