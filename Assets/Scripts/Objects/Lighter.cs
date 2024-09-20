using UnityEngine;
using System.Collections;

public static class LighterTutorial
{
    public static bool tutorialShown = false;

    public static void MarkTutorialAsShown()
    {
        tutorialShown = true;
    }
}

public class Lighter : MonoBehaviour, IInteractable
{
    private bool isHeld = false;
    private bool isPutAway = false;
    private bool isClosed = true;
    private Transform holder;
    private Coroutine moveCoroutine;
    private Rigidbody rb;
    private Collider objectCollider;
    private Animator animator;
    private Transform putAwayPosition;
    private GameObject lighterEffects;
    public GameObject Glowing;

    public GameObject tutorialText;

    [SerializeField] private AudioSource lighterAudioSource = default;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();
        lighterEffects = GameObject.Find("LighterFlame");

        if (lighterEffects != null)
        {
            lighterEffects.SetActive(false);
        }

        if (LighterTutorial.tutorialShown)
        {
            tutorialText.SetActive(false);
        }
    }

    private void Update()
    {
        if (isHeld && !isPutAway && Input.GetKeyDown(KeyCode.F))
        {
            PutAway();
            animator.SetTrigger("TrClose");
            isClosed = true;
            ToggleLighterEffects(false);

            PlaySound(closeClip);
        }
        else if (isHeld && isPutAway && Input.GetKeyDown(KeyCode.F))
        {
            TakeOut();
            animator.SetTrigger("TrOpen");
            isClosed = false;
            ToggleLighterEffects(true);

            PlaySound(openClip);
        }
    }

    private void ToggleLighterEffects(bool isEnabled)
    {
        if (lighterEffects != null)
        {
            lighterEffects.SetActive(isEnabled);
        }
    }

    public void Interact()
    {
        if (!LighterTutorial.tutorialShown)
        {
            tutorialText.SetActive(true);
            LighterTutorial.MarkTutorialAsShown();
        }

        if (isHeld)
        {
            Drop(false);
        }
        else
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Interactor interactor = player.GetComponent<Interactor>();
        holder = interactor.HoldingPosition;
        putAwayPosition = interactor.PutAwayPosition;

        isHeld = true;
        Glowing.SetActive(false);
        isPutAway = false;

        if (CompareTag("Lighter"))
        {
            if (isClosed)
            {
                animator.SetTrigger("TrOpen");
                isClosed = false;
                ToggleLighterEffects(true);

                PlaySound(openClip);
            }
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        float moveSpeed = 100f;
        float rotationSpeed =  50.0f;

        moveCoroutine = StartCoroutine(MoveToHolder(holder, moveSpeed, rotationSpeed));

        rb.isKinematic = true;
        DisableCollisionsWithPlayer(true);
        gameObject.SetActive(true);
    }

    public void Drop(bool isSprinting)
    {
        isHeld = false;
        isPutAway = false;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        transform.SetParent(null);
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        Vector3 dropForceDirection = Camera.main.transform.forward;
        float dropForce = isSprinting ? 5.0f : 2.0f;

        rb.AddForce(dropForceDirection * dropForce, ForceMode.Impulse);

        DisableCollisionsWithPlayer(false);

        GameObject player = GameObject.FindWithTag("Player");
        Interactor interactor = player.GetComponent<Interactor>();
        interactor.heldObject = null;
    }

    private Vector3 velocity = Vector3.zero;
    private IEnumerator MoveToHolder(Transform target, float moveSpeed, float rotationSpeed)
    {
        while (isHeld && target != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, 0.05f, moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.SetParent(target);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }


    private IEnumerator MoveToInventory(Transform target)
    {
        while (isHeld && target != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, 0.05f, 100f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * 100f);
            yield return null;
        }

        transform.SetParent(target);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void PutAway()
    {
        if (putAwayPosition == null) return;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToInventory(putAwayPosition));
        isPutAway = true;
    }

    private void TakeOut()
    {
        if (holder == null) return;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToHolder(holder, 10.0f , 50.0f));
        isPutAway = false;
    }

    private void DisableCollisionsWithPlayer(bool ignore)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(objectCollider, playerCollider, ignore);
            }

            Collider[] playerColliders = player.GetComponentsInChildren<Collider>();
            foreach (var col in playerColliders)
            {
                Physics.IgnoreCollision(objectCollider, col, ignore);
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (lighterAudioSource != null && clip != null)
        {
            lighterAudioSource.PlayOneShot(clip);
        }
    }
}
