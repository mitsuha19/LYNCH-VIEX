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
    private Notebook currentNotebook;
    private Fridge currentFridge;
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
        else if (IsAimingAtNotebook(out Notebook notebook))
        {
            currentNotebook = notebook;
            ShowNotebookText(notebook);
        }
        else if (IsAimingAtFridge(out Fridge fridge))
        {
            currentFridge = fridge;
            ShowFridgeText(fridge);
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
            HideNotebookText();
            currentNotebook = null;
            HideFridgeText();
            currentFridge = null;
        }
    }

    private void InteractWithTarget()
    {
        if (heldObject == null && currentNote == null && currentDoor == null && currentKey == null && currentNotebook == null && currentFridge == null)
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
        else if (currentNotebook != null)
        {
            currentNotebook.Interact();
        }
        else if (currentFridge != null)
        {
            currentFridge.Interact();
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
                interactObj.Interact();

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
                else if (interactObj is Notebook notebook)
                {
                    currentNotebook = notebook;
                    notebook.Interact();
                }
                else if (interactObj is Fridge fridge)
                {
                    currentFridge = fridge;
                    fridge.Interact();
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

    private bool IsAimingAtNotebook(out Notebook notebook)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            return hitInfo.collider.gameObject.TryGetComponent(out notebook);
        }
        notebook = null;
        return false;
    }

    private bool IsAimingAtFridge(out Fridge fridge)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            return hitInfo.collider.gameObject.TryGetComponent(out fridge);
        }
        fridge = null;
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

    private void HideDoorText()
    {
        if (currentDoor != null)
        {
            currentDoor.doorText.SetActive(false);
        }
    }

    private void ShowNotebookText (Notebook notebook)
    {
        notebook.readText.SetActive(true);
    }

    private void HideNotebookText()
    {
        if(currentNotebook != null)
        {
            currentNotebook.readText.SetActive(false);
        }
    }

    private void ShowFridgeText(Fridge fridge)
    {
        fridge.interactText.SetActive(true);
    }

    private void HideFridgeText()
    {
        if (currentFridge != null)
        {
            currentFridge.interactText.SetActive(false);
        }
    }
    #endregion
}