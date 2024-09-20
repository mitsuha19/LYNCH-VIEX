using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractable
{
    public bool isClose = true;
    public bool isLocked = false;
    public string requiredKeyID;
    public AudioClip openSound;
    public AudioClip closeSound;
    public GameObject doorText;
    public GameObject lockedText;
    public AudioClip lockedSound;
    public AudioClip unlockedSound;
    public GameObject blockedDoorMonolouge;

    public GameObject doorHinge;
    private float startPoint = 0f;
    private float endPoint = -108f;

    public AudioSource audioSource;

    public Interactor interactor;

    private bool isAnimating = false; // Flag to check if the door is currently animating

    void Start()
    {
        lockedText.SetActive(false);
        doorText.SetActive(false);
        float localRotationY = doorHinge.transform.localEulerAngles.y;
        isClose = localRotationY <= startPoint;
    }

    public virtual void Interact()
    {
        // Prevent interaction if the door is currently animating
        if (isAnimating)
            return;

        if (CompareTag("BlockedDoor"))
        {
            blockedDoorMonolouge.SetActive(false);
            blockedDoorMonolouge.SetActive(true);
        }
        else if (CompareTag("EndDoor"))
        {
            if (isLocked)
            {
                if (!Inventory.HasItem(requiredKeyID))
                {
                    blockedDoorMonolouge.SetActive(false);
                    blockedDoorMonolouge.SetActive(true);
                }
            }
        }

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
        isClose = false;
    }

    private void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(RotateDoor(endPoint, startPoint));
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

    private IEnumerator RotateDoor(float fromAngle, float toAngle)
    {
        isAnimating = true; // Start animation flag
        float rotationSpeed = 144f;
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

        isAnimating = false; // End animation flag
    }

    public GameObject door;
    public float maxDistance;

    void OnDrawGizmos()
    {
        if (door != null)
        {
            Vector3 rayOrigin = door.transform.position;
            Vector3 direction = door.transform.right;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayOrigin, direction * maxDistance);
        }
    }

    private bool IsBlocked()
    {
        Vector3 rayOrigin = door.transform.position;
        Vector3 direction = door.transform.forward;
        float maxDistance;
        if (CompareTag("BlockedDoor"))
            maxDistance = 1f;
        else
            maxDistance = 0.8f;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, direction, out hit, maxDistance))
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            if (hit.collider.gameObject != this.gameObject && !hit.collider.CompareTag("DoorFrame"))
            {
                return true;
            }
        }

        return false;
    }

    #region texts
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
    #endregion
}
