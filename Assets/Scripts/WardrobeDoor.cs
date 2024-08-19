using UnityEngine;

public class WardrobeDoor : MonoBehaviour, IInteractable
{
    public bool isClose = true;
    public AudioClip openSound;
    public AudioClip closeSound;
    public CanvasGroup openText; 

    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!isClose)
            CloseDoor();
        else
            OpenDoor();
    }

    private void OpenDoor()
    {
        animator.SetTrigger("TrOpen"); 
        audioSource.PlayOneShot(openSound);
        isClose = false;
    }

    private void CloseDoor()
    {
        animator.SetTrigger("TrClose"); 
        audioSource.PlayOneShot(closeSound);
        isClose = true;
    }
}
