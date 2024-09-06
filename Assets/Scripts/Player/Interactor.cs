using System.Collections;
using UnityEngine;
interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange = 5f;
    public Transform HoldingPosition;
    public Transform PutAwayPosition;
    public CanvasGroup interactionText;
    public Lighter heldObject;
    private Note currentNote;
    private Door currentDoor;
    private Diary currentDiary;
    private Key currentKey;
    private PadLockRay currentPadlock;


    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithTarget();
        }
        if (IsAimingAtPickableObject(out Lighter pickableObject) && heldObject == null)
        {
            ShowInteractionText();
        }
        else if (IsAimingAtNoteObject(out Note note))
        {
            currentNote = note;
            ShowNoteText(note);
        }
        else if (IsAimingAtDiaryObject(out Diary diary))
        {
            currentDiary = diary;
            ShowDiaryText(diary);
        }
        else if (IsAimingAtKeyObject(out Key key))
        {
            currentKey = key;
            ShowKeyText(key);
        }
        else if (IsAimingAtPadlockObject(out PadLockRay padlock))
        {
            currentPadlock = padlock;
            ShowPadlockText(padlock);
        }
        else if (IsAimingAtDoor(out Door door))
        {
            if (currentDoor != door)
            {
                if (currentDoor != null)
                {
                    currentDoor.HideDoorText();
                    currentDoor = null;
                }
                currentDoor = door;
                currentDoor.ShowDoorText();  
            }
        }
        else
        {
            HideInteractionText();
            HideNoteText();
            currentNote = null;
            HideDiaryText();
            currentDiary = null;
            HideKeyText();
            currentKey = null;
            HideDoorText();
            currentDoor = null;
            HidePadlockText();
            currentPadlock = null;
        }
    }

    private void InteractWithTarget()
    {
        if (heldObject == null && currentNote == null && currentDoor == null && currentKey == null)
        {
            Interact();
        }
        else if (currentNote != null)
        {
            currentNote.ShowNoteUI();
        }
        else if (currentDiary != null)
        {
            currentDiary.Interact();
        }
        else if (currentDoor != null)
        {
            currentDoor.Interact();  
        }
        else if (currentKey != null)
        {
            currentKey.Interact();
        }
    }

    private void Interact()
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Debug.DrawLine(r.origin, r.origin + r.direction * InteractRange, Color.red, 2.0f);

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {

                Debug.Log("Interacting with: " + interactObj);

                if (interactObj != null)
                {
                    interactObj.Interact();  
                }
                else
                {
                    Debug.LogError("interactObj is null!");
                }

                if (interactObj is Lighter pickableObject)
                {
                    heldObject = pickableObject;
                }
                else if (interactObj is Note note)
                {
                    currentNote = note;
                    currentNote.Interact();
                }
                else if (interactObj is Door door)
                {
                    currentDoor = door;
                    door.Interact();
                }
            }
        }
    }

    #region aimRaycasts
    private bool IsAimingAtPickableObject(out Lighter pickableObject)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            return hitInfo.collider.gameObject.TryGetComponent(out pickableObject);
        }
        pickableObject = null;
        return false;
    }

    private bool IsAimingAtNoteObject(out Note note)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            return hitInfo.collider.gameObject.TryGetComponent(out note);
        }
        else
        {
            note = null;
            return false;
        }
    }

    private bool IsAimingAtDiaryObject(out Diary diary)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            return hitInfo.collider.gameObject.TryGetComponent(out diary);
        }
        else
        {
            diary = null;
            return false;
        }
    }

    public bool IsAimingAtDoor(out Door door)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            return hitInfo.collider.gameObject.TryGetComponent(out door);
        }

        door = null;
        return false;
    }

    private bool IsAimingAtKeyObject(out Key key)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            return hitInfo.collider.gameObject.TryGetComponent(out key);
        }
        key = null;
        return false;
    }

    private bool IsAimingAtPadlockObject(out PadLockRay padlock)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            return hitInfo.collider.gameObject.TryGetComponent(out padlock);
        }

        padlock = null;
        return false;   
    }

    #endregion

    #region interactTexts
    private void ShowInteractionText()
    {
        interactionText.alpha = 1;
        interactionText.interactable = true;
        interactionText.blocksRaycasts = true;
    }

    private void HideInteractionText()
    {
        interactionText.alpha = 0;
        interactionText.interactable = false;
        interactionText.blocksRaycasts = false;
    }

    private void ShowNoteText(Note note)
    {
        note.readText.alpha = 1;
        note.readText.interactable = true;
        note.readText.blocksRaycasts = true;
    }

    private void HideNoteText()
    {
        if (currentNote != null)
        {
            currentNote.readText.alpha = 0;
            currentNote.readText.interactable = false;
            currentNote.readText.blocksRaycasts = false;
        }
    }

    private void ShowKeyText(Key key)
    {
        currentKey.keyText.SetActive(true);
    }

    private void HideKeyText()
    {
        if (currentKey != null)
        {
            currentKey.keyText.SetActive(false);
        }
    }

    private void ShowDiaryText(Diary diary)
    {
        diary.readText.alpha = 1;
        diary.readText.interactable = true;
        diary.readText.blocksRaycasts = true;
    }

    private void HideDiaryText()
    {
        if (currentDiary != null)
        {
            currentDiary.readText.alpha = 0;
            currentDiary.readText.interactable = false;
            currentDiary.readText.blocksRaycasts = false;
        }
    }

    public void ShowDoorText(Door door)
    {
        door.doorText.SetActive(true);
    }

    private void HideLockedText()
    {
        if (currentDoor != null)
        {
            currentDoor.lockedText.SetActive(false);
        }
    }

    private void HideDoorText()
    {
        if (currentDoor != null)
        {
            currentDoor.doorText.SetActive(false);
        }
    }

    private void ShowPadlockText(PadLockRay padlock)
    {
            
            padlock.padlockTextObject.SetActive(true);
    }

    private void HidePadlockText()
    {
            if (currentPadlock != null)
            {
                currentPadlock.padlockTextObject.SetActive(false);
            }
    }
    #endregion

}