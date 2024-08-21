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
    public PickableObject heldObject;
    private Note currentNote;
    private Door currentDoor;   

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null && currentNote == null && currentDoor == null)
            {
                Interact();
            }
            else if (currentNote != null)
            {
                currentNote.ShowNoteUI();
            }
            else if (currentDoor != null)
            {
                currentDoor.Interact();
            }
        }

        if (IsAimingAtPickableObject(out PickableObject pickableObject) && heldObject == null)
        {
            ShowInteractionText();
        }
        else if (IsAimingAtNoteObject(out Note note))
        {
            currentNote = note;
            ShowReadText(note);
        }
        else if (IsAimingAtDoor(out Door door))
        {
            currentDoor = door;
            ShowOpenText(door);
        }
        else
        {
            HideInteractionText();
            HideReadText();
            currentNote = null;
            HideOpenText();
            currentDoor = null;
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
                if (interactObj is PickableObject pickableObject)
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

    private bool IsAimingAtPickableObject(out PickableObject pickableObject)
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

    private bool IsAimingAtDoor(out Door door)
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out door))
            {
                return true;
            }

            if (hitInfo.collider.bounds.Intersects(hitInfo.collider.bounds))
            {
                door = hitInfo.collider.GetComponent<Door>();
                return door != null;
            }
        }
        door = null;
        return false;
    }

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

    private void ShowReadText(Note note)
    {
        note.readText.alpha = 1;
        note.readText.interactable = true;
        note.readText.blocksRaycasts = true;
    }

    private void HideReadText()
    {
        if (currentNote != null)
        {
            currentNote.readText.alpha = 0;
            currentNote.readText.interactable = false;
            currentNote.readText.blocksRaycasts = false;
        }
    }

    private void ShowOpenText(Door door)
    {
        door.openText.alpha = 1;
        door.openText.interactable = true;
        door.openText.blocksRaycasts = true;
    }

    private void HideOpenText()
    {
        if (currentDoor != null)
        {
            currentDoor.openText.alpha = 0;
            currentDoor.openText.interactable = false;
            currentDoor.openText.blocksRaycasts = false;
        }
    }
}
