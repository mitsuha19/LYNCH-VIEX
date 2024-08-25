using UnityEngine;

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

    private bool isInInventory = false; // Tracks if the diary is in the player's inventory
    private bool isDiaryVisible = false; // Tracks the current visibility of the diary

    void Start()
    {
        diaryUIObject.SetActive(false);
        audioSource = gameObject.AddComponent<AudioSource>();

        // If the tutorial has already been shown, ensure the tutorial text is inactive from the start
        if (DiaryTutorial.tutorialShown)
        {
            tutorialText.SetActive(false);
        }
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
        if (!isInInventory)
        {
            // First interaction: add the diary to the inventory
            isInInventory = true;
            ShowDiaryUI();

            // Show the tutorial text if it hasn't been shown yet
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


