using UnityEngine;
using UnityEngine.UI;

public class Diary : MonoBehaviour, IInteractable
{
    public CanvasGroup readText;
    public CanvasGroup DiaryUI;
    public GameObject DiaryUIObject;
    public Button closeButton;
    public AudioClip readSound;
    public AudioClip exitSound;
    private AudioSource audioSource;
    private FirstPersonController playerController;

    void Start()
    {
        DiaryUIObject.SetActive(false);
        DiaryUI.alpha = 0;
        closeButton.onClick.AddListener(HideDiaryUI);

        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<FirstPersonController>();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Interact()
    {
        ShowDiaryUI();
    }

    public void ShowDiaryUI()
    {
        DiaryUIObject.SetActive(true);
        DiaryUI.alpha = 1;
        DiaryUI.interactable = true;
        DiaryUI.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.enabled = false;

        audioSource.PlayOneShot(readSound);
    }

    public void HideDiaryUI()
    {
        DiaryUIObject.SetActive(false);
        DiaryUI.alpha = 0;
        DiaryUI.interactable = false;
        DiaryUI.blocksRaycasts = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.enabled = true;

        audioSource.PlayOneShot(exitSound);
    }
}
