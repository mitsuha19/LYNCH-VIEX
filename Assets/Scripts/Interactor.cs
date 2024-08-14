using System.Collections;
using System.Collections.Generic;
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
    private FirstPersonController playerController;
    private Note currentNote;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        playerController = player.GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null && currentNote == null)
            {
                Interact();
            }
            else if (currentNote != null)
            {
                currentNote.ShowNoteUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (heldObject != null)
            {
                bool isSprinting = playerController != null && playerController.IsSprinting;
                heldObject.Drop(isSprinting);
                heldObject = null;
            }
        }

        if (IsAimingAtPickableObject(out PickableObject pickableObject) && heldObject == null)
                ShowInteractionText();
        else
        {
            HideInteractionText();
            HideReadText();
            currentNote = null;
        }
        
        if (IsAimingAtNoteObject(out Note note))
        {
            currentNote = note;
            ShowReadText(note);
        }
    }

    private void Interact()
    {
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

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
                    currentNote.ShowNoteUI();
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
}
