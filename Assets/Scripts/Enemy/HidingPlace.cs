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

    public AudioSource audioSource;   
    public AudioClip closetHideClip;  

    private bool hiding, playerInTrigger;
    private const string WardrobeDoorTag = "WardrobeDoor";

    private static HidingPlace activeHidingPlace;

    void Start()
    {
        hiding = false;
        playerInTrigger = false;
        //hideText.SetActive(false);
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


                FirstPersonController playerController = normalPlayer.GetComponent<FirstPersonController>();
                playerController.isExitingHidingPlace = true;

                if (closetHideClip != null)
                {
                    audioSource.PlayOneShot(closetHideClip);
                }
            }
            return;
        }

        if (playerInTrigger && IsAimingAtWardrobeDoor(out _))
        {
            if (activeHidingPlace == null || activeHidingPlace == this)
            {
                activeHidingPlace = this; 
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

                    if (!TutorialManager.tutorialShown)
                    {
                        tutorialText.SetActive(true); 
                        TutorialManager.tutorialShown = true; 
                    }

                    if (closetHideClip != null)
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
                activeHidingPlace = null; 
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
