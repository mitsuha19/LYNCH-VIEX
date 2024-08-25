using UnityEngine;
using System.Collections;
public class Door : MonoBehaviour, IInteractable
{
    private bool isClose = true;
    public bool isLocked = false;
    public string requiredKeyID;
    public AudioClip openSound;
    public AudioClip closeSound;
    public GameObject doorText;
    public GameObject lockedText;
    public AudioClip lockedSound;
    public AudioClip unlockedSound;

    public AudioSource audioSource;
    private Animator animator;

    public Interactor interactor;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        lockedText.SetActive(false);
        doorText.SetActive(false);
    }

    public virtual void Interact()
    {
        if (isLocked)
        {
            if (Inventory.HasItem(requiredKeyID))
            {
                UnlockDoor();
            }
            else
            {
                StartCoroutine(ShowLockedTextWithDelay());
                audioSource.PlayOneShot(lockedSound);
                return;
            }
        }
        else
        {
            if (isClose)
                OpenDoor();
            else
                CloseDoor();
        }
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

    private void UnlockDoor()
    {
        isLocked = false;
        audioSource.PlayOneShot(unlockedSound);
    }

    private IEnumerator ShowLockedTextWithDelay()
    {
        HideDoorText();  
        lockedText.SetActive(true);  
        yield return new WaitForSeconds(0.5f); 
        lockedText.SetActive(false);
        if (interactor.IsAimingAtDoor(out Door door))
            ShowDoorText();
    }

    public void ShowDoorText()
    {
        doorText.SetActive(true);
    }

    public void HideDoorText()
    {
        doorText.SetActive(false);
    }

    public void HideLockedText()
    {
        lockedText.SetActive(false);
    }
}
