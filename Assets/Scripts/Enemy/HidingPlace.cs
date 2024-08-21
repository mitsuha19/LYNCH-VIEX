using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialManager
{
    public static bool tutorialShown = false;
}

public class HidingPlace : MonoBehaviour
{
    public GameObject hideText;
    public GameObject normalPlayer, hidingPlayer;
    public EnemyAI monsterScript;
    public Transform monsterTransform;
    public float InteractRange = 3f;
    public float loseDistance;
    public GameObject tutorialText;

    public AudioSource audioSource;   // AudioSource component for playing sounds
    public AudioClip closetHideClip;  // AudioClip for the ClosetHide sound

    private bool hiding, playerInTrigger;
    private const string WardrobeDoorTag = "WardrobeDoor";
    private const string ClosetTag = "Closet";

    // Static variable to keep track of the current active hiding place
    private static HidingPlace activeHidingPlace;

    void Start()
    {
        hiding = false;
        playerInTrigger = false;
        hideText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            hideText.SetActive(false);

            // Clear the active hiding place if the player exits the trigger
            if (activeHidingPlace == this)
            {
                activeHidingPlace = null;
            }
        }
    }

    void Update()
    {
        if (hiding)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                normalPlayer.SetActive(true);
                hidingPlayer.SetActive(false);
                hiding = false;
            }
            return;
        }

        // Only handle the hideText UI if this hiding place is the active one
        if (playerInTrigger && IsAimingAtWardrobeDoor(out _))
        {
            // If there is no active hiding place or the active hiding place is this one, display the hideText
            if (activeHidingPlace == null || activeHidingPlace == this)
            {
                activeHidingPlace = this; // Set this as the active hiding place
                hideText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hideText.SetActive(false);
                    hidingPlayer.SetActive(true);

                    float distance = Vector3.Distance(monsterTransform.position, normalPlayer.transform.position);
                    if (distance > loseDistance && monsterScript.chasing)
                    {
                        monsterScript.StopChase();
                    }

                    hiding = true;
                    normalPlayer.SetActive(false);

                    // Show the tutorial text only the first time the player hides
                    if (!TutorialManager.tutorialShown)
                    {
                        tutorialText.SetActive(true); // Activate the tutorial text
                        TutorialManager.tutorialShown = true; // Set the flag to prevent showing it again
                    }

                    // Play the "ClosetHide" sound if the hiding place is tagged as "Closet"
                    if (CompareTag(ClosetTag) && closetHideClip != null)
                    {
                        audioSource.PlayOneShot(closetHideClip);
                    }
                }
            }
        }
        else
        {
            if (activeHidingPlace == this)
            {
                hideText.SetActive(false);
                activeHidingPlace = null; // Clear the active hiding place if this one is no longer active
            }
        }
    }

    private bool IsAimingAtWardrobeDoor(out Collider wardrobeDoorCollider)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.CompareTag(WardrobeDoorTag))
            {
                wardrobeDoorCollider = hitInfo.collider;
                return true;
            }
        }

        wardrobeDoorCollider = null;
        return false;
    }
}
