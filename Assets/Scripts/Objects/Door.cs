using UnityEngine;
using System.Collections;
public class Door : MonoBehaviour, IInteractable
{
    public bool isClose;
    public bool isLocked = false;
    public string requiredKeyID;
    public AudioClip openSound;
    public AudioClip closeSound;
    public GameObject doorText;
    public GameObject lockedText;
    public AudioClip lockedSound;
    public AudioClip unlockedSound;

    public GameObject door;
    public GameObject doorHinge;
    private float startPoint = 0f;
    private float endPoint = -108f;

    public AudioSource audioSource;

    public Interactor interactor;

    void Start()
    {
        lockedText.SetActive(false);
        doorText.SetActive(false);
    }

    private void Update()
    {
        float currentYRotation = doorHinge.transform.localRotation.eulerAngles.y;

        if (currentYRotation > startPoint)
        {
            isClose = false;
        }
        else
        {
            isClose = true;
        }
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
        StopAllCoroutines();
        StartCoroutine(RotateDoor(startPoint, endPoint));
        audioSource.PlayOneShot(openSound);
    }

    private void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(RotateDoor(endPoint, startPoint));
        audioSource.PlayOneShot(closeSound);
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

    private IEnumerator RotateDoor(float fromAngle, float toAngle)
    {
        float rotationSpeed = 110f;
        float currentRotation = fromAngle;

        while (!Mathf.Approximately(currentRotation, toAngle))
        {
            if (IsBlocked())
            {
                yield return new WaitUntil(() => !IsBlocked());
            }

            currentRotation = Mathf.MoveTowards(currentRotation, toAngle, rotationSpeed * Time.deltaTime);
            doorHinge.transform.localRotation = Quaternion.Euler(0f, currentRotation, 0f);

            yield return null;
        }
    }

    private bool IsBlocked()
    {
        Vector3 rayOrigin = door.transform.position;
        Vector3 direction = door.transform.right;

        float maxDistance = 0.08f;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, direction, out hit, maxDistance))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                return true;
            }
        }
        return false;
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