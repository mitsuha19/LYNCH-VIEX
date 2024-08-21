using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour, IInteractable
{
    public CanvasGroup readText; 
    public CanvasGroup noteUI;
    public GameObject noteUIObject;
    public Button closeButton; 
    public AudioClip readSound; 
    public AudioClip exitSound;
    private AudioSource audioSource; 
    private FirstPersonController playerController; 

    void Start()
    {
        noteUIObject.SetActive(false);
        noteUI.alpha = 0;
        closeButton.onClick.AddListener(HideNoteUI);
        
        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<FirstPersonController>();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Interact()
    {
        ShowNoteUI();
    }

    public void ShowNoteUI()
    {
            noteUIObject.SetActive(true);
            noteUI.alpha = 1;
            noteUI.interactable = true;
            noteUI.blocksRaycasts = true;

            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true; 

            playerController.enabled = false; 
            
            audioSource.PlayOneShot(readSound);
        }

    public void HideNoteUI()
    {
            noteUIObject.SetActive(false);
            noteUI.alpha = 0;
            noteUI.interactable = false;
            noteUI.blocksRaycasts = false;

            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;

            playerController.enabled = true;

            audioSource.PlayOneShot(exitSound);
    }
}
